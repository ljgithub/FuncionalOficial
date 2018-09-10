using CoreLocation;
using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PermisosGPS))]
namespace FarmaEnlace.iOS.Implementations
{
    public class PermisosGPS : IPermisosGPS
    {
        public bool estaActivo { get; set; }
        public PermisosGPS() { }
        public void verificarPermisosGPS()
        {
            // all iOS devices support at least wifi geolocation
            // requestWhenInUseAuthorization must be set in plist

            if (CLLocationManager.Status == CLAuthorizationStatus.Denied)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    NSString settingsString = UIApplication.OpenSettingsUrlString;
                    NSUrl url = new NSUrl(settingsString);
                    UIApplication.SharedApplication.OpenUrl(url);
                }
                estaActivo = false;
            }
            else
            {
                estaActivo = true;
            }
        }

    }
}