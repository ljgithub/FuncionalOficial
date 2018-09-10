using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using FarmaEnlace.Android.Renderers;
using FarmaEnlace.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace FarmaEnlace.Android.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            //NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            if ((string)pin.Id == "002")
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.med_iconubicmapa));
            else if ((string)pin.Id == "010")
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pun_iconubicmapa));
            else
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.iconubicmapa));

            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            //var customPin = GetCustomPin(e.Marker);
            //if (customPin == null)
            //{
            //    throw new Exception("Custom pin not found");
            //}

            //if (!string.IsNullOrWhiteSpace(customPin.Url))
            //{
            //    var url = global::Android.Net.Uri.Parse(customPin.Url);
            //    var intent = new Intent(Intent.ActionView, url);
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    global::Android.App.Application.Context.StartActivity(intent);
            //}
        }

        //public global::Android.Views.View GetInfoContents(Marker marker)
        //{
        //    var inflater = global::Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
        //    if (inflater != null)
        //    {
        //        global::Android.Views.View view;

        //        var customPin = GetCustomPin(marker);
        //        if (customPin == null)
        //        {
        //            throw new Exception("Custom pin not found");
        //        }

        //        //if (customPin.Id.ToString() == "Xamarin")
        //        //{
        //        //    view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
        //        //}
        //        //else
        //        //{
        //        //    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
        //        //}

        //        //var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
        //        //var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

        //        if (infoTitle != null)
        //        {
        //            infoTitle.Text = marker.Title;
        //        }
        //        if (infoSubtitle != null)
        //        {
        //            infoSubtitle.Text = marker.Snippet;
        //        }

        //        return view;
        //    }
        //    return null;
        //}

        public global::Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            //var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            //foreach (var pin in customPins)
            //{
            //    if (pin.Position == position)
            //    {
            //        return pin;
            //    }
            //}
            return null;
        }
    }
}