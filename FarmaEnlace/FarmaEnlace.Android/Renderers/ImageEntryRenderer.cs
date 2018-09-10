using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Text;
using FarmaEnlace.Android.Renderers;
using FarmaEnlace.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace FarmaEnlace.Android.Renderers
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;
        public ImageEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntry)this.Element;


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
            Control.SetTextColor(global::Android.Graphics.Color.ParseColor("#ffffff"));
            Control.SetHintTextColor(global::Android.Graphics.Color.ParseColor("#ffffff"));
            Control.InputType = InputTypes.TextFlagNoSuggestions;
            Control.SetBackgroundResource(Resource.Drawable.entrybox);
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

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }

    }
}