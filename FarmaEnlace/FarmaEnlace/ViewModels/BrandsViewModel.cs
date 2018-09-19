using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Share;
using Xamarin.Forms;
using FarmaEnlace.Helpers;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace FarmaEnlace.ViewModels
{

    public class BrandsViewModel : BaseViewModel
    {
        
        #region Services
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationService;
        List<Brand> brands;
        List<ImageBrand> imagebrands;
        #endregion

        #region Attributes
        bool _isRefreshing;
        bool _isEnabled;
        private int _currentPhoto;
        string _imageMedicity;
        string _imageLogin;
        string _imageEconomica;
        string _imagePuntoNatural;
        private ObservableCollection<BrandItemViewModel> _brandsCollection;
        private ObservableCollection<ImageBrand> _imagebrandsCollection;
        #endregion


        #region Properties
        public string ImageLogin
        {
            get { return this._imageLogin; }
            set { SetValue(ref this._imageLogin, value); }
        }

        public string ImageMedicity
        {
            get { return this._imageMedicity; }
            set { SetValue(ref this._imageMedicity, value); }
        }
        public string ImageEconomica
        {
            get { return this._imageEconomica; }
            set { SetValue(ref this._imageEconomica, value); }
        }

        public string ImagePuntoNatural
        {
            get { return this._imagePuntoNatural; }
            set { SetValue(ref this._imagePuntoNatural, value); }
        }


        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set { SetValue(ref this._isEnabled, value); }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<BrandItemViewModel> BrandsCollection
        {
            get { return this._brandsCollection; }
            set { SetValue(ref this._brandsCollection, value); }
        }
        public ObservableCollection<ImageBrand> ImageBrandsCollection
        {
            get { return this._imagebrandsCollection; }
            set { SetValue(ref this._imagebrandsCollection, value); }
        }

        public int CurrentPhoto
        {
            get { return this._currentPhoto; }
            set { SetValue(ref this._currentPhoto, value); }
        }

        private Timer timer
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public BrandsViewModel()
        {
            instance = this;
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            ShareCommand = new Command<int>(Share);
            LinkCommand = new Command<int>(Link);
            ImageBrandsCollection = new ObservableCollection<ImageBrand>();
            CurrentPhoto = 0;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.IsVisibleMyMenu = true;
            mainViewModel.IsVisibleMyMenuUser = false;
        }

        #region Sigleton
        static BrandsViewModel instance;
        public static BrandsViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new BrandsViewModel();
            }

            return instance;
        }
        #endregion


        

        async public void LoadImagesBrand()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                if (brands == null || brands.Count == 0)
                {
                    var connection = await apiService.CheckConnection();
                    if (!connection.IsSuccess)
                    {
                        ImageMedicity = "btnmedicity.png";
                        ImageEconomica = "btneconomicas.png";
                        ImagePuntoNatural = "btnpuntonatural.png";
                        brands = dataService.Get<Brand>(true);
                        if (brands.Count == 0)
                        {
                            brands = new List<Brand>();
                            Brand medicity = new Brand();
                            medicity.AllowCall = false;
                            medicity.BrandId = 3;
                            medicity.Color = Application.Current.Resources["Medicity"].ToString();
                            medicity.TextColor = Application.Current.Resources["TextMedicity"].ToString();
                            medicity.SearchCode = "002";
                            brands.Add(medicity);
                            Brand economicas = new Brand();
                            economicas.AllowCall = false;
                            economicas.BrandId = 2;
                            economicas.Color = Application.Current.Resources["Economicas"].ToString();
                            economicas.TextColor = Application.Current.Resources["TextEconomicas"].ToString();
                            economicas.SearchCode = "003";
                            brands.Add(economicas);
                            Brand puntoNatural = new Brand();
                            puntoNatural.AllowCall = false;
                            puntoNatural.BrandId = 4;
                            puntoNatural.Color = Application.Current.Resources["PuntoNatural"].ToString();
                            puntoNatural.TextColor = Application.Current.Resources["TextPuntoNatural"].ToString();
                            puntoNatural.SearchCode = "010";
                            brands.Add(puntoNatural);
                        }
                    }
                    else
                    {
                        var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                        var response = await apiService.GetList<Brand>(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/Brands");

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                Resources.Resource.ErrorMessage);
                            return;
                        }
                        brands = (List<Brand>)response.Result;
                        ImageMedicity = brands.Find(x => x.BrandId == 3).ImageFullPath;
                        ImageEconomica = brands.Find(x => x.BrandId == 2).ImageFullPath;
                        ImagePuntoNatural = brands.Find(x => x.BrandId == 4).ImageFullPath;
                       
                        //TODO descomentar cuando se tenga la forma de trabajr offline sin afectar el rendimiento
                        //SaveBrandsOnDB();
                    }
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                     Resources.Resource.Error,
                      Resources.Resource.ErrorMessage);
                return;
            }
            finally
            {
                ImageLogin = "btntarjetavirtual.png";
                UserDialogs.Instance.HideLoading();
            }
            
        }

        #endregion

        #region Methods
        void SaveBrandsOnDB()
        {
            dataService.DeleteAll<Brand>();
            foreach (var brand in brands)
            {
                dataService.Insert(brand);
            }
        }
        private IEnumerable<BrandItemViewModel> ToLandItemViewModel()
        {
            return brands.Select(l => new BrandItemViewModel
            {
                Address = l.Address,
                AllowCall = l.AllowCall,
                BrandId = l.BrandId,
                Color = l.Color,
                Description = l.Description,
                Email = l.Email,
                ImageButton = l.ImageButton,
                Name = l.Name,
                Phone = l.Phone,
                TextColor = l.TextColor,
                SearchCode = l.SearchCode
            });
        }


        #endregion

        #region Commands
        public ICommand RedirectionLoginCommand
        {
            get
            {
                return new RelayCommand(RedirectionLogin);
            }
        }

        async void RedirectionLogin()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                await Task.Delay(500);
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.IsVisibleMyMenu = false;
                mainViewModel.IsVisibleMyMenuUser = true;
                mainViewModel.Brand = brands.Find(x => x.BrandId == 1);
                var token = dataService.First<TokenResponse>(false);
                if (token != null &&
                    token.IsRemembered &&
                    token.Expires > DateTime.Now)
                {
                    mainViewModel.Token = token;
                    MainViewModel.GetInstance().VirtualCard = new VirtualCardViewModel();
                    await navigationService.SetMainPage("MasterView");
                }
                else
                {
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    await navigationService.SetMainPage("LoginView");
                }
                UserDialogs.Instance.HideLoading();
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

        public ICommand RedirectionPuntoNaturalCommand
        {
            get
            {
                return new RelayCommand(RedirectionPuntoNatural);
            }
        }

        public async void RedirectionPuntoNatural()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                await Task.Delay(500);
                UserDialogs.Instance.HideLoading();
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Brand = brands.Find(x => x.BrandId == 4);
                mainViewModel.IsVisibleMyMenu = true;
                mainViewModel.IsVisibleMyMenuUser = false;
                mainViewModel.DetailBrands = new DetailBrandsViewModel();
                await navigationService.SetMainPage("DetailBrandsView");
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
            }
            
        }

        public ICommand RedirectionEconomicasCommand
        {
            get
            {
                return new RelayCommand(RedirectionEconomicas);
            }
        }

        public async void RedirectionEconomicas()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                await Task.Delay(500);
                UserDialogs.Instance.HideLoading();
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Brand = brands.Find(x => x.BrandId == 2);
                mainViewModel.IsVisibleMyMenu = true;
                mainViewModel.IsVisibleMyMenuUser = false;
                mainViewModel.DetailBrands = new DetailBrandsViewModel();
                await navigationService.SetMainPage("DetailBrandsView");
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
            }
           

        }


        public ICommand RedirectionMedicityCommand
        {
            get
            {
                return new RelayCommand(RedirectionMedicity);
            }
        }

        async void RedirectionMedicity()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                await Task.Delay(500);
                UserDialogs.Instance.HideLoading();
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Brand = brands.Find(x => x.BrandId == 3);
                mainViewModel.IsVisibleMyMenu = true;
                mainViewModel.IsVisibleMyMenuUser = false;
                mainViewModel.DetailBrands = new DetailBrandsViewModel();
                await navigationService.SetMainPage("DetailBrandsView");
            }
            catch (Exception e)
            {
                //TODO JAVIER MOSTAR MENSAJE DE ERROR E IMPRIMIR EN CONSOLA
                UserDialogs.Instance.HideLoading();
            }
            
           
        }


        public ICommand ShareCommand { get; private set; }

        async private void Share(int value)
        {
            timer.Stop();
            timer.Start(ImageBrandsCollection[CurrentPhoto].DisplayTime + 10);
            ImageBrand ima = ImageBrandsCollection.FirstOrDefault(x => x.ImageId == value);
            if (ima != null)
            {
                Image img = new Image()
                {
                    Source = ima.ImageFullPath,
                    Aspect = Aspect.AspectFit
                };
                DependencyService.Get<FarmaEnlace.Interfaces.IShare>().Share(ima.ImageName,ima.Remarks, img.Source);
            }
        }

        public ICommand LinkCommand { get; private set; }

        async void Link(int value)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                if (value == 0) return;

                ImageBrand ima = ImageBrandsCollection.FirstOrDefault(x => x.ImageId == value);
                if (string.IsNullOrEmpty(ima.Url))
                {
                    timer.Stop();
                    MainViewModel.GetInstance().ImageBrand = ima;
                    await navigationService.SetMainPage("DetailPromotionView");
                }
                else
                    await CrossShare.Current.OpenBrowser(ima.Url, new Plugin.Share.Abstractions.BrowserOptions
                    {
                        UseSafariWebViewController = true,
                    });
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                   Resources.Resource.Error,
                    Resources.Resource.ErrorMessage);
            }
            
        }
        public ICommand CurrentPhotoChangedCommand
        {
            get
            {
                return new RelayCommand(CurrentPhotoChanged);
            }
        }

        void CurrentPhotoChanged()
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Start(ImageBrandsCollection[CurrentPhoto].DisplayTime);
                }
            }
            catch {}
        }

        void OnTimerFinished()
        {
            if (timer != null)
            {
                try
                {
                    CurrentPhoto = (CurrentPhoto + 1) % ImageBrandsCollection.Count;
                    timer.Start(ImageBrandsCollection[CurrentPhoto].DisplayTime);
                }
                catch 
                {
                    CurrentPhoto = 0;
                    timer.Start(2);
                }

            }
            
        }


        #endregion

    }
}
