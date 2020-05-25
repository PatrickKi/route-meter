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
  /// Used to get the current mileage. Value: 0 - 526.385.151,9 hm (km/10) but value output is in km.
  /// </summary>
  /// <param name="aSocket"></param>
  public class OdometerCommand : ObdDataCommand<double>
  {
    public OdometerCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "A6";

    protected override bool GetValue(string aRawData, out double aExtractedData)
    {
      try
      {
        int lOdometer = Convert.ToInt32(aRawData, 16);
        aExtractedData = (double)lOdometer / 10;
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