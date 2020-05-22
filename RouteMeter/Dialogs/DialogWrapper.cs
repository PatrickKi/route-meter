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

namespace RouteMeter.Dialogs
{
  public class DialogWrapper : DialogBase
  {
    Context fContext;
    private int fContentLayoutResId = int.MinValue;
    private View fContentView;

    public View ContentView { get => fContentView; }

    protected Button fBtnOk;
    protected Button fBtnCancel;
    protected LinearLayout fLlTitle;

    public EventHandler OnOkClick { get; set; }
    public EventHandler OnCancelClick { get; set; }

    private bool fCancelButtonVisible = false;
    public bool CancelButtonVisible
    {
      get
      {
        if (fBtnCancel == null)
          return false;

        fCancelButtonVisible = fBtnCancel.Visibility == ViewStates.Visible;
        return fCancelButtonVisible;
      }
      set
      {
        fCancelButtonVisible = value;
        if (fBtnCancel != null)
          fBtnCancel.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
      }
    }

    private bool fTitleVisible = true;
    public bool TitleVisible
    {
      get
      {
        if (fLlTitle == null)
          return false;

        fTitleVisible = fLlTitle.Visibility == ViewStates.Visible;
        return fTitleVisible;
      }
      set
      {
        fTitleVisible = value;
        if (fLlTitle != null)
          fBtnCancel.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
      }
    }

    private string fTitle = null;
    public string Title
    {
      get
      {
        fTitle = fLlTitle?.FindViewById<TextView>(Resource.Id.tvTitle)?.Text;
        return fTitle;
      }
      set
      {
        fTitle = value;

        TextView lTitle = fLlTitle?.FindViewById<TextView>(Resource.Id.tvTitle);

        if (lTitle != null)
          lTitle.Text = fTitle;
      }
    }

    public DialogWrapper(Activity aContext, View aContentLayoutView, int aTitleResId) : this(aContext, aContentLayoutView)
    {
      fTitle = aContext.GetString(aTitleResId);
    }

    public DialogWrapper(Activity aContext, View aContentLayoutView) : base(aContext, Resource.Layout.dlgWrapper)
    {
      fContext = aContext;
      fContentView = aContentLayoutView;
    }

    public DialogWrapper(Activity aContext, View aContentLayoutView, int aTitleResId, double aWidthPercent, double aHeightPercent) : this(aContext, aContentLayoutView, aWidthPercent, aHeightPercent)
    {
      fTitle = aContext.GetString(aTitleResId);
    }

    public DialogWrapper(Activity aContext, View aContentLayoutView, double aWidthPercent, double aHeightPercent) : base(aContext, Resource.Layout.dlgWrapper, aWidthPercent, aHeightPercent)
    {
      fContext = aContext;
      fContentView = aContentLayoutView;
    }

    public DialogWrapper(Activity aContext, int aContentLayoutResId, int aTitleResId) : this(aContext, aContentLayoutResId)
    {
      fTitle = aContext.GetString(aTitleResId);
    }

    public DialogWrapper(Activity aContext, int aContentLayoutResId) : base(aContext, Resource.Layout.dlgWrapper)
    {
      fContext = aContext;
      fContentLayoutResId = aContentLayoutResId;
    }

    public DialogWrapper(Activity aContext, int aContentLayoutResId, int aTitleResId, double aWidthPercent, double aHeightPercent) : this(aContext, aContentLayoutResId, aWidthPercent, aHeightPercent)
    {
      fTitle = aContext.GetString(aTitleResId);
    }

    public DialogWrapper(Activity aContext, int aContentLayoutResId, double aWidthPercent, double aHeightPercent) : base(aContext, Resource.Layout.dlgWrapper, aWidthPercent, aHeightPercent)
    {
      fContext = aContext;
      fContentLayoutResId = aContentLayoutResId;
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      if (fContentView == null)
        fContentView = LayoutInflater.Inflate(fContentLayoutResId, null);

      LinearLayout lLlContent = FindViewById<LinearLayout>(Resource.Id.llContent);
      lLlContent.AddView(fContentView);

      fBtnOk = FindViewById<Button>(Resource.Id.btnOK);
      fBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
      fLlTitle = FindViewById<LinearLayout>(Resource.Id.llTitle);

      if (fBtnOk != null)
      {
        fBtnOk.Click += OnOkClick;
      }

      if (fBtnCancel != null)
      {
        fBtnCancel.Click += OnCancelClick;

        if(fCancelButtonVisible == false)
          fBtnCancel.Visibility = ViewStates.Gone;
      }

      if(fLlTitle != null)
      {
        if (!string.IsNullOrWhiteSpace(fTitle))
          Title = fTitle;
        else
          fTitleVisible = false;

        if(fTitleVisible == false)
          fLlTitle.Visibility = ViewStates.Gone;
      }
    }
  }
}