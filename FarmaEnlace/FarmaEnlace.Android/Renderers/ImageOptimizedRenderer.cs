using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using System.ComponentModel;
using System.Linq;
using Android.Content;

[assembly: ExportRenderer(typeof(FarmaEnlace.Renderers.ImageOptimizedRenderer), typeof(FarmaEnlace.Android.Renderers.ImageOptimizedRenderer))]

namespace FarmaEnlace.Android.Renderers
{
    public class ImageOptimizedRenderer : ImageRenderer
    {
        private bool _isDecoded;
        public ImageOptimizedRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            FarmaEnlace.Renderers.ImageOptimizedRenderer largeImage = (FarmaEnlace.Renderers.ImageOptimizedRenderer)Element;

            if ((!(Element.Width > 0) || !(Element.Height > 0) || _isDecoded) &&
                (e.PropertyName != "ImageSource" || largeImage.ImageSource == null)) return;
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };

            //Get the resource id for the image
            System.Reflection.FieldInfo field = typeof(Resource.Drawable).GetField(largeImage.ImageSource.Split('.').First());
            if (field == null) return;
            int value = (int)field.GetRawConstantValue();

            BitmapFactory.DecodeResource(Context.Resources, value, options);

            //The with and height of the elements (XLargeImage) will be used to decode the image
            int width = (int)Element.Width;
            int height = (int)Element.Height;
            options.InSampleSize = CalculateInSampleSize(options, width, height);

            options.InJustDecodeBounds = false;
            Bitmap bitmap = BitmapFactory.DecodeResource(Context.Resources, value, options);

            //Set the bitmap to the native control
            Control.SetImageBitmap(bitmap);

            _isDecoded = true;
        }

        public int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (!(height > reqHeight) && !(width > reqWidth)) return (int)inSampleSize;
            int halfHeight = (int)(height / 2);
            int halfWidth = (int)(width / 2);

            // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
            while ((halfHeight / inSampleSize > reqHeight) && (halfWidth / inSampleSize > reqWidth))
            {
                inSampleSize *= 2;
            }

            return (int)inSampleSize;
        }
    }
}