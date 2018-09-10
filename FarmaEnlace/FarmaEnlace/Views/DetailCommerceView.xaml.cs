using System.Threading.Tasks;
using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.Views
{
    public partial class DetailCommerceView : ContentPage
    {
        #region Services
        GeolocatorService geolocatorService;
        #endregion

        #region Constructors
        public DetailCommerceView()
        {
            InitializeComponent();

            geolocatorService = new GeolocatorService();

            MoveMapToCurrentPosition();
        }
        #endregion

        #region Methods
        async void MoveMapToCurrentPosition()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    geolocatorService.Latitude,
                    geolocatorService.Longitude);
            }

            await LoadPins();
        }

        async Task LoadPins()
        {
            var commercesViewModel = DetailCommerceViewModel.GetInstance();
            commercesViewModel.LoadPins(geolocatorService.Latitude, geolocatorService.Longitude);
            bool isFirst = true;
            foreach (var pin in commercesViewModel.Pins)
            {
                if (isFirst)
                {
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    pin.Position,
                    Distance.FromKilometers(3)));
                    isFirst = false;
                }
                MyMap.Pins.Add(pin);
            }
            
           
        }
        #endregion
    }
}
