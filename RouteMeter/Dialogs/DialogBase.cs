using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.Autofill;
using Android.Util;
using Android.Views;
using Android.Widget;
using RouteMeter.Helper;

namespace RouteMeter.Dialogs
{
  public class DialogBase : Dialog
  {
    private Activity fContext;
    private int fLayoutResourceId = int.MinValue;

    private double fWidthPercent;
    private double fHeightPercent;

    public DialogBase(Activity aContext, int aLayoutResId) : base(aContext, aLayoutResId)
    {
      fLayoutResourceId = aLayoutResId;
      Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

      fWidthPercent = 0.7;
      fContext = aContext;
    }

    public DialogBase(Activity aContext, int aLayoutResId, double aWidthPercent, double aHeightPercent) : base(aContext)
    {
      fLayoutResourceId = aLayoutResId;
      Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

      if (aWidthPercent <= 0 || aWidthPercent > 1 || aHeightPercent <= 0 || aHeightPercent > 1)
        throw new Exception("Size parameters must be bigger than 0 and less or equal to 1.", new ArgumentOutOfRangeException());

      fWidthPercent = aWidthPercent;
      fHeightPercent = aHeightPercent;

      fContext = aContext;
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      SetContentView(fLayoutResourceId);

      if(fWidthPercent > 0 && fHeightPercent > 0)
        DialogHelper.SetDialogSize(fContext, Window, fWidthPercent, fHeightPercent);
    }
  }
}