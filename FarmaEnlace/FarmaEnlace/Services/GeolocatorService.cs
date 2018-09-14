using System;
using System.Threading.Tasks;
using Android.Locations;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace FarmaEnlace.Services
{
    public class GeolocatorService
    {
        

        //LocationRequest locationRequest = new LocationRequest()
                               //   .SetPriority(LocationRequest.PriorityHighAccuracy)
                              //    .SetInterval(60 * 1000 * 5)
                             //     .SetFastestInterval(60 * 1000 * 2);


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

        #region Methods
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
                     location=   await CrossGeolocator.Current.GetLastKnownLocationAsync();
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
        #endregion
    }
}
