
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using CoreLocation;
//using CoreLocation;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.Views;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
using Xamarin.Forms;

//using Android.Gms.Location;

namespace FarmaEnlace.ViewModels
{
    public class DetailProductViewModel : BaseViewModel
    {

        #region Atributes
        string barCode;
        string _filter;
        string _textColorName;
        string _textColorPrice;

        string _nameBrand;
        private bool isCallVoz;
        private bool isCallScan;
        CLLocationManager cLLocationManager;
        public List<Product> ListProduct { get; set; }



        #endregion

        #region Services
        GeolocatorService geolocatorService;
        DialogService dialogService;
        NavigationService navigationService;
        ApiService apiService;
        List<StockProduct> stockProduct;
        List<Product> products;

        //LocationRequest mLocationRequest;

        #endregion

        #region Properties
        public ICommand VozCommand
        {
            get
            {
                return new RelayCommand(Voz);
            }
        }


        public double ContentHeight
        {
            get;
            set;
        }

        public Product DetailProduct
        {
            get;
            set;
        }
        public string BarCode
        {
            get { return this.barCode; }
            set { SetValue(ref this.barCode, value); }
        }

        public string Filter
        {
            get { return this._filter; }
            set { SetValue(ref this._filter, value); }
        }

        public string TextColorName
        {
            get { return this._textColorName; }
            set { SetValue(ref this._textColorName, value); }
        }

        public string TextColorPrice
        {
            get { return this._textColorPrice; }
            set { SetValue(ref this._textColorPrice, value); }
        }

        public string NameBrand
        {
            get { return this._nameBrand; }
            set { SetValue(ref this._nameBrand, value); }
        }
        #endregion

        #region Constructor
        public DetailProductViewModel(Product product)
        {
            stockProduct = new List<StockProduct>();
            DetailProduct = product;
            dialogService = new DialogService();
            navigationService = new NavigationService();
            apiService = new ApiService();
            geolocatorService = new GeolocatorService();
            LoadColors();

            isCallVoz = false;
            isCallScan = false;

            if (Device.RuntimePlatform == Device.iOS)
            {
                cLLocationManager = new CLLocationManager();
            }


            ContentHeight = App.ScreenHeight - 55 - 50 - 100;

        }
        #endregion

