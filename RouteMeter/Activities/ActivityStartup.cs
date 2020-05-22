using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using RouteMeter.Helper;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace RouteMeter.Activities
{
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, NoHistory = true)]
  public class ActivityStartup : AppCompatActivity, View.IOnClickListener
  {
    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      Platform.Init(this, savedInstanceState);
      SetContentView(Resource.Layout.activityStartup);
      Toolbar lToolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

      Button lBtn = FindViewById<Button>(Resource.Id.btnGivePermission);
      lBtn.SetOnClickListener(this);

      CheckPermissionsAsync((aGranted) =>
      {
        if (aGranted)
          StartActivity(typeof(ActivityNavigation)); //StartActivity(typeof(ActivityMain));
        else
        {
          lToolbar.Visibility = ViewStates.Visible;
          SetActionBar(lToolbar);
          ActionBar.Title = GetString(Resource.String.app_name);
          lBtn.Visibility = ViewStates.Visible;
        }
      });
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
      Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    private async Task<bool> CheckPermissionsAsync(Action<bool> aCallback)
    {
      bool lPermissionsGranted = true;

      lPermissionsGranted = (await CheckAndRequestPermission<Permissions.LocationWhenInUse>()) == PermissionStatus.Granted && lPermissionsGranted;

      aCallback?.Invoke(lPermissionsGranted);
      return lPermissionsGranted;
    }

    private async Task<PermissionStatus> CheckAndRequestPermission<T>() where T : BasePermission, new()
    {
      PermissionStatus lStatus = PermissionStatus.Unknown;

      try
      {
        lStatus = await Permissions.CheckStatusAsync<T>();
        if (lStatus != PermissionStatus.Granted)
          lStatus = await Permissions.RequestAsync<T>();
      }
      catch (Exception ex)
      {
        DialogHelper.ShowToast(this, ex.Message, ToastLength.Long);
      }

      return lStatus;
    }

    public void OnClick(View v)
    {
      switch (v.Id)
      {
        case Resource.Id.btnGivePermission:
          CheckPermissionsAsync((aGranted) =>
          {
            if (aGranted)
              StartActivity(typeof(ActivityNavigation)); //StartActivity(typeof(ActivityMain));
          });
          break;
      }
    }
  }
}

