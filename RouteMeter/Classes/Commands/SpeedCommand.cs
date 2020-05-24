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

namespace RouteMeter.Classes.Commands
{
  public class SpeedCommand : ObdDataCommand<int>
  {
    public SpeedCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "0D";

    protected override int GetValue(string aRawData)
    {
      try
      {
        return Convert.ToInt32(aRawData, 16);
      }
      catch
      {
        return int.MinValue;
      }
    }
  }
}