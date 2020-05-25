using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using RouteMeter.Classes;
using RouteMeter.Dialogs;
using RouteMeter.Helper;
using static RouteMeter.Helper.PreferenceHelper;
using Fragment = Android.Support.V4.App.Fragment;

namespace RouteMeter.Fragments
{
  class FragmentMain : FragmentBase
  {
    public List<string> DebugLog { get; set; } = new List<string>();

    public override View OnCreateView(LayoutInflater aInflater, ViewGroup aContainer, Bundle aSavedInstanceState)
    {
      return aInflater.Inflate(Resource.Layout.fragmentMain, aContainer, false);
    }

    public override void OnViewCreated(View aView, Bundle aSavedInstanceState)
    {
      base.OnViewCreated(aView, aSavedInstanceState);
      Activity.SetTitle(Resource.String.app_name);

      ObdDataProvider.OnDataUpdated += delegate
      {
        RunOnUiThread(UpdateView);
      };

      //AndroidOBDBluetoothConnection.Current.OnDataReceived += (aData) =>
      //{
      //  AddToLog(aData);
      //  RunOnUiThread(UpdateView);
      //};

      AndroidOBDBluetoothConnection.Current.OnWorkerTaskEnded += () =>
      {
        AddToLog("A worker thread as ended.");
        RunOnUiThread(UpdateView);
      };

      StartConnection();
      UpdateView();
    }

    public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
    {
      inflater.Inflate(Resource.Menu.menu_main, menu);
      base.OnCreateOptionsMenu(menu, inflater);
    }

    public override bool OnOptionsItemSelected(IMenuItem aItem)
    {
      switch (aItem.ItemId)
      {
        case Resource.Id.menu_retry_connection:
          StartConnection();
          break;
        case Resource.Id.menu_preferences:
          new DlgPreferences(Activity).Show();
          break;
      }

      return base.OnOptionsItemSelected(aItem);
    }

    public void OnClick(View v)
    {
      switch (v.Id)
      {

      }
    }

    private void AddToLog(string aString)
    {
      if (DebugLog.Count >= 100)
        DebugLog.RemoveAt(0);

      DebugLog.Add(aString);
    }

    private void StartConnection()
    {
      if (AndroidOBDBluetoothConnection.Current.IsBluetoothEnabled)
      {
        AndroidOBDBluetoothConnection.Current.Start(AndroidOBDBluetoothConnection.DEFAULT_OBDII, () =>
        {
          string lPossibleObdDevice = AndroidOBDBluetoothConnection.Current.BondedDeviceNames.Where(x =>
            x.Text.ToUpper() != AndroidOBDBluetoothConnection.DEFAULT_OBDII && x.Text.ToUpper().Contains(AndroidOBDBluetoothConnection.PATTERN_OBD)).FirstOrDefault()?.Text;
          if (string.IsNullOrWhiteSpace(lPossibleObdDevice))
          {
            SelectOtherDevice(() =>
            {
              RunOnUiThread(() =>
              {
                DialogHelper.ShowToast(Activity, Resource.String.hint_pairing, ToastLength.Long);
                DialogHelper.DisplayYesNoPrompt(Activity, Resource.String.question_try_again, (sender, e) =>
                {
                  StartConnection();
                }, null);
              });
            });
          }
          else
          {
            AndroidOBDBluetoothConnection.Current.Start(lPossibleObdDevice, () =>
            {
              SelectOtherDevice(() =>
              {
                RunOnUiThread(() =>
                {
                  DialogHelper.ShowToast(Activity, Resource.String.hint_pairing, ToastLength.Long);
                  DialogHelper.DisplayYesNoPrompt(Activity, Resource.String.question_try_again, (sender, e) =>
                  {
                    StartConnection();
                  }, null);
                });
              });
            });
          }
        });
      }
      else
      {
        AskEnableBluetooth((aEnabled, aActionByUser) =>
        {
          if (aEnabled)
            StartConnection();
          else if (!aActionByUser)
          {
            RunOnUiThread(() =>
            {
              DialogHelper.DisplayYesNoPrompt(Activity, Resource.String.question_try_again, (sender, e) =>
              {
                StartConnection();
              }, null);
            });
          }
        });
      }
    }

