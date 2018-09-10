using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Services
        NavigationService navigationService;
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
		#endregion
		
        #region Attributes
        ObservableCollection<Product> _products;
		bool _isRefreshing;
        string _filter;
        string _descriptionCategory;
        bool _isVisibleLabel;
        string _textoResultadoColor;

        private bool isCallVoz;
        private bool isCallScan;
        #endregion

        #region Properties

        public string TextoResultadoColor
        {
            get { return this._textoResultadoColor; }
            set { SetValue(ref this._textoResultadoColor, value); }
        }

        public List<Product> ListProduct { get; set; }

        public string DescriptionCategory
        {
            get { return this._descriptionCategory; }
            set { SetValue(ref this._descriptionCategory, value); }
        }

        public ObservableCollection<Product> Products
        {
            get { return this._products; }
            set { SetValue(ref this._products, value); }
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
        public bool IsVisibleLabel
        {
            get { return this._isVisibleLabel; }
            set { SetValue(ref this._isVisibleLabel, value); }
        }

        

        public string ProductName { get; set; }
        public string BarCode { get; set; }
        public string IdCategory { get; set; }
        
        public bool IsSaveDBLocal { get; set; }
        #endregion

        #region Constructor
        public ProductsViewModel()
        {
            instance = this;
			apiService = new ApiService();
			dialogService = new DialogService();
            dataService = new DataService();
            navigationService = new NavigationService();
            SelectProductCommand = new Command<string>(SelectProduct);
            Filter = string.Empty;
            IsVisibleLabel = false;
            IdCategory = "0";
            LoadColors();

            isCallVoz = false;
            isCallScan = false;

            IsSaveDBLocal = false;
        }
        
        public void Load() {

            Filter = string.Empty;
            LoadColors();
            Products = new ObservableCollection<Product>(
                ListProduct.OrderBy(p => p.Name));
        }
        async public void LoadProducts()
        {
            IsVisibleLabel = false;
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                LoadColors();
                ListProduct = null;
                Products = null;
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);

                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(BarCode))
                    {
                        ListProduct = dataService.Get<Product>(true).Where(x => x.Name.ToUpperInvariant().Contains(ProductName.ToUpperInvariant()) && x.BrandId == mainViewModel.Brand.BrandId).ToList();
                        DescriptionCategory = string.Format(Resources.Resource.ResultSearch, ProductName.ToUpperInvariant());
                    }
                    else if (string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(BarCode))
                    {
                        ListProduct = dataService.Get<Product>(true).Where(x => x.Barcode == BarCode && x.BrandId == mainViewModel.Brand.BrandId).ToList();
                        DescriptionCategory = string.Format(Resources.Resource.ResultSearch, BarCode.ToUpperInvariant());
                    }
                    else if (string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(BarCode) && !string.IsNullOrEmpty(IdCategory))
                    {
                        string[] strCategories = IdCategory.Split('|');
                        int cont = strCategories.Count();
                        ListProduct = dataService.Get<Product>(true).Where(x => x.CategoryId == Convert.ToInt32(strCategories[cont - 1]) && x.BrandId == mainViewModel.Brand.BrandId).ToList();
                    }
                    if (ListProduct.Count == 0)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Info,
                            Resources.Resource.NoInformation);
                        IsVisibleLabel = true;
                        return;
                    }
                }
                else
                {
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();

                    if (!string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(BarCode))
                    {
                        var response = await apiService.GetList<Product>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/Products?name=" + Helpers.Common.RemoveAccentsWithNormalization(ProductName) + "&searchCode=" + mainViewModel.Brand.SearchCode
                        );

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            return;
                        }

                        ListProduct = (List<Product>)response.Result;
                        DescriptionCategory = string.Format(Resources.Resource.ResultSearch, ProductName.ToUpperInvariant());
                    }
                    else if (string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(BarCode))
                    {
                        var response = await apiService.GetList<Product>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/Products?barCode=" + BarCode + "&searchCode=" + mainViewModel.Brand.SearchCode
                        );

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            return;
                        }

                        ListProduct = (List<Product>)response.Result;
                        DescriptionCategory = string.Format(Resources.Resource.ResultSearch, BarCode.ToUpperInvariant());

                    }
                    else if (string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(BarCode) && !string.IsNullOrEmpty(IdCategory))
                    {
                        var response = await apiService.GetList<Product>(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/Products?idCategory=" + IdCategory + "&searchCode=" + mainViewModel.Brand.SearchCode);

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            return;
                        }
                        ListProduct = (List<Product>)response.Result;
                    }

                    if (ListProduct == null || ListProduct.Count == 0)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Info,
                            Resources.Resource.NoInformation);
                        IsVisibleLabel = true;
                        return;
                    }

                    //System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(SaveProductsOnDB));
                    //myThread.Start();

                    //TODO LROCHA comentado temporalmente hasta probar estabilidad sin grabar en la bdd SaveProductsOnDB();
                }
                Products = new ObservableCollection<Product>(ListProduct.OrderBy(p => p.Name));
            }
            catch (Exception e)
            {

                await dialogService.ShowMessageBrand(
                             Resources.Resource.Error,
                             Resources.Resource.TryAgain,
                             "iconinfo",
                             mainViewModel.Brand.SearchCode);
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Filter = string.Empty;
                UserDialogs.Instance.HideLoading();
            }

        }
        #endregion

        #region Sigleton
        static ProductsViewModel instance;


        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }

        #endregion

        #region Methods
        public void LoadColors()
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand.SearchCode == "002")//Medicity
            {
                TextoResultadoColor = "#0071ba";
            }
            else if (mainViewModel.Brand.SearchCode == "010")//Punto Natural
            {
                TextoResultadoColor = "#ff4f00";
            }
            else //Economicas
            {
                TextoResultadoColor = "#ed1c2e";
            }
        }

        /// <summary>
        /// Metodo que guarda las estadisticas de Productos
        /// </summary>
        /// <param name="firstOrDefault"></param>
        private void SaveStatistic(Product firstOrDefault)
        {
            if (firstOrDefault != null)
            {
                var mainViewModel = MainViewModel.GetInstance();
                //consigo todas las estadisticas que hay
                List<StatisticProduct> listaProductos = dataService.Get<StatisticProduct>(false);
                int brandId = mainViewModel.Brand.BrandId;
                List<StatisticProduct> list = listaProductos.Where(x => x.BrandId == brandId && x.CategoryId == firstOrDefault.CategoryId && x.ProductId == firstOrDefault.ProductId).ToList();
                StatisticProduct product = new StatisticProduct();
                if (list.Count() > 0)
                {
                    //update
                    product = list.FirstOrDefault();
                    product.SumProduct++;
                    dataService.Update<StatisticProduct>(product);
                }
                else
                {
                    //insert
                    product.BrandId = brandId;
                    product.CategoryId = firstOrDefault.CategoryId;
                    product.Name = firstOrDefault.Name;
                    product.ProductId = firstOrDefault.ProductId;
                    product.SumProduct = 1;
                    //TODO revisar porque se cae el insert, si en las estadisticas se supone que solo es una acumulacion de accesos y deberia poder repetir el productoId
                    //dataService.Insert<StatisticProduct>(product);
                    dataService.Update<StatisticProduct>(product);
                }
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
                                //voice = await DependencyService.Get<IVoice>().GetVoice();
                                //if (!string.IsNullOrEmpty(Filter))
                                //{
                                //    ProductName = voice;
                                //    BarCode = string.Empty;
                                //}
                                View contentPage = new VoiceRecognition_iOS("ProductsViewModel");
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
                            ProductName = voice;
                            BarCode = string.Empty;
                            LoadProducts();
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

        public ICommand SelectProductCommand { get; private set; }

        async void SelectProduct(string internalCode)
        {
            IsRefreshing = true;
            Filter = string.Empty;
            Product firstOrDefault = ListProduct.FirstOrDefault(x => x.InternalCode == internalCode);
            #region Statistics
            SaveStatistic(firstOrDefault);
            #endregion
            if (firstOrDefault != null)
            {
                MainViewModel.GetInstance().DetailProduct = new DetailProductViewModel(firstOrDefault);
                MainViewModel.GetInstance().Product = firstOrDefault;
                
                await navigationService.NavigateOnMaster("DetailProductView");
            }
            IsRefreshing = false;
        }
        
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        void Search()
        {
            IsRefreshing = true;
            if (!string.IsNullOrEmpty(Filter))
            {
                ProductName = Filter;
                BarCode = string.Empty;
                LoadProducts();
            }
            IsRefreshing = false;
        }

        public void SaveProductsOnDB()
        {
            var mainViewModel = MainViewModel.GetInstance();
            string[] strCategories = IdCategory.Split('|');
            int cont =  strCategories.Count();
            foreach (var products in ListProduct)
            {
                products.BrandId = mainViewModel.Brand.BrandId;
                if (string.IsNullOrEmpty(IdCategory))
                {
                    IdCategory = "0";
                    products.CategoryId = 0;
                }
                else
                {
                    products.CategoryId = Convert.ToInt32(strCategories[cont - 1]);
                }
                dataService.InsertOrUpdate(products);
            }
        }

        public ICommand ReturnCategoryCommand
        {
            get
            {
                return new RelayCommand(ReturnCategory);
            }
        }

        async void ReturnCategory()
        {
            IsRefreshing = true;
            await navigationService.BackOn();
            IsRefreshing = false;
        }

        async void LoadProductsByBarCode(string barCode)
        {
            ListProduct.Clear();
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    await dialogService.ShowMessage(
                            Resources.Resource.Info,
                            Resources.Resource.NoInformation);
                    return;
                }
                else
                {
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                    var response = await apiService.GetList<Product>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/Products?barCode=" + barCode+ "&searchCode=" + mainViewModel.Brand.SearchCode
                        );

                    if (!response.IsSuccess)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            response.Message);
                        return;
                    }

                    ListProduct = (List<Product>)response.Result;
                    //TODO LROCHA PORUQE NO ESTABA GRABANDO EN LA BDD cuando la busqueda del producto era por codigo de barras?????
                    //SaveProductsOnDB();
                    IsSaveDBLocal = true;
                }

                if (ListProduct.Count == 0)
                {
                    await dialogService.ShowMessage(
                            Resources.Resource.Info,
                            Resources.Resource.NoInformation);
                    return;
                }

                mainViewModel.Products.DescriptionCategory = string.Format("RESULTADO: ");
                IdCategory = string.Empty;
                ProductName = string.Empty;
                LoadProducts();
                //await navigationService.NavigateOnMaster("ProductsView");
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {

                await dialogService.ShowMessageBrand(
                            Resources.Resource.Error,
                            Resources.Resource.TryAgain,
                            "iconinfo",
                            mainViewModel.Brand.SearchCode);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                barCode = string.Empty;
            }
           
        }

        public ICommand ScanCommand { get { return new RelayCommand(Scan); } }

        private async void Scan()
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                if (!isCallScan)
                {
                    isCallScan = true;
                    BarCode = await ScannerSKU();
                    ProductName = string.Empty;
                    IdCategory = string.Empty;
                    LoadProducts();
                    isCallScan = false;
                }
            }
            catch (Exception)
            {

                await dialogService.ShowMessageBrand(
                           Resources.Resource.Error,
                           Resources.Resource.TryAgain,
                           "iconinfo",
                           mainViewModel.Brand.SearchCode);
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

        #endregion

    }
}
