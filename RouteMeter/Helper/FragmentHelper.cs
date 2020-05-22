using Android.Support.V4.App;

namespace RouteMeter.Helper
{
  public class FragmentHelper
  {
    public static bool ReplaceFragment(FragmentManager aFragmentManager, int aFragmentHostResId, Fragment aNewFragment)
    {
      try
      {
        if (aNewFragment != null)
        {
          FragmentTransaction lTransaction = aFragmentManager.BeginTransaction();
          lTransaction.Replace(aFragmentHostResId, aNewFragment);
          lTransaction.Commit();
          return true;
        }
        else
          return false;
      }
      catch
      {
        return false;
      }
    }
  }
}