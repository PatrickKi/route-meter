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

namespace RouteMeter.Classes.Commands
{
  public class LineFeedCommand : ObdCommandBase
  {
    public LineFeedCommand(BluetoothSocket aSocket) : base(aSocket) { }

    protected override string BaseCommand => "AT L0";
  }
}