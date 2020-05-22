using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

namespace RouteMeter.Fragments
{
  public class FragmentBase : Fragment
  {
    public override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      HasOptionsMenu = true; ;
    }

    public void RunOnUiThread(Action aAction)
    {
      Activity?.RunOnUiThread(aAction);
    }
  }
}