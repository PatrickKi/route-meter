﻿using System;
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

namespace RouteMeter.Classes.Commands.SetupCommands
{
  public class SpacesOffCommand : ObdSetupCommand<string>
  {
    public SpacesOffCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "S0";

    protected override bool GetValue(string aRawData, out string aExtractedData)
    {
      aExtractedData = aRawData;
      return true;
    }

    protected override string ValidateAndExtractData(string aRawData)
    {
      return aRawData;
    }
  }
}