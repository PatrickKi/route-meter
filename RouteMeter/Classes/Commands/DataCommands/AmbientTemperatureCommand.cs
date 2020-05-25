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
  /// Used to get the ambient temperature. Value: -40 - 215 °C
  /// </summary>
  public class AmbientTemperatureCommand : ObdDataCommand<int>
  {
    public AmbientTemperatureCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "46";

    protected override bool GetValue(string aRawData, out int aExtractedData)
    {
      try
      {
        int lTemp = Convert.ToInt32(aRawData, 16);
        aExtractedData = lTemp - 40;
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