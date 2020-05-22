using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RouteMeter.Classes.Commands
{
  public class ObdProtocols
  {
    /// <summary>
    /// Auto select protocol.
    /// </summary>
    public const char AUTO = '0';

    /// <summary>
    /// Baudrate: 41.6 kbaud
    /// </summary>
    public const char SAE_J1850_PWM = '1';

    /// <summary>
    /// Baudrate: 10.4 kbaud
    /// </summary>
    public const char SAE_J1850_VPW = '2';
    /// <summary>
    /// Baudrate: 5 baud init
    /// </summary>
    public const char ISO_9141_2 = '3';

    /// <summary>
    /// Baudrate: 5 baud init
    /// </summary>
    public const char ISO_14230_4_KWP = '4';

    /// <summary>
    /// Baudrate: Fast init
    /// </summary>
    public const char ISO_14230_4_KWP_FAST = '5';

    /// <summary>
    /// Baudrate: 11 bit ID, 500 kbaud
    /// </summary>
    public const char ISO_15765_4_CAN = '6';

    /// <summary>
    /// Baudrate: 29 bit ID, 500 kbaud
    /// </summary>
    public const char ISO_15765_4_CAN_B = '7';

    /// <summary>
    /// Baudrate: 11 bit ID, 250 kbaud
    /// </summary>
    public const char ISO_15765_4_CAN_C = '8';

    /// <summary>
    /// Baudrate: 29 bit ID, 250 kbaud
    /// </summary>
    public const char ISO_15765_4_CAN_D = '9';

    /// <summary>
    /// Baudrate: 29 bit ID, 250 kbaud (user adjustable)
    /// </summary>
    public const char SAE_J1939_CAN = 'A';

    /// <summary>
    /// Baudrate: 11 bit ID (user adjustable), 125 kbaud (user adjustable)
    /// </summary>
    public const char USER1_CAN = 'B';

    /// <summary>
    /// Baudrate: 11 bit ID (user adjustable), 50 kbaud (user adjustable)
    /// </summary>
    public const char USER2_CAN = 'C';
  }
}