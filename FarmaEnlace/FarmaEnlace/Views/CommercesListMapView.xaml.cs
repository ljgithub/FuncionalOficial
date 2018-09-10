using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Renderers;
using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.Views
{
    public partial class CommercesListMapView : ContentPage
    {


        #region Services
        GeolocatorService geolocatorService;
        #endregion

        #region Constructors
        public CommercesListMapView()
        {
            InitializeComponent();

            geolocatorService = new GeolocatorService();

            Position posicionEcuador = new Xamarin.Forms.Maps.Position(-0.187806, -78.492231);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(posicionEcuador, Distance.FromKilometers(250)));
            var res = LoadPins();
        }
        #endregion

        

        #region Methods

        async Task<bool> LoadPins()
        {
            var mainViewModel = MainViewModel.GetInstance();
            var commercesListMapViewModel = CommercesListMapViewModel.GetInstance();
            var Pins = commercesListMapViewModel.Pins;
            List<Position> position = new List<Position>();
            MyMap.CustomPins = new List<CustomPin>();
            foreach (var pin in Pins)
            {
                var customPin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = pin.Position,
                    Label = pin.Label,
                    Address = pin.Address,
                    Id = mainViewModel.Brand.SearchCode,
                    Url = mainViewModel.Brand.SearchCode
                };
                MyMap.CustomPins.Add(customPin);
                MyMap.Pins.Add(customPin);
                position.Add(pin.Position);
            }
            MyMap.MoveToRegion(FromPositions(position));
            return true;
        }

        private static MapSpan FromPositions(List<Position> positions)
        {
            double minLat = double.MaxValue;
            double minLon = double.MaxValue;
            double maxLat = double.MinValue;
            double maxLon = double.MinValue;

            foreach (var p in positions)
            {
                minLat = Math.Min(minLat, p.Latitude);
                minLon = Math.Min(minLon, p.Longitude);
                maxLat = Math.Max(maxLat, p.Latitude);
                maxLon = Math.Max(maxLon, p.Longitude);
            }

            return new MapSpan(
                new Position((minLat + maxLat) / 2d, (minLon + maxLon) / 2d),
                (maxLat - minLat) + 0.02, (maxLon - minLon) + 0.02);
        }




        #endregion

    }
}
