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

    /// <summary>
    /// Event is called whenever a value of the OBD data is updated.
    /// </summary>
    public static event Action OnDataUpdated;

    /// <summary>
    /// Current vehicle speed in km/h.
    /// </summary>
    public ObdData<int> Speed { get; } = new ObdData<int>(OnDataUpdated);

    /// <summary>
    /// Current mileage in km.
    /// </summary>
    public ObdData<double> Mileage { get; } = new ObdData<double>(OnDataUpdated);

    /// <summary>
    /// Current fuel rate in L/h.
    /// </summary>
    public ObdData<double> FuelRate { get; } = new ObdData<double>(OnDataUpdated);

    /// <summary>
    /// Current fuel tank level in percent.
    /// </summary>
    public ObdData<int> FuelLevel { get; } = new ObdData<int>(OnDataUpdated);

    /// <summary>
    /// Current engine speed in rounds per minute (rpm).
    /// </summary>
    public ObdData<double> EngineRpm { get; } = new ObdData<double>(OnDataUpdated);

    /// <summary>
    /// Current ambient air temperature in °C.
    /// </summary>
    public ObdData<int> AmbientTemperature { get; } = new ObdData<int>(OnDataUpdated);
  }

  public class ObdData<T>
  {
    /// <summary>
    /// Timespan in seconds within the value counts as valid.
    /// </summary>
    public const int VALID_TIMESPAN = 5;

    public ObdData(Action aCallbackUpdated)
    {
      fCallbackUpdated = aCallbackUpdated;
    }

    protected Action fCallbackUpdated;
    public DateTime LastUpdated { get; private set; } = DateTime.MinValue;
    public bool Valid => LastUpdated.AddSeconds(VALID_TIMESPAN) >= DateTime.Now;
    public T Value { get; private set; } = default(T);

    public void Update(T aValue)
    {
      Value = aValue;
      LastUpdated = DateTime.Now;
      fCallbackUpdated?.Invoke();
    }
  }
}