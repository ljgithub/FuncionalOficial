using CoreAnimation;
using CoreGraphics;
using FarmaEnlace.iOS;
using FarmaEnlace.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FarmaEnlace.Renderers.NavigationPageRenderer), typeof(FarmaEnlace.iOS.NavigationPageRenderer))]
namespace FarmaEnlace.iOS
{
   public class NavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                FarmaEnlace.Renderers.NavigationPageRenderer obj = (FarmaEnlace.Renderers.NavigationPageRenderer)e.NewElement;
                string logo = obj.Logo;

                string nameImg = "";
                if (!string.IsNullOrEmpty(logo))
                {
                    nameImg = logo.Split('.')[0];
                    logo = FarmaEnlace.iOS.Renderers.Utils.GetLogoUIResponsive(nameImg);
                }

                var img = UIImage.FromFile(logo);
                UIView contentView=this.NavigationBar.Subviews[2];                
                UIImageView imgview=new UIImageView(img);
                var imageHeight = this.NavigationBar.Frame.Size.Height+7;
                imgview.ContentMode = UIViewContentMode.ScaleAspectFit;
                contentView.Add(imgview);

                if (logo.Contains("eco")) {
                    imageHeight = imageHeight -8;
                }
                if (logo.Contains("pun"))
                {
                    imageHeight = imageHeight - 8;
                }
                imgview.Frame = new CGRect(0, 0, imgview.Frame.Size.Width, imageHeight);
                imgview.ContentMode = UIViewContentMode.ScaleAspectFit;

                //SetBackgroundImage(img, UIBarMetrics.Default);


            }
        }
    }
}
