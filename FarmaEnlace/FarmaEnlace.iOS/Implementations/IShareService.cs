
using Foundation;
using FarmaEnlace.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FarmaEnlace.iOS.Implementations;

[assembly: Dependency(typeof(IShareService))]
namespace FarmaEnlace.iOS.Implementations
{
    public class IShareService : IShare
    {
        public async void Share(string subject, string message, ImageSource image)
        {
            var handler = new ImageLoaderSourceHandler();
            var uiImage = await handler.LoadImageAsync(image);

            var img = NSObject.FromObject(uiImage);
            var mess = NSObject.FromObject(message);

            var activityItems = new[] { mess, img };
            var activityController = new UIActivityViewController(activityItems, null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                while (topController.PresentedViewController != null)
                {
                    topController = topController.PresentedViewController;
                }
                topController.PresentViewController(activityController, true, () => { });
            }
            else
            {
                //La forma de presentar en el ipad es mediante un Popover por lo que hay que deinirlo el source.
                //el rect tambien se define para que no se vea tan arriba
                var popover = activityController.PopoverPresentationController;
                if (popover != null)
                {
                    popover.SourceView = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                    popover.SourceRect =  new CoreGraphics.CGRect((UIApplication.SharedApplication.KeyWindow.RootViewController.View.Bounds.Width / 2), (UIApplication.SharedApplication.KeyWindow.RootViewController.View.Bounds.Height / 4), 0, 0);
                }
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(activityController, true, null);
            }            
        }
    }
}
