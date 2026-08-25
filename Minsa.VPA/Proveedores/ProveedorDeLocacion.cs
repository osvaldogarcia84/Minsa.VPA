using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeLocacion //: Java.Lang.Object, ILocationListener
    {
        //private readonly LocationManager _locationManager;
        //private readonly string _locationProvider;

        //public Location Locacion { get; private set; }
        //public event EventHandler<Location> LocacionEncontrada;

        //protected virtual void OnLocacionEncontrada(Location e)
        //{
        //    EventHandler<Location> handler = LocacionEncontrada;
        //    if (handler != null)
        //        handler(this, e);
        //}

        //public ProveedorDeLocacion(Context contexto, long intervalo = 0)
        //{
        //    _locationManager = (LocationManager)contexto.GetSystemService(Context.LocationService);
        //    var criteriaForLocationService = new Criteria
        //    {
        //        Accuracy = Accuracy.Fine            

        //    };
        //    IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);
        //    _locationProvider = acceptableLocationProviders.Any() ? acceptableLocationProviders.First() : String.Empty;
        //    if (_locationProvider.EsValido())
        //        Obtener(intervalo);
        //}

        //public ProveedorDeLocacion(Context contexto, string provider)
        //{
        //    //provider = LocationManager.GpsProvider;
        //    _locationManager = (LocationManager)contexto.GetSystemService(Context.LocationService);
        //    var criteriaForLocationService = new Criteria
        //    {
        //        Accuracy = Accuracy.Fine
        //    };
        //    IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);
        //    _locationProvider = acceptableLocationProviders.Any(e => e == provider) ? provider : acceptableLocationProviders.First();
        //    if (_locationProvider.EsValido())
        //        Obtener(0);
        //}

        //public void Obtener(long intervalo = 0)
        //{

        //    try
        //    {
        //        _locationManager.RequestLocationUpdates(_locationProvider, intervalo, 0, this);

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("OnResume", ex.StackTrace);
        //    }

        //}

        //public void Remover()
        //{
        //    _locationManager.RemoveUpdates(this);
        //}
    


        //public void OnLocationChanged(Location location)
        //{
        //    Locacion = location;
        //    OnLocacionEncontrada(location);
        //}

        //public void OnProviderDisabled(string provider)
        //{
        //}

        //public void OnProviderEnabled(string provider)
        //{
        //}

        //public void OnStatusChanged(string provider, Availability status, Bundle extras)
        //{
        //}
    }
}