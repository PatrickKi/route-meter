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

namespace RouteMeter.Classes
{
  public class ObdDataProvider
  {
    #region Static Properties
    protected static ObdDataProvider fCurrent;
    public static ObdDataProvider Current
    {
      get
      {
        if (fCurrent == null)
          fCurrent = new ObdDataProvider();

        return fCurrent;
      }
      set
      {
        fCurrent = value;
      }
    }
    #endregion

    public ObdData<int> Speed { get; set; }
  }

  public class ObdData<T>
  {
    /// <summary>
    /// Timespan in seconds within the value counts as valid.
    /// </summary>
    public const int VALID_TIMESPAN = 5;

    public DateTime LastUpdated { get; private set; } = DateTime.MinValue;
    public bool Valid => LastUpdated.AddSeconds(VALID_TIMESPAN) >= DateTime.Now;
    public T Value { get; private set; } = default(T);

    public void Update(T aValue)
    {
      Value = aValue;
      LastUpdated = DateTime.Now;
    }
  }
}