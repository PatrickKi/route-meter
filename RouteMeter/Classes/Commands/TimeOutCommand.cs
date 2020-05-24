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
using Java.Lang;

namespace RouteMeter.Classes.Commands
{
  public class TimeOutCommand : ObdSetupCommand<string>
  {
    /// <summary>
    /// Timeout command.
    /// </summary>
    /// <param name="aSocket">Bluetooth socket.</param>
    /// <param name="aTimeout">Timeout in milliseconds between 0 and 1000.</param>
    public TimeOutCommand(BluetoothSocket aSocket, int aTimeout) : base(aSocket)
    {
      if (aTimeout > 0 && aTimeout > 1000)
        throw new System.Exception("Timeout needs to be greater than 0 and less or equal to 1000. Reason: Value devided by 4 needs to be between 0 and 255 for HEX conversion.");

      int lTimeout = (int)((double)aTimeout / 4);
      CommandParameter = Integer.ToHexString(0xFF & lTimeout);
    }

    protected override string BaseCommand => "ST";

    protected override bool GetValue(string aRawData, out string aExtractedData)
    {
      aExtractedData = aRawData;
      return true;
    }
  }
}