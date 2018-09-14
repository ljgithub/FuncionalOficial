using System;
using System.Collections.Generic;
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
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.ViewModels
{
    public class CommercesListMapViewModel : BaseViewModel
    {

        #region Attributes
        bool _isRefreshing;
        string _filterMap;

        private bool isCallVoz;
        #endregion


        #region Services
        NavigationService navigationService;
        ObservableCollection<Pin> _pins;
        GeolocatorService geolocatorService;
        DialogService dialogService;
        #endregion

        #region Properties


        public ObservableCollection<Pin> Pins
        {
            get { return this._pins; }
            set { SetValue(ref this._pins, value); }
        }

        public string FilterMap
        {
            get { return this._filterMap; }
            set { SetValue(ref this._filterMap, value); }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { SetValue(ref this._isRefreshing, value); }
        }

        #endregion

        #region Constructors

        public CommercesListMapViewModel()
        {
            instance = this;
            navigationService = new NavigationService();
            geolocatorService = new GeolocatorService();
            dialogService = new DialogService();

            isCallVoz = false;
        }
        #endregion

        #region Sigleton
        static CommercesListMapViewModel instance;

        public static CommercesListMapViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CommercesListMapViewModel();
            }

            return instance;
        }


        #endregion

        #region Methos
        async public Task<ObservableCollection<Pin>> Load()
        {
            await Task.Delay(500);
            return Pins;
        }

        public void SearchVoz(string cad)
        {
            FilterMap = cad;
            if (!string.IsNullOrEmpty(FilterMap))
            {
                SearchCommerce();
            }
        }

        #endregion

        #region Commands

        public ICommand VozCommand
        {
            get
            {
                return new RelayCommand(Voz);
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
                                //FilterMap = await DependencyService.Get<IVoice>().GetVoice();
                                //if (!string.IsNullOrEmpty(FilterMap))
                                //{
                                //    SearchCommerce();
                                //}
                                View contentPage = new VoiceRecognition_iOS("CommerceListMapViewModel");
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
                        FilterMap = await DependencyService.Get<IVoice>().GetVoice();
                        if (!string.IsNullOrEmpty(FilterMap))
                        {
                            SearchCommerce();
                        }
                    }
                    isCallVoz = false;
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
               Resources.Resource.Error,
                "Estimado usuario, por favor, reintente su busqueda");
                isCallVoz = false;
            }
            
            FilterMap = string.Empty;

        }

        async void Listlink()
        {
            await navigationService.BackOn();
        }

        public ICommand ListlinkCommand
        {
            get
            {
                return new RelayCommand(Listlink);
            }
        }


        public ICommand SearchCommerceCommand
        {
            get
            {
                return new RelayCommand(SearchCommerce);
            }
        }

        async void  SearchCommerce()
        {
            IsRefreshing = true;
            try
            {
                if (!string.IsNullOrEmpty(FilterMap))
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
                        var mainViewModel = MainViewModel.GetInstance();
                        mainViewModel.CommercesList = new CommercesListViewModel();
                        mainViewModel.CommercesList.NearbyPharmacies = false;
                        mainViewModel.CommercesList.Filter = FilterMap;
                        mainViewModel.CommercesList.TwentyFourHours = false;
                        mainViewModel.CommercesList.IsVisible = true;
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
            
            FilterMap = string.Empty;
            IsRefreshing = false;
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
                    permisoGPS.activatePermissions();
                }
            }
            catch (Exception)
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
