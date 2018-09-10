using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Android.Support.V7.Widget;
using Android.Content;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FarmaEnlace.Renderers.NavigationPageRenderer), typeof(FarmaEnlace.Android.NavPageRenderer))]
namespace FarmaEnlace.Android
{
    public class NavPageRenderer : NavigationPageRenderer
    {
        private Toolbar toolbar;

        public NavPageRenderer(Context context) : base(context)
        {

        }

        public override void OnViewAdded(global::Android.Views.View child)
        {
            base.OnViewAdded(child);
            if (child.GetType() == typeof(Toolbar))
            {
                toolbar = (Toolbar)child;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && toolbar != null)
            {
                FarmaEnlace.Renderers.NavigationPageRenderer obj = (FarmaEnlace.Renderers.NavigationPageRenderer)e.NewElement;
                string logoName = obj.Logo;
                int drawableId = Resources.GetIdentifier(logoName.Split('.')[0], "drawable", this.Context.PackageName);
                global::Android.Widget.ImageView imageInToolbar = (global::Android.Widget.ImageView)toolbar.FindViewById(Resource.Id.image);
                imageInToolbar.SetImageResource(drawableId);
            }

        }
    }
}