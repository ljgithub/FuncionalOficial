using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using FarmaEnlace.Services;
using Foundation;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(GeoLocation))]
namespace FarmaEnlace.iOS.Implementations
{
    class GeoLocation : IGeoLocatorService
    {

        #region Properties
        public static double Latitude { get; set; }

        public static double Longitude { get; set; }

        #endregion

        public async Task<bool> findLocation(bool hasInternetAccess)
        {                          
            //locationProvider = locationManager.GetBestProvider(locationCriteria, true);
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location =  await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));

                if (location == null)
                {
                    location =  await CrossGeolocator.Current.GetLastKnownLocationAsync();
                }
                if (location != null)
                {
                    GeolocatorService.Latitude = location.Latitude;
                    GeolocatorService.Longitude = location.Longitude;
                    return true;
                } 
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
             
        public void requestLocationUpdates(bool hasInternetAccess)
        {
            
        }
    }
}