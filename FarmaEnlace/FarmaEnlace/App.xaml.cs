using System;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using FarmaEnlace.Views;
using Xamarin.Forms;

namespace FarmaEnlace
{
    public partial class App : Application
    {
        #region Services
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }

        public static MasterView Master
        {
            get;
            internal set;
        }


        public static int ScreenWidth;
        public static int ScreenHeight;
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();
            FlowListView.Init();
            MainPage = new LoadingView();
        }
        #endregion

        #region Methods


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static async Task<Response> GetSplash()
        {
            var apiService = new ApiService();
            var dialogService = new DialogService();

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                return new Response { IsSuccess = false};
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.Get<Brand>(
                urlAPI,
                Application.Current.Resources["PrefixAPI"].ToString(),
                "/GetSplash");

            
            return response;
        }

        

        #endregion
    }
}
