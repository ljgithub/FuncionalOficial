﻿using CoreLocation;
using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PermisosGPS))]
namespace FarmaEnlace.iOS.Implementations
{
    public class PermisosGPS : IPermisosGPS
    {
        CLLocationManager cLLocationManager;
        

        public bool estaActivo { get; set; }
        public PermisosGPS() {
           
        }
        public bool checkGpsPermission()
        {
            // all iOS devices support at least wifi geolocation
            // requestWhenInUseAuthorization must be set in plist
            cLLocationManager = new CLLocationManager();
            if (CLLocationManager.Status <= CLAuthorizationStatus.Denied)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    cLLocationManager.RequestWhenInUseAuthorization();
                }
                estaActivo = false;
            }
            else
            {
                estaActivo = true;
            }
            return estaActivo;
        }

        public void requestGPSActivation()
        {
            NSString settingsString = UIApplication.LaunchOptionsLocationKey;
            NSUrl url = new NSUrl(settingsString);
            UIApplication.SharedApplication.OpenUrl(url);
        }

    }
}