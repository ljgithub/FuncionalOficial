using Android.Locations;
using Xamarin.Forms;

using FarmaEnlace.Interfaces;
using FarmaEnlace.Android.Implementations;
using Android.Content;
using Android.Provider;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using System;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(PermisosGPS))]
namespace FarmaEnlace.Android.Implementations
{
    public class PermisosGPS : IPermisosGPS
    {


        #region Attributes
        private static int MY_PERMISSION_ACCESS_COARSE_LOCATION = 11;
        Activity activity;

        public bool estaActivoGPS { get; set; }
        public bool estaActivoRED { get; set; }
        #endregion

        #region Constructor
        public PermisosGPS() {
            
        }
        #endregion

        #region Methods
        public void requestGPSActivation()
        {
            Intent gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
            Forms.Context.StartActivity(gpsSettingIntent);
        }

        public bool checkGpsPermission() {
            //verifico si la app tiene el permiso para acceder al GPS, retorno true si es que si tiene o false de lo contrario

            if (Forms.Context.CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
            {
                return true;
            } else {
                activity = new Activity();
                ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.AccessCoarseLocation }, MY_PERMISSION_ACCESS_COARSE_LOCATION);
                return false;
            }
        }
        #endregion
    }
}