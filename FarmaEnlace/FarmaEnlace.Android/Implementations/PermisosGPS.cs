using Android.Locations;
using Xamarin.Forms;

using FarmaEnlace.Interfaces;
using FarmaEnlace.Android.Implementations;
using Android.Content;
using Android.Provider;

[assembly: Xamarin.Forms.Dependency(typeof(PermisosGPS))]
namespace FarmaEnlace.Android.Implementations
{
    public class PermisosGPS : IPermisosGPS
    {
        public bool estaActivo { get; set; }
        public PermisosGPS() { }
        public void verificarPermisosGPS()
        {
            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            bool estaActivo = locationManager.IsProviderEnabled(LocationManager.GpsProvider);

            if (estaActivo == false)
            {
                Intent gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
                Forms.Context.StartActivity(gpsSettingIntent);
            }

        }
    }
}