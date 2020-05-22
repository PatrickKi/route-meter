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
using static RouteMeter.Classes.AndroidOBDBluetoothConnection;

namespace RouteMeter.Classes.Commands
{
  public abstract class ObdCommandBase
  {
    protected BluetoothSocket Socket { get; set; }

    protected abstract string BaseCommand { get; }
    protected string CommandParameter { get; set; }

    public ObdCommandBase(BluetoothSocket aSocket, string aCommandParameter = null)
    {
      Socket = aSocket;
      CommandParameter = aCommandParameter;
    }

    /// <summary>
    /// Send the command to the bluetooth socket. Returns true when command was sent and a response was received.
    /// </summary>
    /// <param name="aCallbackDataReceived">Callback for the received data.</param>
    /// <returns>True when command was sent and a response was received.</returns>
    public bool Send(DataReceivedDelegate aCallbackDataReceived)
    {
      if (Socket.IsConnected)
      {
        byte[] cmd = Encoding.ASCII.GetBytes(BuildCommand(BaseCommand, CommandParameter));
        Socket.OutputStream.Write(cmd, 0, cmd.Length);
        Socket.OutputStream.Flush();

        string lResponse = ReadResponse();
        if (!string.IsNullOrWhiteSpace(lResponse))
        {
          aCallbackDataReceived?.Invoke(lResponse);
          return true;
        }
      }

      return false;
    }

    protected virtual string BuildCommand(string aBaseCommand, string aCommandParameter)
    {
      string lCommand = string.Empty;
      if (string.IsNullOrEmpty(aCommandParameter))
        lCommand = string.Format("{0}", aBaseCommand);
      else
        lCommand = string.Format("{0} {1}", aBaseCommand, aCommandParameter);

      return string.Format("{0} \r", lCommand);
    }

    public string ReadResponse()
    {
      StringBuilder lResult = new StringBuilder();

      try
      {
        int b;

        // read until '>' arrives OR end of stream reached
        char c;
        // -1 if the end of the stream is reached
        while ((b = Socket.InputStream.ReadByte()) > -1)
        {
          c = (char)b;
          //if (c == '>') // read until '>' arrives
          //{
          //  break;
          //}
          lResult.Append(c);
        }

      }
      catch (Exception e)
      {
        System.Console.WriteLine(e.ToString());
      }

      // Data may have echo or informative text like "INIT BUS..." or similar.
      // The response ends with two carriage return characters. So we need to take
      // everything from the last carriage return before those two (trimmed above).

      //lResult.Replace("SEARCHING", "");
      //lResult.Replace("\\s", "");

      return lResult.ToString();
    }
  }
}