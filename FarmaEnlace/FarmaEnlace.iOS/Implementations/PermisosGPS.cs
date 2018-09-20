using CoreLocation;
using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using FarmaEnlace.Services;
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
        public int checkGpsPermission()
        {
            int status = GeolocatorService.ALLOWED;

            cLLocationManager = new CLLocationManager();
            if (CLLocationManager.Status <= CLAuthorizationStatus.Denied)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    cLLocationManager.RequestWhenInUseAuthorization();
                }

                if (CLLocationManager.Status == CLAuthorizationStatus.Denied) status= GeolocatorService.DENIED;
                else if (CLLocationManager.Status < CLAuthorizationStatus.Denied) status= GeolocatorService.UNDEFINED;
            }
            else
            {
                status= GeolocatorService.ALLOWED;
            }

            return status;
        }

        public void requestGPSActivation()
        {
            NSString settingsString = UIApplication.LaunchOptionsLocationKey;
            NSUrl url = new NSUrl(settingsString);
            UIApplication.SharedApplication.OpenUrl(url);
        }

    }
}