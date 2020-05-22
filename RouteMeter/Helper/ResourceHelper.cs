using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace RouteMeter.Helper
{
  public class ResourceHelper
  {
    public static Android.Graphics.Color GetColor(Context aContext, int ColorResId)
    {
      return new Android.Graphics.Color(ContextCompat.GetColor(aContext, ColorResId));
    }
  }
}