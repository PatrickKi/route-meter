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
  public abstract class ObdDataCommand<T> : ObdCommandBase<T>
  {
    public ObdDataCommand(BluetoothSocket aSocket, string aCommandParameter = null) : base(aSocket, aCommandParameter) { }

    protected override string CommandPrefix => PREFIX_SEND_DATA_CMD;
  }
}