using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Text;
using FarmaEnlace.Renderers;
using Android.Content;

[assembly: ExportRenderer(typeof(EntryBoxRenderer), typeof(FarmaEnlace.Android.Renderers.EntryBoxRenderer))]
namespace FarmaEnlace.Android.Renderers
{
    public class EntryBoxRenderer : EntryRenderer
    {
        public EntryBoxRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {

            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetTextColor(global::Android.Graphics.Color.ParseColor("#FE5000"));
                Control.InputType = InputTypes.TextFlagNoSuggestions;
                Control.SetBackgroundResource(Resource.Drawable.entrybox);
            }
        }
    }
}