using System;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public class CommerceMapViewModel : BaseViewModel
    {

        #region Attributes
        bool _isRefreshing;
        bool _isVisibleInfo = false;
        string _filter;

        private bool isCallVoz;
        #endregion


        #region Services
        NavigationService navigationService;
        ObservableCollection<Pin> _pins;
        Commerce _commerce;
        GeolocatorService geolocatorService;
        DialogService dialogService;
        #endregion

        #region Properties


        public ObservableCollection<Pin> Pins
        {
            get { return this._pins; }
            set { SetValue(ref this._pins, value); }
        }

        public Commerce Commerce
        {
            get { return this._commerce; }
            set { SetValue(ref this._commerce, value); }
        }

       

        public string Filter
        {
            get { return this._filter; }
            set { SetValue(ref this._filter, value); }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { SetValue(ref this._isRefreshing, value); }
        }
        public bool IsVisibleInfo
        {
            get { return this._isVisibleInfo; }
            set { SetValue(ref this._isVisibleInfo, value); }
        }

        #endregion

        #region Constructors

        public CommerceMapViewModel()
        {
            instance = this;
            navigationService = new NavigationService();
            geolocatorService = new GeolocatorService();
            dialogService = new DialogService();
            MapCommerceCommand = new Command<int>(MapCommerce);

            isCallVoz = false;
        }
        #endregion

        #region Sigleton
        static CommerceMapViewModel instance;

        public static CommerceMapViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CommerceMapViewModel();
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
            Filter = cad;
            if (!string.IsNullOrEmpty(Filter))
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
                                //Filter = await DependencyService.Get<IVoice>().GetVoice();
                                //if (!string.IsNullOrEmpty(Filter))
                                //{
                                //    SearchCommerce();
                                //}
                                View contentPage = new VoiceRecognition_iOS("CommerceMapViewModel");
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
                        Filter = await DependencyService.Get<IVoice>().GetVoice();
                        if (!string.IsNullOrEmpty(Filter))
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
            
        }
        public ICommand MapCommerceCommand { get; private set; }

        private async void MapCommerce(int idCommerce)
        {
            try
            {
                string loc = string.Format("{0},{1}", Commerce.Latitude.ToString(CultureInfo.InvariantCulture), Commerce.Longitude.ToString(CultureInfo.InvariantCulture));

                await Task.Delay(500);
                switch (Xamarin.Forms.Device.RuntimePlatform)
                {
                    case Xamarin.Forms.Device.iOS:
                        bool existeGoogle = DependencyService.Get<IAppsSO>().ExistGoogleMaps();
                        if (existeGoogle)
                        {
                            Xamarin.Forms.Device.OpenUri(
                            new Uri(string.Format("comgooglemaps://?q={0}", System.Net.WebUtility.UrlEncode(loc))));
                        }
                        else
                        {
                            Xamarin.Forms.Device.OpenUri(
                            new Uri(string.Format("http://maps.apple.com/?q={0}", System.Net.WebUtility.UrlEncode(loc))));
                        }
                        break;
                    case Xamarin.Forms.Device.Android:
                        Xamarin.Forms.Device.OpenUri(
                          new Uri(string.Format("geo:0,0?q={0}", System.Net.WebUtility.UrlEncode(loc))));
                        break;
                    //case Xamarin.Forms.Device.WinPhone:
                    //    Xamarin.Forms.Device.OpenUri(
                    //      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(loc))));
                    //    break;
                }
            }
            catch (Exception)
            {

                await dialogService.ShowMessage(
                 Resources.Resource.Error,
                  "Estimado usuario, por favor, reintente su busqueda");
            }
            
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

        async void SearchCommerce()
        {
            IsRefreshing = true;

            try
            {
                if (!string.IsNullOrEmpty(Filter))
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
                        mainViewModel.CommercesList.Filter = Filter;
                        mainViewModel.CommercesList.TwentyFourHours = false;
                        mainViewModel.CommercesList.IsVisible = true;
                        await navigationService.BackOn();
                    }

                }
            }
            catch (Exception)
            {

                await dialogService.ShowMessage(
                Resources.Resource.Error,
                 "Estimado usuario, por favor, reintente su busqueda");
            }
            
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
                    permisoGPS.verificarPermisosGPS();
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
