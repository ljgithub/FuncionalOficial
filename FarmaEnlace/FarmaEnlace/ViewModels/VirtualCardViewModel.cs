using Acr.UserDialogs;

using FarmaEnlace.Helpers;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.Views;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace FarmaEnlace.ViewModels
{

    public class VirtualCardViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        bool _isVisibleCode;
        bool _isVisible;
        string _code;
        string _titlePromotions;
        string _descriptionPromotions;
        string _regresiveTime;
        string _codX = "XXXXXX";
        string _lbl_message_time_code_elapsed= "";
        TimeSpan resultado;
        DateTime fecha_meta;


        private ObservableCollection<ImageBrand> _imagebrandsCollection;
        private int _currentPhoto;
        private ImageBrand _itemSelected;
        List<ImageBrand> imagebrands;

        private bool isShowCarouselSalePlus;
        #endregion

        #region Properties

        public string Message_Time_Elapsed
        {
            get
            {
                return _lbl_message_time_code_elapsed;
            }
            set
            {
                if (_lbl_message_time_code_elapsed != value)
                {
                    _lbl_message_time_code_elapsed = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Message_Time_Elapsed)));
                }
            }
        }

        public string Codx
        {
            get
            {
                return _codX;
            }
            set
            {
                if (_codX != value)
                {
                    _codX = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Codx)));
                }
            }
        }

        public bool block { get; private set; }

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

        public bool IsVisibleCode
        {
            get
            {
                return _isVisibleCode;
            }
            set
            {
                if (_isVisibleCode != value)
                {
                    _isVisibleCode = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsVisibleCode)));
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

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Code)));
                }
            }
        }

        public string RegresiveTime {

            get { return _regresiveTime; }
            set {


                if (_regresiveTime!= value)
                {
                    _regresiveTime = value;

                    PropertyChanged?.Invoke(
                       this,
                       new PropertyChangedEventArgs(nameof(RegresiveTime)));
                }
                

            }
        }



        public ObservableCollection<ImageBrand> ImagesCollection
        {
            get { return this._imagebrandsCollection; }
            set
            {
                if (_imagebrandsCollection != value)
                {
                    _imagebrandsCollection = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ImagesCollection)));
                }
            }
        }

        public int CurrentPhoto
        {
            get { return this._currentPhoto; }
            set
            {
                if (_currentPhoto != value)
                {
                    _currentPhoto = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CurrentPhoto)));
                }
            }
        }

        public ImageBrand ItemSelected
        {
            get { return this._itemSelected; }
            set
            {
                if (_itemSelected != value)
                {
                    _itemSelected = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ItemSelected)));
                }
            }
        }

        public string TitlePromotion
        {
            get { return this._titlePromotions; }
            set
            {
                if (_titlePromotions != value)
                {
                    _titlePromotions = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TitlePromotion)));
                }
            }
        }

        public string DescriptionPromotion
        {
            get { return this._descriptionPromotions; }
            set
            {
                if (_descriptionPromotions != value)
                {
                    _descriptionPromotions = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(DescriptionPromotion)));
                }
            }
        }

        private Timer timer
        {
            get;
            set;
        }

        private bool executingTimer
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public VirtualCardViewModel()
        {
            instance = this;
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            IsVisible = true;
            IsVisibleCode = false;
            IsEnabled = true;

            //para el carrusel de promociones
            ImagesCollection = new ObservableCollection<ImageBrand>();
            CurrentPhoto = 0;
            LoadImagesBrand();
            PositionCommand = new Command((Position));
            TitlePromotion = "";
            DescriptionPromotion = "";

            isShowCarouselSalePlus = false;
            executingTimer = false;

        }
        #endregion

        #region Sigleton
        static VirtualCardViewModel instance;
        public static VirtualCardViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new VirtualCardViewModel();
            }

            return instance;
        }
        #endregion


        #region Commands
        public ICommand GenerateCommand
        {
            get
            {
                return new RelayCommand(ShowCarouselSalePlus);
            }
        }

        public ICommand GenerateSalesPlusCommand
        {
            get
            {
                return new RelayCommand(Generate);
            }
        }

        async void ShowCarouselSalePlus()
        {            
            try
            {
                isShowCarouselSalePlus = false;
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                IsRunning = true;
                IsEnabled = false;
                var connection = await apiService.CheckConnection();
                if ((!connection.IsSuccess) || (_imagebrandsCollection.Count==0))
                {
                    Generate();
                }
                else{
                    isShowCarouselSalePlus = true;
                    View contePage = new CarouselSalePlus();

                    await dialogService.ShowCarouselSalePlus(contePage);
                }                              
            }
            catch (Exception ex){

            }
            finally
            {
                IsRunning = false;
                IsEnabled = true;
                UserDialogs.Instance.HideLoading();
            }            
        }

        public void VisibleCode()
        {
            IsVisible = true;
            IsVisibleCode = false;
        }

        async void Generate()
        {
            // ocultamos la venta el pop up de venta pluss si el ususario tiene este pop up
            if (isShowCarouselSalePlus)
            {
                await PopupNavigation.PopAsync();
                isShowCarouselSalePlus = false;
                
            }

            #region SaveStatisticsSalesCode
            SaveStatistics();
            #endregion            

            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                IsRunning = true;
                IsEnabled = false;
                IsVisible = false;
                IsVisibleCode = true;
                var mainViewModel = MainViewModel.GetInstance();
                var connection = await apiService.CheckConnection();

                if (!connection.IsSuccess)
                {
                    Encription encription = new Encription();
                    Code = encription.GetCode("cedula", mainViewModel.Token.UserName);
                    

                }
                else
                {
                    
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();

                    var response = await apiService.Get<Customer>(
                        urlAPI,
                        Application.Current.Resources["PrefixAPI"].ToString(),
                        "/Customers",
                        mainViewModel.Token.TokenType,
                        mainViewModel.Token.AccessToken,
                        mainViewModel.Token.UserName);
                

                    if (!response.IsSuccess)
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            response.Message);
                        return;
                    }
                    Customer customer = (Customer)response.Result;
                    Encription encription = new Encription();
                    Code = encription.GetCode(customer.TypeId, mainViewModel.Token.UserName);
                    Message_Time_Elapsed = "abc";
                    BeginTemp();
                    
                }
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                       
                      Resources.Resource.Error,Resources.Resource.TryAgain);
            }
            finally
            {

                IsRunning = false;
                IsEnabled = true;
                UserDialogs.Instance.HideLoading();
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
            timer.Stop();
            timer.Start(ImagesCollection[CurrentPhoto].DisplayTime);

            //vamos a cambiar los textos del title y description
            DescriptionPromotion = ImagesCollection[CurrentPhoto].Remarks;
            TitlePromotion = ImagesCollection[CurrentPhoto].ImageName;
        }

        public ICommand PositionCommand { get; private set; }
        

        private void Position(object obj)
        {
            var o = obj;
        }
        void Position(int value)
        {
            var v = value;
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
        #endregion

        #region Metodos
        async public void LoadImagesBrand()
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(String.Empty, MaskType.Black);

                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    
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
                }

                if (imagebrands != null && imagebrands.Count > 0)
                {
                    ImagesCollection = new ObservableCollection<ImageBrand>(imagebrands.Where(x => x.BrandId == mainViewModel.Brand.BrandId).OrderBy(o => o.DisplayOrder));
                    if (ImagesCollection.Count == 0)
                    { // a lo mejor no mostrar nada
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

        private void SaveStatistics()
        {
            var mainViewModel = MainViewModel.GetInstance();
            //consigo todas las estadisticas que hay            
            DataService dataService = new DataService();
            List<StatisticGeneral> statisticsG = dataService.Get<StatisticGeneral>(false);
            int brandId = mainViewModel.Brand.BrandId;
            List<StatisticGeneral> list = statisticsG.Where(x => x.BrandId == brandId).ToList();
            StatisticGeneral statisticGeneral = new StatisticGeneral();
            if (list.Count() > 0)
            {   //update
                statisticGeneral = list.FirstOrDefault();
                statisticGeneral.SumSalesCode++;
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
                statisticGeneral.SumPharmacys = 0;
                statisticGeneral.SumProducts = 0;
                statisticGeneral.SumSalesCode = 1;
                dataService.Insert<StatisticGeneral>(statisticGeneral);
            }
        }

         public void BeginTemp()
        {
            
             fecha_meta = DateTime.Now.AddMinutes(2);

            if (executingTimer == false)
            {
                //Si el Timer no estaba corriendo porque era nuevo o ya habia terminado, entonces creo un nuevo timer
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    executingTimer = true;
                    resultado = fecha_meta.Subtract(DateTime.Now);
                    RegresiveTime = resultado.ToString(@"mm\:ss");

                    if (resultado.ToString(@"mm\:ss").Equals("00:00"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsVisible = true;
                            IsVisibleCode = false;
                            Code = "";
                            Codx = "";

                            Message_Time_Elapsed = Resources.Resource.TimeElapsedCode;

                        });
                        executingTimer = false;
                        return false;

                    }

                    return true;
                });
            } else
            {
                //if el timer seguia en ejecucion entonces ya no lo creo sino que solo actualizo los valores reiniciando el conteo
            }
                
           
           
          /*  if(resultado.ToString() == "00:00")
            {
                IsVisible = true;
                IsVisibleCode = false;
                Code = "";
                Codx = "";
            }*/
        }
        #endregion
    }
}
