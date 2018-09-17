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
using System.Threading.Tasks;

[assembly: Dependency(typeof(GeoLocation))]
namespace FarmaEnlace.Android.Implementations
{
    class GeoLocation : Java.Lang.Object, IGeoLocatorService, ILocationListener
    {
        #region Attributes
        public LocationManager locationManager;
        private Context context;
        private static long MIN_DISTANCE_CHANGE_FOR_UPDATES = 10; // The minimum time between updates in milliseconds
        private static long MIN_TIME_BW_UPDATES = 1000; // 1 second
        
        #endregion

        #region Constructors
        
        public GeoLocation()
        {
            this.context = Forms.Context;
        }

        #endregion

        #region methods

        public String getBestProviderName(bool hasInternetAccess)
        {
            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            Criteria criteria = new Criteria();
            criteria.PowerRequirement = Power.Low;
            criteria.Accuracy = Accuracy.Medium;
            criteria.SpeedRequired = true;
            criteria.AltitudeRequired = false;
            criteria.BearingRequired = false;
            criteria.CostAllowed = false;
            return locationManager.GetBestProvider(criteria, true);
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {
            GeolocatorService.Latitude = location.Latitude;
            GeolocatorService.Longitude = location.Longitude;

            Console.WriteLine("Meotodo OnLocationChanged: " +
            GeolocatorService.Latitude+" - " + GeolocatorService.Longitude);
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

        bool IGeoLocatorService.findLocation(bool hasInternetAccess)
        {
            String provider = getBestProviderName(hasInternetAccess);

            //construyo los servicios de ubicacion si estuvieran abajo
            if (locationManager == null)
            {
                locationManager = (LocationManager)context.GetSystemService(Context.LocationService);
                locationManager.RequestLocationUpdates(
                         provider,
                         MIN_TIME_BW_UPDATES,
                         MIN_DISTANCE_CHANGE_FOR_UPDATES, this);
            }

            if (GeolocatorService.Latitude==0 || GeolocatorService.Longitude==0) {
                Location lastKnown = locationManager.GetLastKnownLocation(provider);

                if (lastKnown == null)
                {
                    GeolocatorService.Latitude = lastKnown.Latitude;
                    GeolocatorService.Longitude = lastKnown.Longitude;
                    Console.WriteLine("long: " + lastKnown.Longitude + " lat: " + lastKnown.Latitude);
                    return true;
                } else
                {
                    return false;
                }
            } else {
                return true;
            }
            throw new NotImplementedException();
        }

        public void requestLocationUpdates(bool hasInternetAccess)
        {
            String provider = getBestProviderName(hasInternetAccess);

            //construyo los servicios de ubicacion si estuvieran abajo
            if (locationManager == null)
            {

                locationManager = (LocationManager)context.GetSystemService(Context.LocationService);
                locationManager.RequestLocationUpdates(
                         provider,
                         MIN_TIME_BW_UPDATES,
                         MIN_DISTANCE_CHANGE_FOR_UPDATES, this);
            }
            else {
                //locationManager.RemoveUpdates(this);
            }

            locationManager.RequestLocationUpdates(provider, MIN_TIME_BW_UPDATES, MIN_DISTANCE_CHANGE_FOR_UPDATES, this);
            

        }

        public Task<bool> findLocationAsync(bool hasInternetAccess)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}