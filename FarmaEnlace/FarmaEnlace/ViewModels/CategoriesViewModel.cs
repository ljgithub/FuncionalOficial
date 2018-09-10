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
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class CategoriesViewModel : Category, INotifyPropertyChanged 
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        NavigationService navigationService;
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Attributes
        List<Category> categories;
        List<Product> products;
        string _descriptionCategory;
        bool _gridIsVisible;
        Category _categorySelected;
        ObservableCollection<Category> _categories;
        ObservableCollection<Category> _categoriesLine;
        ObservableCollection<Category> _categoriesFilter;
        bool _isRefreshing;
        bool _isEnabled;
        bool _isVisible;
        string _filter;
        object _lastTappedItem;
        string _imageLineParent;
        private string barCode;
        
        private bool isCallScan;
        private bool isCallVoz;
        #endregion

        #region Properties
        public string cat { get; set; }

        public string PhoneNumber { get; set; }

      
        public string ImageLineParent
        {
            get
            {
                return _imageLineParent;
            }
            set
            {
                if (_imageLineParent != value)
                {
                    _imageLineParent = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ImageLineParent)));
                }
            }
        }

        public string BarCode
        {
            set
            {
                if (barCode != value)
                {
                    barCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BarCode"));
                }
            }
            get
            {
                return barCode;
            }
        }

        public object LastTappedItem
        {
            set
            {
                if (_lastTappedItem != value)
                {
                    _lastTappedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastTappedItem"));
                }
            }
            get
            {
                return barCode;
            }
        }



        public bool GridIsVisible
        {
            get
            {
                return _gridIsVisible;
            }
            set
            {
                if (_gridIsVisible != value)
                {
                    _gridIsVisible = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(GridIsVisible)));
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsVisible)));
                }
            }
        }

        public string DescriptionCategory
        {
            get
            {
                return _descriptionCategory;
            }
            set
            {
                if (_descriptionCategory != value)
                {
                    _descriptionCategory = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(DescriptionCategory)));
                }
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }

        

        public ObservableCollection<Category> CategoriesCollection
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategoriesCollection)));
                }
            }
        }

        public ObservableCollection<Category> CategoriesLineCollection
        {
            get
            {
                return _categoriesLine;
            }
            set
            {
                if (_categoriesLine != value)
                {
                    _categoriesLine = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategoriesLineCollection)));
                }
            }
        }

        public Category CategorySelected
        {
            get
            {
                return _categorySelected;
            }
            set
            {
                if (_categorySelected != value)
                {
                    _categorySelected = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategorySelected)));
                }
            }
        }

        public ObservableCollection<Category> CategoriesFilter
        {
            get
            {
                return _categoriesFilter;
            }
            set
            {
                if (_categoriesFilter != value)
                {
                    _categoriesFilter = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategoriesFilter)));
                }
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        public Category PreviousCategory { get; set; }
        public Category CurrentCategory { get; set; }

        public string NameLine { get; set; }
        
        #endregion

        #region Constructors

        public CategoriesViewModel()
        {
            instance = this;
            navigationService = new NavigationService();
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            SelectCategoryCommand = new Command<int>(SelectCategory);
            GridIsVisible = false;

            isCallScan = false;
            isCallVoz = false;
        }


        #endregion

        #region Sigleton
        static CategoriesViewModel instance;
        public static CategoriesViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new CategoriesViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods

       


        async public void LoadLineCategories()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                CategoriesCollection = new ObservableCollection<Category>();
                Filter = string.Empty;
                var mainViewModel = MainViewModel.GetInstance();
                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    categories = dataService.Get<Category>(true).Where(x => x.BrandId == mainViewModel.Brand.BrandId && x.ParentId == null && x.IsActive == true).ToList();
                    if (categories.Count == 0)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Info,
                            Resources.Resource.NoInformation);
                        IsRefreshing = false;
                        IsEnabled = true;
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                }
                else
                {
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                    var response = await apiService.GetList<Category>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/Categories?idBrand=" + mainViewModel.Brand.BrandId);

                    if (!response.IsSuccess)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            response.Message);
                        IsRefreshing = false;
                        IsEnabled = true;
                        UserDialogs.Instance.HideLoading();
                        return;
                    }

                    categories = (List<Category>)response.Result;
                    SaveCategoriesOnDB();
                }
                LoadLineCategory(categories);
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                  Resources.Resource.Error,
                   "Estimado usuario, por favor, reintente su busqueda");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
           
        }

        async public void LoadCategories()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                CategoriesCollection = new ObservableCollection<Category>();
                Filter = string.Empty;
                if (!string.IsNullOrEmpty(CurrentCategory.Name))
                    DescriptionCategory = CurrentCategory.Name;
                var mainViewModel = MainViewModel.GetInstance();
                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    categories = dataService.Get<Category>(true).Where(x => x.ParentId == CurrentCategory.FarmaEnlaceId && x.IsActive == true).ToList();
                    if (categories.Count == 0)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Info,
                            Resources.Resource.NoInformation);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                }
                else
                {
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                    var response = await apiService.GetList<Category>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/Categories/?idCategory=" + CurrentCategory.FarmaEnlaceId +
                        "&idBrand=" + mainViewModel.Brand.BrandId);

                    if (!response.IsSuccess)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            response.Message);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }

                    categories = (List<Category>)response.Result;
                    SaveCategoriesOnDB();
                }
                LoadCategory(categories);
                

            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     "Estimado usuario, por favor, reintente su busqueda");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            
        }

        void LoadCategory(List<Category> categories)
        {
            var list = categories.Select(c => new Category
            {
                BrandId = c.BrandId,
                CategoryId = c.CategoryId,
                FarmaEnlaceId = c.FarmaEnlaceId,
                Image = c.Image,
                ImageLine = c.ImageLine,
                IsActive = c.IsActive,
                Name = c.Name,
                Order = c.Order,
                ParentId = c.ParentId,
                SearchCode = c.SearchCode,
                SearchProduct = c.SearchProduct
            }).ToList();

            CategoriesCollection = new ObservableCollection<Category>(list);
        }

        void LoadLineCategory(List<Category> categories)
        {
            var list = categories.Select(c => new Category
            {
                BrandId = c.BrandId,
                CategoryId = c.CategoryId,
                FarmaEnlaceId = c.FarmaEnlaceId,
                Image = c.Image,
                ImageLine = c.ImageLine,
                IsActive = c.IsActive,
                Name = c.Name,
                Order = c.Order,
                ParentId = c.ParentId,
                SearchCode = c.SearchCode,
                SearchProduct = c.SearchProduct
            }).ToList();

            CategoriesLineCollection = new ObservableCollection<Category>(list);
        }

        void SaveCategoriesOnDB()
        {
            foreach (var category in categories.Take(20))
            {
                dataService.InsertOrUpdate(category);
            }
        }
        

        public async void SearhProduct(string product)
        {
      
            if (!string.IsNullOrEmpty(product))
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Products = ProductsViewModel.GetInstance();
                mainViewModel.Products.ProductName = product;
                mainViewModel.Products.Products = null;
                mainViewModel.Products.BarCode = string.Empty;
                mainViewModel.Products.LoadProducts();
                await navigationService.NavigateOnMaster("ProductsView");
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
            
            string voice = string.Empty;
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
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     "Estimado usuario, por favor, reintente su busqueda");
                isCallVoz = false;
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
            try
            {
                if (!string.IsNullOrEmpty(Filter))
                {
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Products = ProductsViewModel.GetInstance();
                    mainViewModel.Products.ProductName = Filter;
                    mainViewModel.Products.Products = null;
                    mainViewModel.Products.BarCode = string.Empty;
                    mainViewModel.Products.LoadProducts();
                    Filter = string.Empty;
                    await navigationService.NavigateOnMaster("ProductsView");
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     "Estimado usuario, por favor, reintente su busqueda");
            }

        }

        public ICommand SelectCategoryLineCommand
        {
            get
            {
                return new RelayCommand(SelectCategoryLine);
            }
        }

        async void SelectCategoryLine()
        {
            try
            {
                Category firstOrDefault = categories.FirstOrDefault(x => x.FarmaEnlaceId == CategorySelected.FarmaEnlaceId);

                #region Statistics
                SaveStatistic(firstOrDefault);
                #endregion

                if (firstOrDefault != null && firstOrDefault.SearchProduct)
                {
                    LoadProducts(CategorySelected.FarmaEnlaceId);
                }
                else
                {
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.CategoriesSub1 = CategoriesSub1ViewModel.GetInstance();
                    mainViewModel.CategoriesSub1.CurrentCategory = CategorySelected;
                    mainViewModel.CategoriesSub1.NameLine = CategorySelected.Name;
                    mainViewModel.CategoriesSub1.ImageLineParent = CategorySelected.ImageLineFullPath;
                    mainViewModel.CategoriesSub1.cat = firstOrDefault.FarmaEnlaceId.ToString();
                    mainViewModel.CategoriesSub1.LoadCategories();
                    await navigationService.NavigateOnMaster("CategoriesSub1View");
                }
            }
            catch (Exception e)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     "Estimado usuario, por favor, reintente su busqueda");
            }


        }

        /// <summary>
        /// Metodo que guarda en la base de datos local las estadisticas de Categorias visitadas
        /// </summary>
        /// <param name="firstOrDefault"></param>
        private void SaveStatistic(Category firstOrDefault)
        {
            if(firstOrDefault != null)
            {
                var mainViewModel = MainViewModel.GetInstance();
                //consigo todas las estadisticas que hay
                List<StatisticCategory> statistics = dataService.Get<StatisticCategory>(false);
                int brandId = mainViewModel.Brand.BrandId;
                List<StatisticCategory> list = statistics.Where(x => x.BrandId == brandId  && x.CategoryId == firstOrDefault.CategoryId).ToList();
                StatisticCategory category = new StatisticCategory();
                if (list.Count() > 0)
                {
                    //update
                    category = list.FirstOrDefault();
                    category.SumCategory++;
                    dataService.Update<StatisticCategory>(category);
                }
                else
                {
                    //insert
                    category.BrandId = brandId;
                    category.CategoryId = firstOrDefault.CategoryId;
                    category.FarmaEnlaceId = firstOrDefault.FarmaEnlaceId;
                    category.Name = firstOrDefault.Name;
                    category.ParentId = firstOrDefault.ParentId;
                    category.SumCategory = 1;
                    dataService.Insert<StatisticCategory>(category);
                }
            }
        }

        public ICommand SelectCategoryCommand { get; private set; }

        async void SelectCategory(int FarmaEnlaceId)
        {
            try
            {
                Category firstOrDefault = categories.FirstOrDefault(x => x.FarmaEnlaceId == FarmaEnlaceId);
                if (firstOrDefault != null && firstOrDefault.SearchProduct)
                {
                    LoadProducts(FarmaEnlaceId);
                }
                else
                {
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Categories.CurrentCategory = firstOrDefault;
                    mainViewModel.Categories.NameLine = firstOrDefault.Name;
                    mainViewModel.Categories.ImageLineParent = ImageLineParent;
                    mainViewModel.Categories.cat = cat + "|" + firstOrDefault.SearchCode;
                    mainViewModel.Categories.LoadCategories();
                    await navigationService.NavigateOnMaster("CategoriesView");

                }
            }
            catch (Exception)
            {

                await dialogService.ShowMessage(
                      Resources.Resource.Error,
                       "Estimado usuario, por favor, reintente su busqueda");
                UserDialogs.Instance.HideLoading();
                return;
            }
            
            
            
        }

        public ICommand ReturnCategoryCommand
        {
            get
            {
                return new RelayCommand(ReturnCategory);
            }
        }

        void ReturnCategory()
        {
            GridIsVisible = false;
            CategorySelected.CategoryId = (int)PreviousCategory.ParentId;
            LoadCategories();
        }


        async void LoadProducts(int idCategory)
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Products = ProductsViewModel.GetInstance();
                mainViewModel.Products.BarCode = string.Empty;
                mainViewModel.Products.ProductName = string.Empty;
                if(string.IsNullOrEmpty(NameLine))
                    mainViewModel.Products.DescriptionCategory = categories.FirstOrDefault(x => x.FarmaEnlaceId == idCategory).Name;
                else
                    mainViewModel.Products.DescriptionCategory = string.Format("{0} - {1}", NameLine, categories.FirstOrDefault(x => x.FarmaEnlaceId == idCategory).Name);
                if(string.IsNullOrEmpty(cat))
                    mainViewModel.Products.IdCategory = categories.FirstOrDefault(x => x.FarmaEnlaceId == idCategory).FarmaEnlaceId.ToString();
                else
                    mainViewModel.Products.IdCategory = cat + "|" + categories.FirstOrDefault(x => x.FarmaEnlaceId == idCategory).FarmaEnlaceId;
                mainViewModel.Products.LoadProducts();
                await navigationService.NavigateOnMaster("ProductsView");
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     "Estimado usuario, por favor, reintente su busqueda");
                UserDialogs.Instance.HideLoading();
            }
        }


        async void LoadProductsByBarCode(string barCode)
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Products = ProductsViewModel.GetInstance();
                mainViewModel.Products.ProductName = string.Empty;
                mainViewModel.Products.BarCode = barCode;
                mainViewModel.Products.Products = null;
                mainViewModel.Products.LoadProducts();
                await navigationService.NavigateOnMaster("ProductsView");
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     "Estimado usuario, por favor, reintente su busqueda");
                UserDialogs.Instance.HideLoading();
            }
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
                    "Estimado usuario, por favor, reintente su busqueda");
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

      


        #endregion
    }
}
