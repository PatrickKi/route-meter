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
using Java.Lang;
using RouteMeter.Helper;
using static RouteMeter.Helper.PreferenceHelper;

namespace RouteMeter.Dialogs
{
  public class DlgPreferences
  {
    protected Dialog Dialog { get; set; }
    protected Context ExtContext { get; set; }

    public DlgPreferences(Context aContext)
    {      
      ExtContext = aContext;
      Dialog = DialogHelper.GetCustomDialog(aContext, aContext.GetString(Resource.String.settings), Resource.Layout.dlgPreferences, SetupView, null);
      Dialog.SetCancelable(true);
      Dialog.SetCanceledOnTouchOutside(false);
    }

    private View SetupView(View aView)
    {
      Switch lSwAskNonStdAdapter = aView.FindViewById<Switch>(Resource.Id.swAskNonStandardAdapter);
      lSwAskNonStdAdapter.Checked = PreferenceHelper.GetValue<bool>(ExtContext, PreferenceKeys.ASK_NON_STD_ADAPTER, false);
      lSwAskNonStdAdapter.CheckedChange += (sender, e) =>
      {
        PreferenceHelper.SetValue(ExtContext, PreferenceKeys.ASK_NON_STD_ADAPTER, e.IsChecked);
      };

      return aView;
    }

    public void Show()
    {
      Dialog?.Show();
    }
  }
}