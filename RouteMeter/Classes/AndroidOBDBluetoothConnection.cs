using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Net;
using Java.Util;
using RouteMeter.Classes.Commands;
using RouteMeter.Helper;
using Exception = System.Exception;
using Thread = System.Threading.Thread;

namespace RouteMeter.Classes
{
  public class AndroidOBDBluetoothConnection
  {
    #region Static Properties
    protected static AndroidOBDBluetoothConnection fCurrent;
    public static AndroidOBDBluetoothConnection Current
    {
      get
      {
        if (fCurrent == null)
          fCurrent = new AndroidOBDBluetoothConnection();

        return fCurrent;
      }
      set
      {
        fCurrent = value;
      }
    }
    #endregion

    #region Constants
    /// <summary>
    /// Standard UUID for this type of connection.
    /// </summary>
    protected const string CONNECTION_UUID = "00001101-0000-1000-8000-00805F9B34FB";

    /// <summary>
    /// Standard timeout in seconds.
    /// </summary>
    protected const int CONNECTION_TIMEOUT = 30;

    /// <summary>
    /// Standard timeout for enabling the bluetooth adapter in milliseconds.
    /// </summary>
    protected const int BLUETOOTH_ENABLE_TIMEOUT = 5000;

    /// <summary>
    /// Default time between data updates in milliseconds.
    /// </summary>
    protected const int DEFAULT_SAMPLE_PERIOD = 500;

    public const string DEFAULT_OBDII = "OBDII";
    public const string PATTERN_OBD = "OBD";
    #endregion

    public BluetoothDevice ConnectedDevice { get => ConnectedSocket?.RemoteDevice; }

    protected BluetoothSocket ConnectedSocket { get; set; }
    protected Task RunningWorkerTask { get; set; }
    protected CancellationTokenSource CancelToken { get; set; }

    public bool IsConnected { get => ConnectedDevice != null; }

    public delegate void DataReceivedDelegate(string aData);
    public event DataReceivedDelegate OnDataReceived;
    public event Action OnWorkerTaskEnded;

    public bool IsBluetoothEnabled => BluetoothAdapter.DefaultAdapter?.IsEnabled ?? false;
    public List<BondedDevice> BondedDeviceNames => BluetoothAdapter.DefaultAdapter?.BondedDevices.Select(x => new BondedDevice(x.Name)).ToList();

    /// <summary>
    /// Start a seperate worker task that uses a bluetooth connection to exchange data.
    /// </summary>
    /// <param name="aDeviceName">Device to connect to.</param>
    /// <param name="aCallbackError">Called when there was an error while connecting to a device.</param>
    /// <param name="aSamplePeriod">Time period in milliseconds.</param>
    public void Start(string aDeviceName, Action aCallbackError, int aSamplePeriod = DEFAULT_SAMPLE_PERIOD)
    {
      Task.Run(async () =>
      {
        CancellationTokenSource lPreviousToken = CancelToken;
        CancelToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken());

        if (lPreviousToken != null)
        {
          // Cancel the previous task and wait for its termination
          lPreviousToken.Cancel();
          try { await RunningWorkerTask; } catch { }
        }

        RunningWorkerTask = WorkerTask(aDeviceName, aCallbackError, aSamplePeriod);
        await RunningWorkerTask;
      });
    }

    public void Cancel()
    {
      CancelToken?.Cancel();
      if (ConnectedSocket != null)
      {
        ConnectedSocket.Close();
        ConnectedSocket = null;
      }
    }

