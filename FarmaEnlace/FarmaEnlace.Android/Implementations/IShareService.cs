﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using FarmaEnlace.Android.Implementations;
using FarmaEnlace.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(IShareService))]
namespace FarmaEnlace.Android.Implementations
{
    public class IShareService : Activity, IShare
    {
        public async void Share(string subject, string message,
        ImageSource image)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/png");

            var handler = new ImageLoaderSourceHandler();
            var bitmap = await handler.LoadImageAsync(image, this);

            var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                + Java.IO.File.Separator + "logo.png");

            using (var os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
            }

            intent.PutExtra(Intent.ExtraStream, global::Android.Net.Uri.FromFile(path));
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Compartir Imagen"));

        }
    }
}
