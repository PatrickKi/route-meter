using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RouteMeter.Helper
{
  public class PreferenceHelper
  {
    #region Keys
    public class PreferenceKeys
    {
      public const string ASK_NON_STD_ADAPTER = "ASK_NON_STD_ADAPTER";
    }
    #endregion

    public static void SetValue(Context aContext, string aKey, object aValue)
    {
      ISharedPreferences lPrefs = PreferenceManager.GetDefaultSharedPreferences(aContext);
      ISharedPreferencesEditor lEditor = lPrefs.Edit();

      if (aValue is string)
        lEditor.PutString(aKey, aValue as string);
      else if (aValue is int)
        lEditor.PutInt(aKey, Convert.ToInt32(aValue));
      else if (aValue is float)
        lEditor.PutFloat(aKey, Convert.ToSingle(aValue));
      else if (aValue is bool)
        lEditor.PutBoolean(aKey, Convert.ToBoolean(aValue));
      else
        throw new Exception("Value type is not supported. Types: string, int, float, bool.");

      lEditor.Apply();
    }

    public static T GetValue<T>(Context aContext, string aKey, T aDefaultValue)
    {
      ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(aContext);

      if (typeof(T) == typeof(string))
        return (T)Convert.ChangeType(prefs.GetString(aKey, aDefaultValue as string), typeof(T));
      else if (typeof(T) == typeof(int))
        return (T)Convert.ChangeType(prefs.GetInt(aKey, Convert.ToInt32(aDefaultValue)), typeof(T));
      else if (typeof(T) == typeof(float))
        return (T)Convert.ChangeType(prefs.GetFloat(aKey, Convert.ToSingle(aDefaultValue)), typeof(T));
      else if (typeof(T) == typeof(bool))
        return (T)Convert.ChangeType(prefs.GetBoolean(aKey, Convert.ToBoolean(aDefaultValue)), typeof(T));
      else
        throw new Exception("T value type is not supported. Types: string, int, float, bool.");
    }
  }
}