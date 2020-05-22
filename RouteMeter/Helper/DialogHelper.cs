using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Annotation;
using Android.Util;
using Android.Views;
using Android.Widget;
using RouteMeter.Activities;

namespace RouteMeter.Helper
{
  public class DialogHelper
  {
    public static void ShowToast(Context aContext, int aResId, ToastLength aToastLength)
    {
      ShowToast(aContext, aContext.GetText(aResId), aToastLength);
    }

    public static void ShowToast(Context aContext, string aText, ToastLength aToastLength)
    {
      Toast lToast = Toast.MakeText(aContext, aText, aToastLength);
      lToast.View.SetBackgroundResource(Resource.Drawable.default_toast);
      TextView lView = (TextView)lToast.View.FindViewById<TextView>(Android.Resource.Id.Message);
      lView.Gravity = GravityFlags.Center;
      lToast.Show();
    }

    public static void DisplayAlert(Activity aContext, int aStringResId, EventHandler<DialogClickEventArgs> aOkClickHandler)
    {
      DisplayAlert(aContext, aContext.GetString(aStringResId), aOkClickHandler);
    }

    public static void DisplayAlert(Context aContext, string aText, EventHandler<DialogClickEventArgs> aOkClickHandler)
    {
      AlertDialog.Builder lBuilder = new AlertDialog.Builder(aContext);
      lBuilder.SetTitle(Resource.String.information);
      lBuilder.SetMessage(aText);
      lBuilder.SetPositiveButton(Resource.String.ok, aOkClickHandler);
      AlertDialog lDlg = lBuilder.Create();
      lDlg.Window.SetBackgroundDrawableResource(Resource.Drawable.default_dialog);
      lDlg.Show();
    }

    public static void DisplayYesNoPrompt(Context aContext, int aQuestionResId, EventHandler<DialogClickEventArgs> aYesClickHandler, EventHandler<DialogClickEventArgs> aNoClickHandler)
    {
      DisplayYesNoPrompt(aContext, aContext.GetString(aQuestionResId), aYesClickHandler, aNoClickHandler);
    }

    public static void DisplayYesNoPrompt(Context aContext, string aQuestion, EventHandler<DialogClickEventArgs> aYesClickHandler, EventHandler<DialogClickEventArgs> aNoClickHandler)
    {
      DisplayQuestionPrompt(aContext, aQuestion, false, Resource.String.yes, Resource.String.no, aYesClickHandler, aNoClickHandler);
    }

    public static void DisplayOkCancelPrompt(Context aContext, int aQuestionResId, EventHandler<DialogClickEventArgs> aOkClickHandler, EventHandler<DialogClickEventArgs> aCancelClickHandler)
    {
      DisplayOkCancelPrompt(aContext, aContext.GetString(aQuestionResId), aOkClickHandler, aCancelClickHandler);
    }

    public static void DisplayOkCancelPrompt(Context aContext, string aQuestion, EventHandler<DialogClickEventArgs> aOkClickHandler, EventHandler<DialogClickEventArgs> aCancelClickHandler)
    {
      DisplayQuestionPrompt(aContext, aQuestion, false, Resource.String.ok, Resource.String.cancel, aOkClickHandler, aCancelClickHandler);
    }

    public static void DisplayQuestionPrompt(Context aContext, bool aCancelByTouchOutside, int aQuestionResId, int aBtnTextPositiveResId, int aBtnTextNegativeResId, EventHandler<DialogClickEventArgs> aOkClickHandler, EventHandler<DialogClickEventArgs> aCancelClickHandler)
    {
      DisplayQuestionPrompt(aContext, aContext.GetString(aQuestionResId), aCancelByTouchOutside, aBtnTextPositiveResId, aBtnTextNegativeResId, aOkClickHandler, aCancelClickHandler);
    }

