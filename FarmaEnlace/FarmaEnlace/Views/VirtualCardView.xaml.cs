using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public partial class VirtualCardView : ContentPage
    {
        

        public VirtualCardView()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            var virtualCardViewModel = ViewModels.VirtualCardViewModel.GetInstance();
            virtualCardViewModel.VisibleCode();
            base.OnAppearing();
            
        }

        protected override bool OnBackButtonPressed()
        {
            NavigationService navigationService = new NavigationService();
            Device.BeginInvokeOnMainThread(async () => {
                await navigationService.BackOnBrand();
            });
            return true;
        }

        
    }
}
