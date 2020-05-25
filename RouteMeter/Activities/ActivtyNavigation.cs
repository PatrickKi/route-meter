using System;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using RouteMeter.Classes;
using RouteMeter.Fragments;
using RouteMeter.Helper;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace RouteMeter.Activities
{
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
  public class ActivityNavigation : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
  {
    private DrawerLayout Drawer { get; set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.activityNavigation);
      Android.Support.V7.Widget.Toolbar lToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
      SetSupportActionBar(lToolbar);

      Drawer = FindViewById<DrawerLayout>(Resource.Id.dlRoot);
      ActionBarDrawerToggle lToggle = new ActionBarDrawerToggle(this, Drawer, lToolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
      Drawer.AddDrawerListener(lToggle);
      lToggle.SyncState();

      NavigationView lNavView = FindViewById<NavigationView>(Resource.Id.nvDrawer);
      lNavView.SetNavigationItemSelectedListener(this);

      IMenuItem lFirstItem = lNavView.Menu.GetItem(0);
      if (lFirstItem != null)
      {
        OnNavigationItemSelected(lFirstItem);
        lFirstItem.SetChecked(true);
      }
    }

    public override void OnBackPressed()
    {
      if (Drawer?.IsDrawerOpen(GravityCompat.Start) ?? false)
        Drawer?.CloseDrawer(GravityCompat.Start);
      else
        base.OnBackPressed();
    }

    public bool OnNavigationItemSelected(IMenuItem item)
    {
      //Dont open the same drawer again
      if (!item.IsChecked)
      {
        Fragment lFragment = null;

        switch (item.ItemId)
        {
          case Resource.Id.nav_start:
            lFragment = new FragmentMain();
            break;
          case Resource.Id.nav_analysis:
            lFragment = new FragmentAnalysis();
            break;
          case Resource.Id.nav_send_as_mail:
            DialogHelper.ShowToast(this, "Noch nicht implementiert.", Android.Widget.ToastLength.Short);
            break;
        }

        FragmentHelper.ReplaceFragment(SupportFragmentManager, Resource.Id.fragmentContent, lFragment);
      }

      Drawer?.CloseDrawer(GravityCompat.Start);
      return true;
    }
  }
}

