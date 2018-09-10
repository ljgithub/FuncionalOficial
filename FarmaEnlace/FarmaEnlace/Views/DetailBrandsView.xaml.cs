using FarmaEnlace.Models;
using FarmaEnlace.Services;
using Xamarin.Forms;


namespace FarmaEnlace.Views
{

    public partial class DetailBrandsView : ContentPage
    {
        public DetailBrandsView()
        {
            InitializeComponent();
            var detailBrandsViewModel = ViewModels.DetailBrandsViewModel.GetInstance();

            


             CarouselImages.ItemSelected += (sender, args) =>
             {
                 var image = args.SelectedItem as ImageBrand;
                 if (image == null)
                     return;

                 detailBrandsViewModel.ItemSelected = image;
             };
        }
        
        protected override bool OnBackButtonPressed()
        {
            NavigationService navigationService = new NavigationService();
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => {
                await navigationService.BackOnBrand();               
            });


            return true;
        }
       


    }


}
