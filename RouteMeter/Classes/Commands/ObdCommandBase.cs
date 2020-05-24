using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static RouteMeter.Classes.AndroidOBDBluetoothConnection;

namespace RouteMeter.Classes.Commands
{
  public abstract class ObdCommandBase<T>
  {
    protected const string PREFIX_SEND_SETUP = "AT";
    protected const string PREFIX_SEND_DATA_CMD = "01";
    protected const string PREFIX_RECEIVE_DATA = "41";
    protected const string PREFIX_TROUBLE_CODES = "03";

    protected object fSyncObject = new object();

    protected BluetoothSocket Socket { get; set; }

    protected abstract string CommandPrefix { get; }
    protected abstract string BaseCommand { get; }
    protected string CommandParameter { get; set; }

    public delegate void DataReceivedDelegate(T aData);

    public ObdCommandBase(BluetoothSocket aSocket, string aCommandParameter = null)
    {
      Socket = aSocket;
      CommandParameter = aCommandParameter;
    }

    /// <summary>
    /// Send the command to the bluetooth socket. Returns true when command was sent and a response was received.
    /// </summary>
    /// <param name="aCallbackDataReceived">Callback for the received data.</param>
    /// <returns>True when command was sent and a response was received.</returns>
    public bool Send(DataReceivedDelegate aCallbackDataReceived)
    {
      if (Socket.IsConnected)
      {
        lock (fSyncObject)
        {
          byte[] lCmd = Encoding.ASCII.GetBytes(BuildCommand(CommandPrefix, BaseCommand, CommandParameter));
          Socket.OutputStream.Write(lCmd, 0, lCmd.Length);
          Socket.OutputStream.Flush();
          Thread.Sleep(50);

          string lResponse = ReadResponse();
          if (!string.IsNullOrWhiteSpace(lResponse))
          {
            string lExtractedData = ValidateAndExtractData(lResponse);
            if (!string.IsNullOrWhiteSpace(lExtractedData))
            {
              if (GetValue(lResponse, out T lValue))
              {
                aCallbackDataReceived?.Invoke(lValue);
                return true;
              }
            }
          }
        }
      }

      return false;
    }

    protected virtual string BuildCommand(string aPrefix, string aBaseCommand, string aCommandParameter)
    {
      string lCommand = string.Empty;
      if (string.IsNullOrEmpty(aCommandParameter))
        lCommand = string.Format("{0} {1}", aPrefix, aBaseCommand);
      else
        lCommand = string.Format("{0} {1} {2}", aPrefix, aBaseCommand, aCommandParameter);

      return string.Format("{0}\r", lCommand);
    }

    protected virtual string ValidateAndExtractData(string aRawData)
    {
      string lResult = string.Empty;

      if (aRawData.StartsWith(PREFIX_RECEIVE_DATA))
        lResult = aRawData.Substring(3);
      else
        return string.Empty;

      if (lResult.StartsWith(CommandPrefix))
        lResult = lResult.Substring(3);
      else
        return string.Empty;

      return lResult;
    }

    protected abstract bool GetValue(string aRawData, out T aExtractedData);
    //return (T)Convert.ChangeType(aInput, typeof(T));

    public string ReadResponse()
    {
      string lResult = string.Empty;
      StringBuilder lData = new StringBuilder();
      try
      {
        char c;
        while ((c = (char)Socket.InputStream.ReadByte()) > -1) // -1 = end of stream
        {
          if (c == '>') // > = end of data
            break;
          lData.Append(c);
        }
      }
      catch (Exception e)
      {
        System.Console.WriteLine(e.ToString());
      }

      lResult = RemoveErrors(lData.ToString());

      return lResult;
    }


    /// <summary>
    /// Remove any error message code from the received raw data.
    /// </summary>
    /// <param name="aInput">Input</param>
    /// <returns></returns>
    protected string RemoveErrors(string aInput)
    {
      string lResult = aInput;

      lResult.Replace("UNABLE TO CONNECT", "");
      lResult.Replace("SEARCHING", "");
      lResult.Replace("ERROR", "");
      lResult.Replace("NO DATA", "");
      lResult.Replace("STOPPED", "");
      lResult.Replace("INIT", "");
      lResult.Replace("BUS", "");
      lResult.Replace("?", "");
      lResult.Replace(".", "");
      lResult.Replace("\\s", "");
      lResult.Replace("\\r", "");

      return lResult.Trim();
    }
  }
}