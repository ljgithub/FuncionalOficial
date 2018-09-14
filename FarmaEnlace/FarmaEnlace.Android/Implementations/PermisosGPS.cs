using Android.Locations;
using Xamarin.Forms;

using FarmaEnlace.Interfaces;
using FarmaEnlace.Android.Implementations;
using Android.Content;
using Android.Provider;
using Android.Net.Wifi;

[assembly: Xamarin.Forms.Dependency(typeof(PermisosGPS))]
namespace FarmaEnlace.Android.Implementations
{
    public class PermisosGPS : IPermisosGPS
    {
        public bool estaActivoGPS { get; set; }
        public bool estaActivoRED { get; set; }

        public PermisosGPS() { }
        public void activatePermissions()
        {
            LocationManager locationManagerGPS = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
          
            
            bool estaActivoGPS = locationManagerGPS.IsProviderEnabled(LocationManager.GpsProvider);
                  

            if (!estaActivoGPS)
            {
                Intent gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
                Forms.Context.StartActivity(gpsSettingIntent);
            }


        }
    }
}