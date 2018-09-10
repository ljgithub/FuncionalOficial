using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace FarmaEnlace.Services
{
    public class GeolocatorService
    {
        #region Properties
        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public async Task<bool> GetLocation()
        {
            try {
               
                var locator = CrossGeolocator.Current;
                
                    locator.DesiredAccuracy = 50;
                    var location = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
                    if (location == null)
            {
                     location=   await CrossGeolocator.Current.GetLastKnownLocationAsync();
                    }
                Latitude = location.Latitude;
                Longitude = location.Longitude;
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