    private void AskEnableBluetooth(Action<bool, bool> aCallbackBluetoothEnabled)
    {
      RunOnUiThread(() =>
      {
        DialogHelper.DisplayYesNoPrompt(Activity, Resource.String.question_enable_bluetooth, (sender, e) =>
        {
          ProgressDialog lDlg = DialogHelper.DisplayProgressDialog(Activity, Resource.String.enabling_bluetooth);
          AndroidOBDBluetoothConnection.Current.EnableBluetooth((aEnabled) =>
          {
            RunOnUiThread(() =>
            {
              lDlg.Dismiss();
              if (!aEnabled)
                DialogHelper.ShowToast(Activity, Resource.String.error_enable_bluetooth, ToastLength.Long);
            });

            aCallbackBluetoothEnabled?.Invoke(aEnabled, false);
          });
        }, (sender, e) => { aCallbackBluetoothEnabled?.Invoke(false, true); });
      });
    }

    private void SelectOtherDevice(Action aCallbackError)
    {
      if (PreferenceHelper.GetValue<bool>(Activity, PreferenceKeys.ASK_NON_STD_ADAPTER, false) == false)
        return;

      RunOnUiThread(() =>
      {
        DialogHelper.ShowToast(Activity, Resource.String.standard_device_not_found, ToastLength.Long);
        List<IListPrompt> lItems = new List<IListPrompt>(AndroidOBDBluetoothConnection.Current.BondedDeviceNames);
        if (lItems.Count > 0)
        {
          DialogHelper.DisplayListPrompt(Activity, lItems, (sender, e) =>
          {
            AndroidOBDBluetoothConnection.Current.Start(lItems[e.Which].Text, aCallbackError);
          });
        }
        else
        {
          DialogHelper.ShowToast(Activity, Resource.String.no_devices_found, ToastLength.Long);
        }
      });
    }

    private void UpdateView()
    {
      TextView lTvSelectedDevice = View.FindViewById<TextView>(Resource.Id.tvDevice);
      TextView lTvLog = View.FindViewById<TextView>(Resource.Id.tvLog);
      TextView lTvObdData = View.FindViewById<TextView>(Resource.Id.tvObdData);

      try
      {
        string lConnectedDevice = AndroidOBDBluetoothConnection.Current.ConnectedDevice?.Name;
        if (string.IsNullOrWhiteSpace(lConnectedDevice))
        {
          lTvSelectedDevice.Text = GetString(Resource.String.no_device_connected);
          lTvSelectedDevice.SetTextColor(ResourceHelper.GetColor(Activity, Resource.Color.colorNegative));
        }
        else
        {
          lTvSelectedDevice.Text = string.Format(GetString(Resource.String.connected_to_device), lConnectedDevice);
          lTvSelectedDevice.SetTextColor(ResourceHelper.GetColor(Activity, Resource.Color.colorPositive));
        }

        lTvLog.Text = string.Join("\n", DebugLog.ToList());
        lTvLog.MovementMethod = new ScrollingMovementMethod();

        string lSpeed = ObdDataProvider.Current.Speed.Valid ? string.Format("{0} km/h", ObdDataProvider.Current.Speed.Value) : "-";
        string lMileage = ObdDataProvider.Current.Mileage.Valid ? string.Format("{0:0.0} km", ObdDataProvider.Current.Mileage.Value) : "-";
        string lFuelRate = ObdDataProvider.Current.FuelRate.Valid ? string.Format("{0:0.0} L/h", ObdDataProvider.Current.FuelRate.Value) : "-";
        string lFuelLevel = ObdDataProvider.Current.FuelLevel.Valid ? string.Format("{0} %", ObdDataProvider.Current.FuelLevel.Value) : "-";
        string lEngineRpm = ObdDataProvider.Current.EngineRpm.Valid ? string.Format("{0:0.0}", ObdDataProvider.Current.EngineRpm.Value) : "-";
        string lAmbientTemp = ObdDataProvider.Current.AmbientTemperature.Valid ? string.Format("{0} °C", ObdDataProvider.Current.AmbientTemperature.Value) : "-";

        lTvObdData.Text = string.Format("Vehicle speed: {0}\nMileage: {1}\nFuel rate: {2}\nFuel level: {3}\nEngine RPM: {4}\nAmbient Temperature: {5}",
          lSpeed, lMileage, lFuelRate, lFuelLevel, lEngineRpm, lAmbientTemp);
      }
      catch (Exception ex)
      {
        DialogHelper.ShowToast(Activity, "Updating UI Error: " + ex.Message, ToastLength.Long);
      }
    }
  }
}