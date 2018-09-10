using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System.Net;

namespace FarmaEnlace.Android
{
    [Activity(Theme = "@style/Theme.Splash",
              MainLauncher = true, ResizeableActivity =true,
              NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.StartActivity(typeof(MainActivity));
        }
    }
}
