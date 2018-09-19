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
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class CategoriesSub3ViewModel : Category, INotifyPropertyChanged
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

        public CategoriesSub3ViewModel()
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
        static CategoriesSub3ViewModel instance;
        public static CategoriesSub3ViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new CategoriesSub3ViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
      



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
                     Resources.Resource.ErrorMessage);
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



        void SaveCategoriesOnDB()
        {
            foreach (var category in categories.Take(20))
            {
                dataService.InsertOrUpdate(category);
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
                     Resources.Resource.ErrorMessage);
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
                     Resources.Resource.ErrorMessage);
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
                    mainViewModel.CategoriesSub4 = CategoriesSub4ViewModel.GetInstance();
                    mainViewModel.CategoriesSub4.CurrentCategory = firstOrDefault;
                    mainViewModel.CategoriesSub4.NameLine = firstOrDefault.Name;
                    mainViewModel.CategoriesSub4.ImageLineParent = ImageLineParent;
                    mainViewModel.CategoriesSub4.cat = cat + "|" + firstOrDefault.FarmaEnlaceId.ToString();
                    mainViewModel.CategoriesSub4.LoadCategories();
                    await navigationService.NavigateOnMaster("CategoriesSub4View");
                }
            }
            catch (Exception)
            {

                await dialogService.ShowMessage(
                      Resources.Resource.Error,
                       Resources.Resource.ErrorMessage);
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
                mainViewModel.Products.DescriptionCategory = string.Format("{0} - {1}", NameLine, categories.FirstOrDefault(x => x.FarmaEnlaceId == idCategory).Name);
                mainViewModel.Products.IdCategory = cat + "|" + categories.FirstOrDefault(x => x.FarmaEnlaceId == idCategory).FarmaEnlaceId;
                mainViewModel.Products.LoadProducts();
                await navigationService.NavigateOnMaster("ProductsView");
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                     Resources.Resource.ErrorMessage);
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
                     Resources.Resource.ErrorMessage);
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
                    Resources.Resource.ErrorMessage);
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
