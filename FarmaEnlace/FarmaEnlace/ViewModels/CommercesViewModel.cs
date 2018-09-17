
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class CommercesViewModel : BaseViewModel
    {
        

        #region Attributes
        bool _isToggled;
        bool _isRefreshing;
        string _filter;
        string _location;
        bool _isVisibleButton;
        bool _isVisibleSearchBar;
        ApiService apiService;
        #endregion

        #region Properties

        public bool IsToggled
        {
            get { return this._isToggled; }
            set { SetValue(ref this._isToggled, value); }
        }
        
        public string Filter
        {
            get { return this._filter; }
            set { SetValue(ref this._filter, value); }
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
        public CommercesViewModel()
        {
            navigationService = new NavigationService();
            geolocatorService = new GeolocatorService();
            dialogService = new DialogService();
            instance = this;
        }
        #endregion

        #region Sigleton
        static CommercesViewModel instance;

        public static CommercesViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new CommercesViewModel();
            }

            return instance;
        }
        #endregion

       

        #region Commands
        public ICommand NearbyPharmaciesCommand
        {
            get
            {
                return new RelayCommand(NearbyPharmacies);
            }
        }

        public async Task<bool> CheckIntenetAvaibility()
        {
            apiService = new ApiService();
            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await dialogService.ShowMessage(
                       Resources.Resource.Info,
                       Resources.Resource.ErrorConection);
                UserDialogs.Instance.HideLoading();
                return false;
            }
            else
            {
                return true;
            }
        }

        async public void NearbyPharmacies()
        {
            IGeolocator locator =  CrossGeolocator.Current;
            bool available=false;
            try
            {                
                if (locator.IsGeolocationEnabled==false)
                {
                     available = await GeolocatorService.checkLocationAvaibility();
                }else                
                {
                        UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                        bool hasInternetAccess = await CheckIntenetAvaibility();
                        bool hasLocation = DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().findLocation(hasInternetAccess);

                        if (hasLocation)
                        {
                            UserDialogs.Instance.HideLoading();
                            var mainViewModel = MainViewModel.GetInstance();
                            mainViewModel.CommercesList = new CommercesListViewModel();
                            mainViewModel.CommercesList.NearbyPharmacies = true;
                            mainViewModel.CommercesList.Filter = string.Empty;
                            mainViewModel.CommercesList.TwentyFourHours = false;
                            mainViewModel.CommercesList.IsVisible = true;
                            mainViewModel.CommercesList.Latitude = GeolocatorService.Latitude;
                            mainViewModel.CommercesList.Longitude = GeolocatorService.Longitude;
                            await navigationService.NavigateOnMaster("CommercesListView");
                        }
                }
                
            }
            catch (Exception e)
            {

                UserDialogs.Instance.HideLoading();
                await dialogService.ShowMessage(
               Resources.Resource.Error,
                "Estimado usuario, por favor, reintente su busqueda");
            }

            
        }

     /*   public ICommand SearchCommerceCommand
        {
            get
            {
                return new RelayCommand(SearchCommerce);
            }
        }

        async void SearchCommerce()
        {
            
            try
            {
                if (!string.IsNullOrEmpty(Filter))
                {
                    Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
                    if (locator.IsGeolocationEnabled == false)
                    {
                        UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
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
                        mainViewModel.CommercesList.Filter = Filter;
                        mainViewModel.CommercesList.TwentyFourHours = false;
                        mainViewModel.CommercesList.IsVisible = true;
                        mainViewModel.CommercesList.Latitude = GeolocatorService.Latitude;
                        mainViewModel.CommercesList.Longitude = GeolocatorService.Longitude;
                        await navigationService.NavigateOnMaster("CommercesListView");
                    }

                }

            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                   Resources.Resource.Error,
                    "Estimado usuario, por favor, reintente su busqueda");
            }
        }*/

        public ICommand TwentyFourHoursPharmaciesCommand
        {
            get
            {
                return new RelayCommand(TwentyFourHoursPharmacies);
            }
        }

        async void TwentyFourHoursPharmacies()
        {
            IGeolocator locator = CrossGeolocator.Current;
            bool available = false;
            try
            {
                if (locator.IsGeolocationEnabled == false)
                {
                    available = await GeolocatorService.checkLocationAvaibility();
                }
                else
                {
                    UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                    bool hasInternetAccess = await CheckIntenetAvaibility();
                    bool hasLocation = DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().findLocation(hasInternetAccess);

                    if (hasLocation)
                    {
                        UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                        UserDialogs.Instance.HideLoading();
                        var mainViewModel = MainViewModel.GetInstance();
                        mainViewModel.CommercesList = new CommercesListViewModel();
                        mainViewModel.CommercesList.NearbyPharmacies = false;
                        mainViewModel.CommercesList.Filter = string.Empty;
                        mainViewModel.CommercesList.TwentyFourHours = true;
                        mainViewModel.CommercesList.IsVisible = false;
                        await navigationService.NavigateOnMaster("CommercesListView");
                    }
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                   Resources.Resource.Error,
                    "Estimado usuario, por favor, reintente su busqueda");
            }
        }

      
        public ICommand SearchPharmaciesCommand
        {
            get
            {
                return new RelayCommand(SearchPharmacies);
            }
        }

        async void SearchPharmacies()
        {
            var mainViewModel = MainViewModel.GetInstance();
            IGeolocator locator = CrossGeolocator.Current;
            bool available = false;
            try
            {
                if (locator.IsGeolocationEnabled == false)
                {
                    available = await GeolocatorService.checkLocationAvaibility();
                }
                else
                {
                    UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                    bool hasInternetAccess = await CheckIntenetAvaibility();
                    bool hasLocation = DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().findLocation(hasInternetAccess);

                    if (hasLocation)
                    {
                        UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                        mainViewModel.CommercesSearch = new CommercesSearchViewModel();
                        await navigationService.NavigateOnMaster("CommercesSearchView");
                        UserDialogs.Instance.HideLoading();
                    }
                }
            }
            catch (Exception)
            {

                await dialogService.ShowMessageBrand(
                Resources.Resource.Error,
                  "Estimado usuario, por favor, reintente su busqueda",
                  "iconinfo",
                  mainViewModel.Brand.SearchCode);
                UserDialogs.Instance.HideLoading();
            }
            
        }

        #endregion



    }
}
