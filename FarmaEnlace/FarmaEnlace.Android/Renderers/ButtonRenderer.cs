using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;

[assembly: ExportRenderer(typeof(Button), typeof(FarmaEnlace.Android.Renderers.ButtonRender))]

namespace FarmaEnlace.Android.Renderers
{
    public class ButtonRender : ButtonRenderer
    {
        public ButtonRender(Context context) : base(context)
        {

        }
        protected override void OnDraw(global::Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            Control?.SetPadding(0, Control.PaddingTop, 0, Control.PaddingBottom);
        }
    }
}