    public static void DisplayQuestionPrompt(Context aContext, string aQuestion, bool aCancelByTouchOutside, int aBtnTextPositiveResId, int aBtnTextNegativeResId, EventHandler<DialogClickEventArgs> aOkClickHandler, EventHandler<DialogClickEventArgs> aCancelClickHandler)
    {
      AlertDialog.Builder lBuilder = new AlertDialog.Builder(aContext);
      lBuilder.SetTitle(Resource.String.question);
      lBuilder.SetMessage(aQuestion);
      lBuilder.SetPositiveButton(aBtnTextPositiveResId, aOkClickHandler);
      lBuilder.SetNegativeButton(aBtnTextNegativeResId, aCancelClickHandler);
      lBuilder.SetCancelable(aCancelByTouchOutside);

      AlertDialog lDlg = lBuilder.Create();
      lDlg.Window.SetBackgroundDrawableResource(Resource.Drawable.default_dialog);

      lDlg.Show();
    }

    public static void DisplayListPrompt(Context aContext, List<IListPrompt> aItems, EventHandler<DialogClickEventArgs> aOnSelectHandler)
    {
      AlertDialog.Builder lBuilder = new AlertDialog.Builder(aContext);
      lBuilder.SetTitle(Resource.String.choose);
      lBuilder.SetItems(aItems.Select(x => x.Text).ToArray(), aOnSelectHandler);
      AlertDialog lDlg = lBuilder.Create();
      lDlg.Window.SetBackgroundDrawableResource(Resource.Drawable.default_dialog);
      lDlg.Show();
    }

    public static ProgressDialog DisplayProgressDialog(Context aContext)
    {
      ProgressDialog lDlg = new ProgressDialog(aContext);
      lDlg.Indeterminate = true;
      lDlg.SetProgressStyle(ProgressDialogStyle.Spinner);
      lDlg.SetCancelable(false);
      lDlg.Window.SetBackgroundDrawableResource(Resource.Drawable.default_dialog);
      lDlg.Show();
      return lDlg;
    }

    public static ProgressDialog DisplayProgressDialog(Context aContext, int aMessageResId)
    {
      ProgressDialog lDlg = new ProgressDialog(aContext);
      lDlg.Indeterminate = true;
      lDlg.SetProgressStyle(ProgressDialogStyle.Spinner);
      lDlg.SetMessage(aContext.GetString(aMessageResId));
      lDlg.SetCancelable(false);
      lDlg.Window.SetBackgroundDrawableResource(Resource.Drawable.default_dialog);
      lDlg.Show();
      return lDlg;
    }

    public static Dialog GetCustomDialog(Context aContext, int aTitleResId, int aDialogLayoutResId, SetupViewDelegate aSetupViewDelegate, EventHandler<DialogClickEventArgs> aOnCloseHandler)
    {
      return GetCustomDialog(aContext, aContext.GetString(aTitleResId), aDialogLayoutResId, aSetupViewDelegate, aOnCloseHandler);
    }

    public delegate View SetupViewDelegate(View aView);
    public static Dialog GetCustomDialog(Context aContext, string aTitle, int aDialogLayoutResId, SetupViewDelegate aSetupViewDelegate, EventHandler<DialogClickEventArgs> aOnCloseHandler)
    {
      AlertDialog.Builder lBuilder = new AlertDialog.Builder(aContext);
      lBuilder.SetTitle(aTitle);
      
      View lView = LayoutInflater.FromContext(aContext).Inflate(aDialogLayoutResId, null);
      if (aSetupViewDelegate != null)
        lView = aSetupViewDelegate(lView);
      lBuilder.SetView(lView);

      lBuilder.SetPositiveButton(Resource.String.close, aOnCloseHandler);
      AlertDialog lDlg = lBuilder.Create();
      lDlg.Window.SetBackgroundDrawableResource(Resource.Drawable.default_dialog);
      return lDlg;
    }

    public static void SetDialogSize(Activity aContext, Window aDialogWindow, double aWidthPercent, double aHeightPercent)
    {
      if (aWidthPercent <= 0 || aWidthPercent > 1 || aHeightPercent <= 0 || aHeightPercent > 1)
        throw new Exception("Size parameters must be bigger than 0 and less or equal to 1.", new ArgumentOutOfRangeException());

      DisplayMetrics lDisplay = new DisplayMetrics();
      aContext.WindowManager.DefaultDisplay.GetRealMetrics(lDisplay);
      aDialogWindow.SetLayout((int)(lDisplay.WidthPixels * aWidthPercent), (int)(lDisplay.HeightPixels * aHeightPercent));
    }
  }

  public interface IListPrompt
  {
    string Text { get; set; }
  }
}