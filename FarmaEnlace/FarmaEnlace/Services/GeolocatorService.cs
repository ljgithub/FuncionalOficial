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
        public static double Latitude;
        public static double Longitude;

        public static int DENIED=2;
        public static int ALLOWED=0;
        public static int UNDEFINED=1;

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

        public static async Task<int> checkLocationAvaibility()
        {
            //verifica si la aplicacion esta lista para usar el GPS, para ello debe verificar si la opcion en el telefono esta activa y tambien verificar
            //si tiene el permiso necesario. Solo si ambas condiciones estan correctas retorno 0, sino retorno 2 cuando esta denegado o 1 si puede autorizarse con el usuario.
            DialogService dialogService = new DialogService();
            bool isGPSActive = false;
            int permission = 0;
            IPermisosGPS permisoGPS = DependencyService.Get<IPermisosGPS>();

            Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
            bool respuestaServicios = true;
            if (locator.IsGeolocationEnabled == false)
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    respuestaServicios = await dialogService.ShowConfirm("Notificación", "Para continuar, permite que tu dispositivo active la ubicación, que se usa en el servicio de ubicación.");                    
                }

                if (respuestaServicios)
                {
                    permisoGPS.requestGPSActivation();//cambiart nombre  
                }

                isGPSActive = false;
            }
            else
            {
                return ALLOWED;
            }

            if (respuestaServicios)
            {
                 permission=permisoGPS.checkGpsPermission();
            }

            //si el gps esta desactivado pero no ha sido denegado por el usuario
            if (!isGPSActive && permission < 2) return UNDEFINED;

            //si el gps si estaba activo, la respuesta depende unicamente de los permisos
            return permission;
        }

    }
}
