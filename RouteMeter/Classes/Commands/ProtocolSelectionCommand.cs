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
  class ProtocolSelectionCommand : ObdCommandBase
  {
    /// <summary>
    /// Protocol selection command.
    /// </summary>
    /// <param name="aSocket">Bluetooth socket.</param>
    /// <param name="aProtocol">Protocol char from ObdProtocols.cs</param>
    public ProtocolSelectionCommand(BluetoothSocket aSocket, char aProtocol) : base(aSocket, aProtocol.ToString()) { }

    protected override string BaseCommand => "AT SP";
  }
}