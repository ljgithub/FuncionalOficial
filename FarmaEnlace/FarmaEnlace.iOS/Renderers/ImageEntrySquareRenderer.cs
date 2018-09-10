using System;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using FarmaEnlace.iOS.Renderers;
using FarmaEnlace.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageEntrySquare), typeof(ImageEntrySquareRenderer))]
namespace FarmaEnlace.iOS.Renderers
{
    public class ImageEntrySquareRenderer : EntryRenderer
    {
        private int tamanio = 50; //icon de tamanio 50x50
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var element = (ImageEntrySquare)this.Element;
            var textField = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        textField.LeftViewMode = UITextFieldViewMode.Always;
                        textField.LeftView = GetImageView(element.Image, tamanio, tamanio);
                        break;
                    case ImageAlignment.Right:
                        textField.RightViewMode = UITextFieldViewMode.Always;
                        textField.RightView = GetImageView(element.Image, tamanio, tamanio);
                        break;
                }
            }

            textField.BorderStyle = UITextBorderStyle.None;
            textField.Font = UIFont.FromName("Helvetica", 15f);
            textField.Background = UIImage.FromBundle("entryBackgroundSquare.png");
        }

        private UIView GetImageView(string imagePath, int height, int width)
        {
            UIImageView uiImageView = new UIImageView(ResizeImageIOS(imagePath, width, height));

            UIView objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, width + 10, height));
            objLeftView.AddSubview(uiImageView);

            return objLeftView;
        }

        #region resize img
        public static UIImage ResizeImageIOS(string imagePath, float width, float height)
        {
            UIImage originalImage = UIImage.FromBundle(imagePath);
            var Hoehe = originalImage.Size.Height;
            var Breite = originalImage.Size.Width;

            nfloat ZielHoehe = 0;
            nfloat ZielBreite = 0;

            if (Hoehe > Breite) 
            {
                ZielHoehe = height;
                nfloat teiler = Hoehe / height;
                ZielBreite = Breite / teiler;
            }
            else 
            {
                ZielBreite = width;
                nfloat teiler = Breite / width;
                ZielHoehe = Hoehe / teiler;
            }

            width = (float)ZielBreite;
            height = (float)ZielHoehe;

            UIGraphics.BeginImageContext(new SizeF(width, height));
            originalImage.Draw(new RectangleF(0, 0, width, height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return resizedImage;
        }
        #endregion
    }
}
