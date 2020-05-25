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
  /// Used to get the current vehicle speed. Value: 0 - 255 km/h
  /// </summary>
  public class SpeedCommand : ObdDataCommand<int>
  {
    public SpeedCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "0D";

    protected override bool GetValue(string aRawData, out int aExtractedData)
    {
      try
      {
        aExtractedData = Convert.ToInt32(aRawData, 16);
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