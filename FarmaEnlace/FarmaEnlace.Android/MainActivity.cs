using Acr.UserDialogs;
using Rg.Plugins.Popup;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FarmaEnlace.Android.Implementations;
using Xamarin.Forms;
using ZXing.Mobile;
using CarouselView.FormsPlugin.Android;
using Android.Content;
using FarmaEnlace.Android.Helper;
using Android.Speech;
using Android.Util;
using Android.Graphics;
using System.Net;
using Android.Widget;
using Android.Views;
using FFImageLoading.Forms.Droid;

namespace FarmaEnlace.Android
{
    [Activity(Label = "Tu App Farmacia", Icon = "@drawable/iclauncher",
              Theme = "@style/MainTheme",
              MainLauncher = false,
              ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        SpeechRecognizer Recognizer { get; set; }
        Intent SpeechIntent { get; set; }

        #region Methods
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(() => this);
            Xamarin.FormsMaps.Init(this, bundle);
            MobileBarcodeScanner.Initialize(this.Application);
            DependencyService.Register<FileStore>();
            DependencyService.Register<IShareService>();
            DependencyService.Register<CloseApplication>();
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init(true);

            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);


            LoadApplication(new App());
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (IntentHelper.IsVoiceIntent(requestCode))
            {
                IntentHelper.ActivityResult(requestCode, data);
            }
        }

        #endregion
    }

    

}

