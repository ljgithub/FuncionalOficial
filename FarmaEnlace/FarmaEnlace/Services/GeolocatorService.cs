using System;
using System.Threading.Tasks;
using Android.Locations;
using CoreLocation;
using FarmaEnlace.Interfaces;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace FarmaEnlace.Services
{
    public class GeolocatorService
    {

        #region Properties
        public static double Latitude
        {
            get;
            set;
        }

        public static double Longitude
        {
            get;
            set;
        }
        #endregion

        public async Task<bool> GetLocation()
        {

            //locationProvider = locationManager.GetBestProvider(locationCriteria, true);
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
                if (location == null)
                {
                    location = await CrossGeolocator.Current.GetLastKnownLocationAsync();
                }
                Latitude = location.Latitude;
                Longitude = location.Longitude;
                Console.WriteLine("Long: " + Longitude + " Lat :" + Latitude);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public static async Task<bool> checkLocationAvaibility()
        {
            //verifica si la aplicacion esta lista para usar el GPS, para ello debe verificar si la opcion en el telefono esta activa y tambien verificar
            //si tiene el permiso necesario. Solo si ambas condiciones estan correctas retorno true, sino false.
            DialogService dialogService = new DialogService();
            bool isGPSActive = false;
            bool hasGPSPermissions = false;      
            IPermisosGPS permisoGPS = DependencyService.Get<IPermisosGPS>();

            Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
            if (locator.IsGeolocationEnabled == false)
            {
                bool respuesta = await dialogService.ShowConfirm("", "Para continuar, permite que tu dispositivo active la ubicación, que se usa en el servicio de ubicación.");
                if (respuesta)
                   permisoGPS.requestGPSActivation();//cambiart nombre                                                 
                isGPSActive = false;
            }
            else
            {
                isGPSActive = true;
            }

            //hasGPSPermissions revisar permisos para GPS
            hasGPSPermissions = permisoGPS.checkGpsPermission();
            return (isGPSActive && hasGPSPermissions);
        }

    }
}
