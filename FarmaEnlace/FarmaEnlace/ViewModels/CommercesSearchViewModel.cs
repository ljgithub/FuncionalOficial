
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.Views;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class CommercesSearchViewModel : BaseViewModel
    {
        

        #region Attributes
        bool _isToggled;
        bool _isRefreshing;
        string _filterSearch;
        string _location;
        bool _isVisibleButton;
        bool _isVisibleSearchBar;

        private bool isCallVoz;
        #endregion

        #region Properties

        public bool IsToggled
        {
            get { return this._isToggled; }
            set { SetValue(ref this._isToggled, value); }
        }
        
        public string FilterSearch
        {
            get { return this._filterSearch; }
            set { SetValue(ref this._filterSearch, value); }
        }
        public string Location
        {
            get { return this._location; }
            set { SetValue(ref this._location, value); }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { SetValue(ref this._isRefreshing, value); }
        }

        public bool IsVisibleSearchBar
        {
            get { return this._isVisibleSearchBar; }
            set { SetValue(ref this._isVisibleSearchBar, value); }
        }
        public bool IsVisibleButton
        {
            get { return this._isVisibleButton; }
            set { SetValue(ref this._isVisibleButton, value); }
        }
        

        #endregion


        #region Services
        NavigationService navigationService;
        GeolocatorService geolocatorService;
        DialogService dialogService;
        #endregion


        #region Constructor
        public CommercesSearchViewModel()
        {
            instance = this;
            navigationService = new NavigationService();
            geolocatorService = new GeolocatorService();
            dialogService = new DialogService();

            isCallVoz = false;
        }
        #endregion

        #region Sigleton
        static CommercesSearchViewModel instance;

        public static CommercesSearchViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new CommercesSearchViewModel();
            }

            return instance;
        }
        #endregion

        async Task<bool> MoveMapToCurrentPosition(bool hasInternetAccess)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                //poner en un a implementacion o un if pero deberia ser parte del checkLocationAvaibili
                //cLLocationManager.RequestWhenInUseAuthorization(); vovler a activar para ios
            }

            DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().findLocation(hasInternetAccess);

            if (GeolocatorService.Latitude != 0 && GeolocatorService.Longitude != 0)
            { return true; }
            else { return false; }
        }


        #region Methods

        #endregion

        #region Commands

        public ICommand VozCommand
        {
            get
            {
                return new RelayCommand(Voz);
            }
        }

        public void SearchVoz(string cad)
        {
            FilterSearch = cad;
            if (!string.IsNullOrEmpty(FilterSearch))
            {
                SearchCommerce();
            }
        }

        async void Voz()
        {
            try
            {
                if (!isCallVoz)
                {
                    isCallVoz = true;
                    if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                    {
                        string systemVersion = DependencyService.Get<IDeviceSO>().GetDeviceSO();
                        string splitVersion = systemVersion.Split('.').FirstOrDefault();
                        int version = 0;
                        if (int.TryParse(splitVersion, out version))
                        {
                            if (version >= 10)
                            {
                                //FilterSearch = await DependencyService.Get<IVoice>().GetVoice();
                                //if (!string.IsNullOrEmpty(FilterSearch))
                                //{
                                //    SearchCommerce();
                                //}
                                View contentPage = new VoiceRecognition_iOS("CommercesSearchViewModel");
                                await dialogService.ShowPopUpViewAlwaysVisible(contentPage);
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Alerta", "Su versión de iPhone no es compatible con esta opción", "Aceptar");
                            }
                        }
                    }
                    else
                    {
                        FilterSearch = await DependencyService.Get<IVoice>().GetVoice();
                        if (!string.IsNullOrEmpty(FilterSearch))
                        {
                            SearchCommerce();
                        }
                    }
                    isCallVoz = false;
                }
            }
            catch(Exception)
            {
                var mainViewModel = MainViewModel.GetInstance();
                await dialogService.ShowMessageBrand(
                              Resources.Resource.Error,
                              "Estimado usuario, por favor, reintente su busqueda",
                              "iconinfo",
                              mainViewModel.Brand.SearchCode);
                isCallVoz = false;
            }
        }

        public ICommand SearchCommerceCommand
        {
            get
            {
                return new RelayCommand(SearchCommerce);
            }
        }


      

        public async void  SearchCommerce()
        {
            try
            {
                if (!string.IsNullOrEmpty(FilterSearch))
                {
                    Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
                    if (locator.IsGeolocationEnabled == false)
                    {
                        UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                        bool response = await load();
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                        bool resp = await MoveMapToCurrentPosition(false);
                        UserDialogs.Instance.HideLoading();
                        var mainViewModel = MainViewModel.GetInstance();
                        mainViewModel.CommercesList = new CommercesListViewModel();
                        mainViewModel.CommercesList.NearbyPharmacies = false;
                        mainViewModel.CommercesList.Filter = FilterSearch;
                        mainViewModel.CommercesList.TwentyFourHours = false;
                        mainViewModel.CommercesList.Latitude = GeolocatorService.Latitude;
                        mainViewModel.CommercesList.Longitude = GeolocatorService.Longitude;
                        mainViewModel.CommercesList.IsVisible = true;
                        await navigationService.NavigateOnMaster("CommercesListView");

                    }
                }
                else
                {
                    await dialogService.ShowMessage(
                       Resources.Resource.Error,
                       "Ingrese algún criterio de búsqueda");
                }
            }
            catch (System.Exception)
            {
                await dialogService.ShowMessage(
                Resources.Resource.Error,
                 "Estimado usuario, por favor, reintente su busqueda");
            }
            

            FilterSearch = string.Empty;
        }

        

        private async Task<bool> load()
        {
            bool respuesta = false;
            try
            {
                respuesta = await dialogService.ShowConfirm("", "Para continuar, permite que tu dispositivo active la ubicación, que se usa en el servicio de ubicación.");
                if (respuesta)
                {
                    IPermisosGPS permisoGPS = DependencyService.Get<IPermisosGPS>();
                    permisoGPS.requestGPSActivation();
                }
            }
            catch (System.Exception)
            {

                await dialogService.ShowMessage(
                Resources.Resource.Error,
                 "Estimado usuario, por favor, reintente su busqueda");
            }
           
            return respuesta;
        }

        

        #endregion



    }
}
