using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(AppsSO))]
namespace FarmaEnlace.iOS.Implementations
{
    public class AppsSO : IAppsSO
    {
        public bool ExistGoogleMaps()
        {
            if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl("comgooglemaps://")))
            {
                return true;
            }
            else
                return false;
        }
    }
}