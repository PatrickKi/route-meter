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
  /// Used to get the current fuel level. Value: 0 - 100 %
  /// </summary>
  public class FuelLevelCommand : ObdDataCommand<int>
  {
    public FuelLevelCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "2F";

    protected override bool GetValue(string aRawData, out int aExtractedData)
    {
      try
      {
        int lFuel = Convert.ToInt32(aRawData, 16);
        aExtractedData = (int)((double)100 / 255 * lFuel);
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