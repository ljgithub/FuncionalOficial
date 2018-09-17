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
using CoreLocation;
using FarmaEnlace.Interfaces;
using Plugin.Geolocator;

namespace FarmaEnlace.ViewModels
{

    public class DetailBrandsViewModel : BaseViewModel
    {


        #region Services
        ApiService apiService;
        DataService dataService;
        NavigationService navigationService;

        #endregion

        #region Attributes
        bool _isEnabled;
        bool _isVisibleCall;
        bool _isNotVisibleCall;
        private int _currentPhoto;
        private string _phoneNumber;
        List<ImageBrand> imagebrands;
        ImageBrand _itemSelected;
        CLLocationManager cLLocationManager;
        private ObservableCollection<ImageBrand> _imagebrandsCollection;
        DialogService dialogService;

        #endregion



        #region Properties

        public List<ImageBrand> Imagebrands { get; set; }
        
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set { SetValue(ref this._isEnabled, value); }
        }

        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { SetValue(ref this._phoneNumber, value); }
        }

        public bool IsNotVisibleCall
        {
            get { return this._isNotVisibleCall; }
            set { SetValue(ref this._isNotVisibleCall, value); }
        }

        public bool IsVisibleCall
        {
            get { return this._isVisibleCall; }
            set { SetValue(ref this._isVisibleCall, value); }
        }

        public ObservableCollection<ImageBrand> ImagesCollection
        {
            get { return this._imagebrandsCollection; }
            set { SetValue(ref this._imagebrandsCollection, value); }
        }

        public ImageBrand ItemSelected
        {
            get { return this._itemSelected; }
            set { SetValue(ref this._itemSelected, value); }
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

        #region Constructors

        public DetailBrandsViewModel()
        {
            instance = this;
            var mainViewModel = MainViewModel.GetInstance();
            apiService = new ApiService();
            dataService = new DataService();
            navigationService = new NavigationService();
            ImagesCollection = new ObservableCollection<ImageBrand>();
            dialogService = new DialogService();
            CurrentPhoto = 0;
            LoadImagesBrand();
            PositionCommand = new Command((Position));
        }



        #region Sigleton
        static DetailBrandsViewModel instance;
        public static DetailBrandsViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new DetailBrandsViewModel();
            }

            return instance;
        }
        #endregion

        private void Position(object obj)
        {
            var o = obj;
        }

        async public void LoadImagesBrand()
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);

                if (mainViewModel.Brand != null && mainViewModel.Brand.AllowCall)
                {
                    IsNotVisibleCall = false;
                    IsVisibleCall = true;
                    PhoneNumber = mainViewModel.Brand.Phone;
                }
                else
                {
                    IsNotVisibleCall = true;
                    IsVisibleCall = false;

                }
                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    imagebrands = dataService.Get<ImageBrand>(true);
                    if (imagebrands.Count == 0)
                    {
                        //await dialogService.ShowMessage(
                        //       Resources.Resource.Error,
                        //       Resources.Resource.ErrorConection);
                        //return;
                    }
                }
                else
                {
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                    var response = await apiService.GetList<ImageBrand>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/ImageBrands?idBrand=" + mainViewModel.Brand.BrandId);

                    if (!response.IsSuccess)
                    {
                        await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                        return;
                    }

                    imagebrands = (List<ImageBrand>)response.Result;
                    SaveImageBrandsOnDb();
                }

                if (imagebrands != null && imagebrands.Count > 0)
                {
                    ImagesCollection = new ObservableCollection<ImageBrand>(imagebrands.Where(x => x.BrandId == mainViewModel.Brand.BrandId).OrderBy(o => o.DisplayOrder));
                    if (ImagesCollection.Count == 0)
                    {
                        imagebrands = new List<ImageBrand>();
                        ImageBrand image = new ImageBrand();
                        image.ImageId = 1;
                        image.ImageName = string.Empty;
                        imagebrands.Add(image);
                        ImagesCollection = new ObservableCollection<ImageBrand>(imagebrands);
                    }
                    else
                    {
                        var firstOrDefault = ImagesCollection.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            if (timer != null)
                                timer.Stop();
                            timer = new Timer(OnTimerFinished);
                            timer.Start(ImagesCollection[CurrentPhoto].DisplayTime);
                        }
                    }
                }
                else
                {
                    imagebrands = new List<ImageBrand>();
                    ImageBrand image = new ImageBrand();
                    image.ImageId = 1;
                    image.ImageName = string.Empty;
                    image.Image = string.Empty;
                    imagebrands.Add(image);
                    ImagesCollection = new ObservableCollection<ImageBrand>(imagebrands);
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                  Resources.Resource.Error,
                Resources.Resource.TryAgain);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        #endregion


        #region Methods

        private void SaveImageBrandsOnDb()
        {
            foreach (var imagebrand in imagebrands)
            {
                dataService.InsertOrUpdate(imagebrand);
            }

        }

        private void SaveStatistics(int sumProducts, int sumPharmacys)
        {
            var mainViewModel = MainViewModel.GetInstance();
            //consigo todas las estadisticas que hay
            List<StatisticGeneral> statisticsG = dataService.Get<StatisticGeneral>(false);
            int brandId = mainViewModel.Brand.BrandId;
            List<StatisticGeneral> list = statisticsG.Where(x => x.BrandId == brandId).ToList();
            StatisticGeneral statisticGeneral = new StatisticGeneral();
            if (list.Count() > 0)
            {   //update
                statisticGeneral = list.FirstOrDefault();
                statisticGeneral.SumPharmacys += sumPharmacys;
                statisticGeneral.SumProducts += sumProducts;
                dataService.Update<StatisticGeneral>(statisticGeneral);
            }
            else
            {   //insert     
                if (Device.RuntimePlatform == Device.Android)
                {
                    statisticGeneral.DeviceSO = "Android";
                }
                else
                {
                    statisticGeneral.DeviceSO = "iOS";
                }
                statisticGeneral.BrandId = brandId;
                statisticGeneral.SumPharmacys = sumPharmacys;
                statisticGeneral.SumProducts = sumProducts;
                statisticGeneral.SumSalesCode = 0;
                dataService.Insert<StatisticGeneral>(statisticGeneral);
            }
        }


        #endregion

        #region Commands





        public ICommand ShareCommand
        {
            get
            {
                return new RelayCommand(Share);
            }
        }

        async void Share()
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                if (ItemSelected != null)
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Start(ImagesCollection[CurrentPhoto].DisplayTime + 10);
                    }
                    ImageBrand ima = ImagesCollection.FirstOrDefault(x => x.ImageId == ItemSelected.ImageId);
                    if (ima != null)
                    {
                        Image img = new Image()
                        {
                            Source = ima.ImageFullPath,
                            Aspect = Aspect.AspectFit
                        };
                        DependencyService.Get<FarmaEnlace.Interfaces.IShare>().Share(ima.ImageName, ima.Remarks, img.Source);
                        await Task.Delay(500);
                    }
                }
                else
                {
                    await dialogService.ShowMessage(
                       Resources.Resource.Error,
                         Resources.Resource.ErrorConection);
                }
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
            }


        }


        public ICommand LinkCommand
        {
            get
            {
                return new RelayCommand(Link);
            }
        }

        async void Link()
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);

                if (ItemSelected != null)
                {
                    timer.Stop();
                    timer.Start(ImagesCollection[CurrentPhoto].DisplayTime + 10);
                    ImageBrand ima = ImagesCollection.FirstOrDefault(x => x.ImageId == ItemSelected.ImageId);
                    if (ima != null && string.IsNullOrEmpty(ima.Url))
                    {

                        MainViewModel.GetInstance().DetailPromotion = new DetailPromotionViewModel();
                        MainViewModel.GetInstance().DetailPromotion.LoadColors();
                        MainViewModel.GetInstance().DetailPromotion.ImageBrand = ima;
                        await navigationService.NavigateOnMaster("DetailPromotionMenuView");
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
                        await CrossShare.Current.OpenBrowser(ima.Url, new Plugin.Share.Abstractions.BrowserOptions
                        {
                            UseSafariWebViewController = true,
                        });

                    }
                }
                else
                {
                    await dialogService.ShowMessage(
                     Resources.Resource.Error,
                       Resources.Resource.ErrorConection);
                }

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
            }

        }

        public ICommand PositionCommand { get; private set; }
        void Position(int value)
        {
            var v = value;
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
            timer.Stop();
            timer.Start(ImagesCollection[CurrentPhoto].DisplayTime);
        }

        void OnTimerFinished()
        {
            try
            {
                CurrentPhoto = (CurrentPhoto + 1) % ImagesCollection.Count;
                timer.Start(ImagesCollection[CurrentPhoto].DisplayTime);
            }
            catch
            {
                CurrentPhoto = 0;
                timer.Start(2);
            }

        }



        public ICommand CallCommand
        {
            get
            {
                return new RelayCommand(Call);
            }
        }

        void Call()
        {
            Xamarin.Forms.Device.OpenUri(new Uri("tel:" + PhoneNumber));
        }

        public ICommand ProductsCommand
        {
            get
            {
                return new RelayCommand(Products);
            }
        }

        async void Products()
        {
            bool available = false;
            var mainViewModel = MainViewModel.GetInstance();

            #region SaveStatisticsProducts
            SaveStatistics(1, 0);
            #endregion

            UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);          
            //available = await GeolocatorService.checkLocationAvaibility();
          
            if (available)
            {
                bool hasInternetAccess = await CheckIntenetAvaibility();
                DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().requestLocationUpdates(hasInternetAccess);
                mainViewModel.Categories = CategoriesViewModel.GetInstance();
                mainViewModel.Categories.CategoriesLineCollection = null;
                mainViewModel.Categories.LoadLineCategories();
                await navigationService.NavigateOnMaster("CategoriesLineView");
            }
            UserDialogs.Instance.HideLoading();

        }

        
        

        
        public async Task<bool> CheckIntenetAvaibility()
        {
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

        

        public ICommand CommercesCommand
        {
            get
            {
                return new RelayCommand(Commerces);
            }
        }

        async void Commerces()
        {
            var mainViewModel = MainViewModel.GetInstance();
            #region SaveStatisticsFarmacias
            SaveStatistics(0, 1);
            #endregion
            bool avaible=await GeolocatorService.checkLocationAvaibility();

            //UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);
            var connection = await apiService.CheckConnection();
            if (connection.IsSuccess)
            {
                if (avaible)
                {

                    bool hasInternetAccess = await CheckIntenetAvaibility();
                    DependencyService.Get<FarmaEnlace.Interfaces.IGeoLocatorService>().requestLocationUpdates(hasInternetAccess);

                    mainViewModel.Commerces = CommercesViewModel.GetInstance();
                    await navigationService.NavigateOnMaster("CommercesView");
                } else {
                    //TODO mostar ensaje de error de que no estaba listo el GPS, mensaje sacar e archivo de recursops
                }
            }
            else
            {
                mainViewModel.CommercesList = new CommercesListViewModel();
                mainViewModel.CommercesList.NearbyPharmacies = false;
                mainViewModel.CommercesList.Filter = string.Empty;
                mainViewModel.CommercesList.TwentyFourHours = false;
                await navigationService.NavigateOnMaster("CommercesListView");
                
            }

            //UserDialogs.Instance.HideLoading();

        }


        public async Task <bool> checkAviabilityIOS()
        {
            //verifica si la aplicacion esta lista para usar el GPS, para ello debe verificar si la opcion en el telefono esta activa y tambien verificar
            //si tiene el permiso necesario. Solo si ambas condiciones estan correctas retorno true, sino false.
            DialogService dialogService = new DialogService();
            bool isGPSActive = false;
            bool hasGPSPermissions = false;

            IPermisosGPS permisoGPS = DependencyService.Get<IPermisosGPS>();

            Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
            if (locator.IsGeolocationEnabled == false)
            {
                bool respuesta = await dialogService.ShowConfirm("", "Para continuar, permite que tu dispositivo active la ubicación, que se usa en el servicio de ubicación.");
                if (respuesta)
                    permisoGPS.requestGPSActivation();//cambiart nombre                                   

                isGPSActive = false;
            }
            else
            {
                isGPSActive = true;
            }

            //hasGPSPermissions revisar permisos para GPS
            hasGPSPermissions = permisoGPS.checkGpsPermission();

            return (isGPSActive && hasGPSPermissions);
        }


        #endregion


    }
}
