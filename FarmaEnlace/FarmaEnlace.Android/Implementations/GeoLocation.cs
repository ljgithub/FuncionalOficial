using System;
using Android.App;
using Android.Content;
using Android.OS;

using FarmaEnlace.Interfaces;
using FarmaEnlace.Android.Implementations;
using Xamarin.Forms;
using Android.Locations;
using Android.Support.V4.App;
using Android;
using Android.Util;
using Android.Content.PM;
using Android.Runtime;
using FarmaEnlace.Services;

[assembly: Dependency(typeof(GeoLocation))]
namespace FarmaEnlace.Android.Implementations
{
    class GeoLocation : Java.Lang.Object, IGeoLocatorService, ILocationListener
    {
        public static LocationManager locationManager;

        private Context context;

        private static long MIN_DISTANCE_CHANGE_FOR_UPDATES = 10; 
        // The minimum time between updates in milliseconds
        private static long MIN_TIME_BW_UPDATES = 1000 * 20 * 1; // 1 minute
        

        public GeoLocation()
        {
                
        }

        public GeoLocation(Context context)
        {
            this.context = Forms.Context;
        }

        //public IntPtr Handle => throw new NotImplementedException();


        public void findLocation(bool hasInternetAccess)
        {
            //debe unicamente encontrar una ubicacion, ya sea con GPS satelital o GPS de internet
            //para lo cual aqui necesito saber si tengo conexion a internet o no, porque si tengo conexion ainternet voy a pedir GPS de red sino el satelital
            try
            {
                String provider = null;

                this.context = Forms.Context;
                if (locationManager == null)
                {
                    locationManager = (LocationManager)context.GetSystemService(Context.LocationService);
                }

               
                if (hasInternetAccess) {
                    provider = LocationManager.NetworkProvider;
                } else
                {
                    provider = LocationManager.GpsProvider;
                }

                Location lastKnown = locationManager.GetLastKnownLocation(provider);
                if (lastKnown==null) {
                    GeolocatorService.Latitude = lastKnown.Latitude;
                    GeolocatorService.Longitude = lastKnown.Longitude;
                }

                locationManager.RequestLocationUpdates(
                               provider,
                               MIN_TIME_BW_UPDATES,
                               MIN_DISTANCE_CHANGE_FOR_UPDATES, this);

            }
            catch (Exception e)
            {
                Console.WriteLine("" + e.GetBaseException());
                //mensaje de eerror diciendo que no pudo activar el gps
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {
            GeolocatorService.Latitude = location.Latitude;
            GeolocatorService.Longitude = location.Longitude;

            Console.WriteLine("Meotodo OnLocationChanged");
        }

        public void OnProviderDisabled(string provider)
        {
            Console.WriteLine("Meotodo OnProviderDisabled");
        }

        public void OnProviderEnabled(string provider)
        {
            Console.WriteLine("Meotodo OnProviderEnabled");
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) 
        {
            Console.WriteLine("Meotodo OnStatusChanged");
        }
    }
}