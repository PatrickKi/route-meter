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
using Fragment = Android.Support.V4.App.Fragment;

namespace RouteMeter.Fragments
{
  class FragmentAnalysis : FragmentBase
  {
    public override View OnCreateView(LayoutInflater aInflater, ViewGroup aContainer, Bundle aSavedInstanceState)
    {
      return aInflater.Inflate(Resource.Layout.fragmentAnalysis, aContainer, false);
    }

    public override void OnViewCreated(View aView, Bundle aSavedInstanceState)
    {
      base.OnViewCreated(aView, aSavedInstanceState);
      Activity.SetTitle(Resource.String.nav_analysis);
    }

    public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
    {
      inflater.Inflate(Resource.Menu.menu_analysis, menu);
      base.OnCreateOptionsMenu(menu, inflater);
    }

    public override bool OnOptionsItemSelected(IMenuItem aItem)
    {
      switch (aItem.ItemId)
      {
        case Resource.Id.menu_dummy:
          break;
      }

      return base.OnOptionsItemSelected(aItem);
    }
  }
}