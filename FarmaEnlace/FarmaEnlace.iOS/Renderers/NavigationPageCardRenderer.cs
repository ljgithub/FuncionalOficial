using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FarmaEnlace.Renderers.NavigationPageCardRenderer), typeof(FarmaEnlace.iOS.NavigationPageCardRenderer))]
namespace FarmaEnlace.iOS
{
   public class NavigationPageCardRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {                
                string logo = Renderers.Utils.GetLogoUIResponsive("logo");              

                var img = UIImage.FromFile(logo);
                this.NavigationBar.SetBackgroundImage(img, UIBarMetrics.Default);
            }
        }
    }
}
