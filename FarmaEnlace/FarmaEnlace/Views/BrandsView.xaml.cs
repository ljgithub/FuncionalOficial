using FarmaEnlace.Interfaces;
using FarmaEnlace.Services;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace FarmaEnlace.Views
{
   
    public partial class BrandsView : ContentPage
    {
        public BrandsView()
        {
            InitializeComponent();

            //en el caso que sea un iPhone x el margen en el top tiene que ser 24 mas
            if (App.ScreenWidth == 375 && App.ScreenHeight == 812)
            {
                BransViewName.Margin =   new Thickness (0,15,0,0);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            DialogService dialogService = new DialogService();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await dialogService.ShowConfirm("Alerta!", "¿Desea salir?");
                if (result)
                {
                    var closer = DependencyService.Get<ICloseApplication>();
                    closer?.closeApplication();
                }
            });

            return true;
        }

        protected override void OnAppearing()
        {
            var brandsViewModel = ViewModels.BrandsViewModel.GetInstance();
            brandsViewModel.LoadImagesBrand();
            base.OnAppearing();
        }
    }
}
