using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Text;
using FarmaEnlace.Android.Renderers;
using FarmaEnlace.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageEntrySquare), typeof(ImageEntrySquareRenderer))]
namespace FarmaEnlace.Android.Renderers
{
    public class ImageEntrySquareRenderer : EntryRenderer
    {
        ImageEntrySquare element;
        public ImageEntrySquareRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntrySquare)this.Element;


            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }

            Control.SetTextColor(global::Android.Graphics.Color.ParseColor("#595959"));
            
            Control.InputType = InputTypes.TextFlagNoSuggestions;
            Control.SetBackgroundResource(Resource.Drawable.entryboxSquare);
            editText.CompoundDrawablePadding = 40;
            editText.Enabled = element.IsEnabled;
            if (element.IsPassword)
            {
                editText.TransformationMethod = global::Android.Text.Method.PasswordTransformationMethod.Instance;
            }
            if (element.Keyboard != null)
            {
                editText.InputType = element.Keyboard.ToInputType(); 
            }
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 140,140, true));
        }

    }
}