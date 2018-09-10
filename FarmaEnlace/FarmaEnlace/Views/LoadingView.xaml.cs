using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaEnlace.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingView : ContentPage
	{
        #region Services
        DataService dataService;
        NavigationService navigationService;
        private readonly HttpClient _clienteHttp = new HttpClient();
        #endregion

        #region Atributes
        #endregion


        public LoadingView()
		{
            InitializeComponent();
            navigationService = new NavigationService();
            dataService = new DataService();
            Image logo = null;
            Splash splash = null;
            var oldSplash = dataService.Get<Splash>(true).FirstOrDefault();
            if (oldSplash != null && !string.IsNullOrEmpty(oldSplash.ImageButton))
                splash = oldSplash;
            else
                splash = GetSplash();
            
            if (splash != null && !string.IsNullOrEmpty(splash.ImageBase64))
            {
                logo = new Image
                {
                    Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(splash.ImageBase64))),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFit
                };
                Grid grid = new Grid();
                grid.Children.Add(logo, 0, 0);
                Content = grid;
                if(!string.IsNullOrEmpty(splash.Color))
                    Content.BackgroundColor = Color.FromHex(splash.Color);
            }
            else
            {
                logo = new Image
                {
                    Source = "splash.png",
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFit
                };
                Grid grid = new Grid();
                grid.Children.Add(logo, 0, 0);
                Content = grid;
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(2000);
            MainViewModel.GetInstance().Brands = new BrandsViewModel();
            await navigationService.SetMainPage("BrandsView");
            GetSplashAsync();
            SaveStatisticAsync();
        }

        

        public Splash GetSplash()
        {
            Splash splash = new Splash();
            try
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                _clienteHttp.BaseAddress = new Uri(urlAPI);
                string consulta = Application.Current.Resources["PrefixAPI"].ToString() + "/GetSplash";
                string bannersJson = _clienteHttp.GetStringAsync(consulta).Result;
                if (!string.IsNullOrEmpty(bannersJson))
                {
                    splash = JsonConvert.DeserializeObject<Splash>(bannersJson);
                    dataService.DeleteAllAndInsert(splash);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ObtenerBannersParaCarrusel: " + ex);
            }
            return splash;
        }


        async public void GetSplashAsync()
        {
            var apiService = new ApiService();
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                return;
            }
            else
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();

                var response = await apiService.Get<Splash>(
                    urlAPI,
                    Application.Current.Resources["PrefixAPI"].ToString(),
                    "/GetSplash");

                if (!response.IsSuccess)
                {
                    return;
                }
                var splash = (Splash)response.Result;
                dataService.DeleteAllAndInsert(splash);
            }
        }


        private void SaveStatisticAsync()
        {
            try
            {
                SaveStatisticGeneralAsync();
                SaveStatisticCastegoriesAsync();
                SaveStatisticProductsAsync();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Se ha producido un error guardando las estadisticas....");
                Debug.WriteLine(e.ToString());
            }
        }

        private async void SaveStatisticGeneralAsync()
        {
            var apiService = new ApiService();
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                return;
            }
            else
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                List<StatisticGeneral> statisticsG = dataService.Get<StatisticGeneral>(false);
                var response = await apiService.SaveStaticts<StatisticGeneral>(
                    urlAPI,
                    Application.Current.Resources["PrefixAPI"].ToString(),
                    "/Statistic/SaveStatisticGeneral",
                    statisticsG
                    );
                if (response.IsSuccess)
                {
                    //hay que borrar los registros de cache
                    dataService.DeleteAll<StatisticGeneral>();
                }
            }
        }

        private async void SaveStatisticCastegoriesAsync()
        {
            var apiService = new ApiService();
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                return;
            }
            else
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                List<StatisticCategory> statistics = dataService.Get<StatisticCategory>(false);
                var response = await apiService.SaveStaticts<StatisticCategory>(
                    urlAPI,
                    Application.Current.Resources["PrefixAPI"].ToString(),
                    "/Statistic/SaveStatisticCategory",
                    statistics
                    );
                if (response.IsSuccess)
                {
                    //hay que borrar los registros de cache
                    dataService.DeleteAll<StatisticCategory>();
                }
            }
        }

        private async void SaveStatisticProductsAsync()
        {
            var apiService = new ApiService();
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                return;
            }
            else
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                List<StatisticProduct> statistics = dataService.Get<StatisticProduct>(false);
                var response = await apiService.SaveStaticts<StatisticProduct>(
                    urlAPI,
                    Application.Current.Resources["PrefixAPI"].ToString(),
                    "/Statistic/SaveStatisticProduct",
                    statistics
                    );
                if (response.IsSuccess)
                {
                    //hay que borrar los registros de cache
                    dataService.DeleteAll<StatisticProduct>();
                }
            }
        }
    }
}