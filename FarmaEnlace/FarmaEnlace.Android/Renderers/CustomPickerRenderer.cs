using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using FarmaEnlace.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FarmaEnlace.Renderers.CustomPicker), typeof(FarmaEnlace.Android.Renderers.CustomPickerRenderer))]
namespace FarmaEnlace.Android.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        CustomPicker element;

        public CustomPickerRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            element = (CustomPicker)this.Element;

            
            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#DDDDDD"));
                Control.Background = AddPickerStyles("flecha");
            }
            

            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawableIcon(element.Image), null, null, null);
                editText.SetPadding(0, 8, 8, 8);
            }
            Control.SetTextColor(global::Android.Graphics.Color.ParseColor("#8a8a8a"));
            editText.CompoundDrawablePadding = 40;
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = global::Android.Graphics.Color.ParseColor("#DDDDDD");
            
            border.SetPadding(0, 0, 30,0);
            border.Paint.SetStyle(Paint.Style.Fill);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 60, 60, true));
            
            result.Gravity = global::Android.Views.GravityFlags.Right;
            

            return result;
        }

        private BitmapDrawable GetDrawableIcon(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 140, 140, true));
        }


    }
}