        #region Methods
        public void LoadColors()
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand.SearchCode == "002")//Medicity
            {
                TextColorPrice = "#80ba27"; //Verde
                TextColorName = "#0071ba";// Azul
                NameBrand = "Farmacias Medicity";
            }
            else if (mainViewModel.Brand.SearchCode == "010")//Punto Natural
            {
                TextColorPrice = "#95d600"; // Verde fosoforesente
                TextColorName = "#ff5000"; //Tomate
                NameBrand = "Punto Natural";
            }
            else //Economicas
            {
                TextColorPrice = "#595959"; //plomo 
                TextColorName = "#ed1c2e"; // rojo
                NameBrand = "Farmacias Económicas";
            }
        }
        #endregion

        #region Commands

      
        async void Voz()
        {
            try
            {
                if (!isCallVoz)
                {
                    isCallVoz = true;
                    string voice = string.Empty;
                    if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                    {
                        string systemVersion = DependencyService.Get<IDeviceSO>().GetDeviceSO();
                        string splitVersion = systemVersion.Split('.').FirstOrDefault();
                        int version = 0;
                        if (int.TryParse(splitVersion, out version))
                        {
                            if (version >= 10)
                            {

                                View contentPage = new VoiceRecognition_iOS("CategoriesViewModel");
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
                        voice = await DependencyService.Get<IVoice>().GetVoice();
                        if (!string.IsNullOrEmpty(voice))
                        {
                            var mainViewModel = MainViewModel.GetInstance();
                            mainViewModel.Products = ProductsViewModel.GetInstance();
                            mainViewModel.Products.ProductName = voice;
                            mainViewModel.Products.Products = null;
                            mainViewModel.Products.BarCode = string.Empty;
                            mainViewModel.Products.LoadProducts();
                            await navigationService.NavigateOnMaster("ProductsView");
                        }
                    }
                    isCallVoz = false;
                }
            }
            catch (Exception)
            {
                var mainViewModel = MainViewModel.GetInstance();
                await dialogService.ShowMessageBrand(
                              Resources.Resource.Error,
                              Resources.Resource.TryAgain,
                              "iconinfo",
                              mainViewModel.Brand.SearchCode);
                isCallVoz = false;
            }
        }

        bool GetCurrentPosition(bool hasInternetAccess)
        {

            if (Device.RuntimePlatform == Device.iOS)
            {
                //xdasdfas

                //poner en un a implementacion o un if pero deberia ser parte del checkLocationAvaibili
                cLLocationManager.RequestWhenInUseAuthorization();
            }

            //IGeoLocatorService geoService = DependencyService.Get<IGeoLocatorService>();
            //geoService.bestProvider();

            DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().findLocation(hasInternetAccess);

            if (GeolocatorService.Latitude != 0 && GeolocatorService.Longitude != 0)
            { return true; }
            else { return false; }

        }


        private async void SearchStock()
        {
            bool avaible = await checkLocationAvaibility();

            if (avaible)
            {
                
                var mainViewModel = MainViewModel.GetInstance();
              //  Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
            

            /*YA NO DEBE IR AQUI SINO EN EL CHECKlOCATIONaVA....
            if (locator.IsGeolocationEnabled == false)
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                bool response = await checkLocationAvaibility();
                
            }*/
                      
                    bool hasPosition = GetCurrentPosition(avaible);

                    if (hasPosition)
                    {
                        UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                        var urlAPI = Application.Current.Resources["URLAPI"].ToString();            
                        var response = await apiService.GetList<StockProduct>(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/StockProducts?codigoInterno=" + DetailProduct.InternalCode + "&idSucursal=" + mainViewModel.Brand.SearchCode + "&latitude=" + GeolocatorService.Latitude.ToString(CultureInfo.InvariantCulture) + "&longitude=" + GeolocatorService.Longitude.ToString(CultureInfo.InvariantCulture)
                            );
                        UserDialogs.Instance.HideLoading();

                    if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            UserDialogs.Instance.HideLoading();
                            return;
                        }

                        stockProduct = (List<StockProduct>)response.Result;
                    }
                    else
                    {
                        await dialogService.ShowMessage(
                        Resources.Resource.Info,
                        Resources.Resource.ErrorNoGPS);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }

                
            

                if (stockProduct.Count == 0)
                {
                    await dialogService.ShowMessage(
                        Resources.Resource.Info,
                        Resources.Resource.NoProductStock);
                    UserDialogs.Instance.HideLoading();
                    return;
                }

                UserDialogs.Instance.HideLoading();
                mainViewModel.CommercesList = new CommercesListViewModel();
                mainViewModel.CommercesList.StockProducts = stockProduct;
                mainViewModel.CommercesList.Latitude = GeolocatorService.Latitude;
                mainViewModel.CommercesList.Longitude = GeolocatorService.Longitude;
                mainViewModel.CommercesList.TextoResultado = DetailProduct.Name;
                mainViewModel.CommercesList.TypeSale = DetailProduct.TypeSale;
                await navigationService.NavigateOnMaster("CommercesListView");

            } else {
                //PONER DIAOLOGO DE ADVERTENCIA DICIENDO QUE NO SE REALIZO LA BUSQUEDA POR TENER LOS SERVICIOS DE UBICACION ABAJO, QUE INTENTE DE NUEVO
            }

        }

        private async Task<bool> checkLocationAvaibility()
        {
            //dasdasdasdasd
            //1.- verificar si }la aplicacion tiene los permisos para usar GPS, si no los tiene, return pedirlos al usuario y retorna false
            //2.- solo si ya tiene los permisos entonces verifico si tiene activados los servicios de ubicacion, si no los tiene pedir que se activen y retorna false
            //2.1 el mensaje de la activacion del GPS debe estar en un archivo de recursos
            //3.- si ya estan activos ambos retorna true;

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
                bool respuesta = await dialogService.ShowConfirm("", "Para continuar, permite que tu dispositivo active la ubicación, que se usa en el servicio de ubicación.");

                if (respuesta)
                {
                    IPermisosGPS permisoGPS = DependencyService.Get<IPermisosGPS>();
                    permisoGPS.activatePermissions();
                }

                return respuesta;
            }
        }

        public ICommand SearchStockCommand
        {
            get
            {
                return new RelayCommand(SearchStock);
            }
        }




        async void LoadProductsByBarCode(string barCode)
        {

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Products = ProductsViewModel.GetInstance();
            mainViewModel.Products.ProductName = string.Empty;
            mainViewModel.Products.BarCode = barCode;
            mainViewModel.Products.Products = null;
            mainViewModel.Products.LoadProducts();
            await navigationService.NavigateOnMaster("ProductsView");

        }

        public ICommand ScanCommand { get { return new RelayCommand(Scan); } }

        private async void Scan()
        {
            try
            {
                if (!isCallScan)
                {
                    isCallScan = true;
                    BarCode = await ScannerSKU();
                    UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                    LoadProductsByBarCode(BarCode);
                    UserDialogs.Instance.HideLoading();
                    isCallScan = false;
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                   Resources.Resource.Error,
                    Resources.Resource.TryAgain);
                UserDialogs.Instance.HideLoading();
                isCallScan = false;
            }
        }

        public async Task<string> ScannerSKU()
        {
            try
            {
                var scanner = DependencyService.Get<IQrCodeScanningService>();
                var result = await scanner.ScanAsync();
                return result.ToString();

            }
            catch (Exception ex)
            {
                ex.ToString();
                return string.Empty;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        async void Search()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Products = ProductsViewModel.GetInstance();
                mainViewModel.Products.ProductName = Filter;
                mainViewModel.Products.BarCode = string.Empty;
                mainViewModel.Products.Products = null;
                mainViewModel.Products.LoadProducts();
                await navigationService.NavigateOnMaster("ProductsView");
            }
        }




        #endregion


    }
}
