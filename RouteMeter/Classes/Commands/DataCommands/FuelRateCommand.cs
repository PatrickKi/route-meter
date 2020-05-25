using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RouteMeter.Classes.Commands.DataCommands
{
  /// <summary>
  /// Used to get the current fuel rate. Value: 0 - 3.212.75 L/h
  /// </summary>
  public class FuelRateCommand : ObdDataCommand<double>
  {
    public FuelRateCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "5E";

    protected override bool GetValue(string aRawData, out double aExtractedData)
    {
      try
      {
        int lFuelRate = Convert.ToInt32(aRawData, 16);
        aExtractedData = (double)256 * lFuelRate / 20;
        return true;
      }
      catch
      {
        aExtractedData = int.MinValue;
        return false;
      }
    }
  }
}