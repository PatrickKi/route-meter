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
  /// Used to get the engines rpm. Value: 0 - 16.383,75 rpm
  /// </summary>
  public class EngineRpmCommand : ObdDataCommand<double>
  {
    public EngineRpmCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "0C";

    protected override bool GetValue(string aRawData, out double aExtractedData)
    {
      try
      {
        int lRPM = Convert.ToInt32(aRawData, 16);
        aExtractedData = (double)256 * lRPM / 4;
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