    private async Task WorkerTask(string aDeviceName, Action aCallbackError, int aSamplePeriod)
    {
      DateTime lLastDataReceived = DateTime.Now;
      //OnDataReceived?.Invoke("SamplePeriod: Data updated every " + aSamplePeriod + " ms");

      if (await InitBluetoothSocket(aDeviceName) == false)
      {
        aCallbackError?.Invoke();
        return;
      }

      InitConnectedOdbDevice();

      while (CancelToken.IsCancellationRequested == false)
      {
        try
        {
          await Task.Delay(aSamplePeriod);

          bool lNewDataReceived = false;

          //TODO OBD fault codes at start and end of route

          //connect command calls via | operator
          lNewDataReceived =
            new SpeedCommand(ConnectedSocket).Send((value) => { OnDataReceived?.Invoke(value.ToString()); ObdDataProvider.Current.Speed.Update(value); });

          if (lNewDataReceived)
            lLastDataReceived = DateTime.Now;

          if (lLastDataReceived.AddSeconds(CONNECTION_TIMEOUT) < DateTime.Now)
          {
            OnDataReceived?.Invoke(string.Format("No data read from buffer for {0} seconds. Stopping worker task.", CONNECTION_TIMEOUT));
            break;
          }

          #region Old
          //lDevice = InitDevice(aDeviceName);

          //OnDataReceived?.Invoke(string.Format("Trying device {0} on UUID: {1}", lDevice.Name, CONNECTION_UUID));
          //UUID lUuid = UUID.FromString(CONNECTION_UUID);
          //if ((int)Android.OS.Build.VERSION.SdkInt >= 10)
          //  lBthSocket = lDevice.CreateInsecureRfcommSocketToServiceRecord(lUuid);
          //else
          //  lBthSocket = lDevice.CreateRfcommSocketToServiceRecord(lUuid);

          //if (lBthSocket != null)
          //{
          //  OnDataReceived?.Invoke(string.Format("Socket created on device {0} on UUID {1}", lDevice.Name, CONNECTION_UUID));
          //  await lBthSocket.ConnectAsync();

          //  if (lBthSocket.IsConnected)
          //  {
          //    OnDataReceived?.Invoke(string.Format("Socket connected on device {0} on UUID {1}", lDevice.Name, CONNECTION_UUID));
          //    using (InputStreamReader lReader = new InputStreamReader(lBthSocket.InputStream))
          //    {
          //      using(OutputStreamWriter lWriter = new OutputStreamWriter(lBthSocket.OutputStream))
          //      {
          //        while (CancelToken.IsCancellationRequested == false)
          //        {
          //          if (lReader.Ready())
          //          {
          //            string lData = string.Empty;
          //            char[] lBuffer = new char[100];
          //            await lReader.ReadAsync(lBuffer);

          //            foreach (char c in lBuffer)
          //            {
          //              if (c == '\0')
          //                break;
          //              lData += c;
          //            }

          //            if (lData.Length > 0)
          //              OnDataReceived?.Invoke(string.Format("Data received: {0}", lData));
          //            else
          //              OnDataReceived?.Invoke("No data received.");

          //            lLastBufferRead = DateTime.Now;
          //          }

          //          //No data read in x seconds -> End Task
          //          if (lLastBufferRead.AddSeconds(CONNECTION_TIMEOUT) < DateTime.Now)
          //          {
          //            OnDataReceived?.Invoke(string.Format("No data read from buffer for {0} seconds. Trying next UUID.", CONNECTION_TIMEOUT));
          //            lLastBufferRead = DateTime.Now;
          //            break;
          //          }

          //          await lSleeper; //Sleep for a period of time;
          //        }
          //      }
          //    }
          //  }
          //}
          #endregion
        }
        //catch (Java.IO.IOException aConnectionException)
        //{
        //  aCallbackError?.Invoke();
        //  break;
        //}
        catch (Exception ex)
        {
          string exm = ex.Message;
          OnDataReceived?.Invoke(exm);
          Cancel();
        }
      }

      ConnectedSocket = null;
      OnWorkerTaskEnded?.Invoke();
    }

    private async Task<bool> InitBluetoothSocket(string aDeviceName)
    {
      BluetoothAdapter lAdapter = BluetoothAdapter.DefaultAdapter;

      if (lAdapter == null)
        return false;

      if (!lAdapter.IsEnabled)
        return false;

      BluetoothDevice lDevice = lAdapter.BondedDevices.Where(x => x.Name.ToUpper() == aDeviceName.ToUpper()).FirstOrDefault();

      if (lDevice == null)
        return false;

      OnDataReceived?.Invoke(string.Format("Trying device {0} on UUID: {1}", lDevice.Name, CONNECTION_UUID));

      BluetoothSocket lBthSocket = null;

      UUID lUuid = UUID.FromString(CONNECTION_UUID);
      if ((int)Android.OS.Build.VERSION.SdkInt >= 10)
        lBthSocket = lDevice.CreateInsecureRfcommSocketToServiceRecord(lUuid);
      else
        lBthSocket = lDevice.CreateRfcommSocketToServiceRecord(lUuid);

      if (lBthSocket != null)
      {
        OnDataReceived?.Invoke(string.Format("Socket created on device {0} on UUID: {1}", lDevice.Name, CONNECTION_UUID));
        try
        {
          await lBthSocket.ConnectAsync(); //Only throws error when in try catch for some reason.
        }
        catch (Exception aException)
        {
          return false;
        }

        if (lBthSocket.IsConnected)
        {
          OnDataReceived?.Invoke(string.Format("Socket connected on device {0} on UUID: {1}", lDevice.Name, CONNECTION_UUID));
          ConnectedSocket = lBthSocket;
          return true;
        }
        else
        {
          return false;
        }
      }
      else
      {
        return false;
      }
    }

    private void InitConnectedOdbDevice()
    {
      new SetToDefaultCommand(ConnectedSocket).Send(null);
      new ResetCommand(ConnectedSocket).Send(null);
      new EchoOffCommand(ConnectedSocket).Send(null);
      new LineFeedOffCommand(ConnectedSocket).Send(null);
      new TimeOutCommand(ConnectedSocket, 500).Send(null);
      new ProtocolSelectionCommand(ConnectedSocket, ObdProtocols.AUTO).Send(null);
      OnDataReceived?.Invoke("OBD connection initialized. Hopefully...");
    }

    public void EnableBluetooth(Action<bool> aEnabledCallback)
    {
      BluetoothAdapter lAdapter = BluetoothAdapter.DefaultAdapter;
      lAdapter?.Enable();

      Task.Run(async () =>
      {
        DateTime lTime = DateTime.Now;
        while (lTime.AddMilliseconds(BLUETOOTH_ENABLE_TIMEOUT) > DateTime.Now)
        {
          if (lAdapter.IsEnabled)
          {
            aEnabledCallback?.Invoke(true);
            return;
          }

          await Task.Delay(10);
        }

        aEnabledCallback?.Invoke(false);
      });
    }
  }

  public class BondedDevice : IListPrompt
  {
    public BondedDevice(string aText)
    {
      fText = aText;
    }

    private string fText = string.Empty;
    public string Text { get => fText; set => fText = value; }
  }
}