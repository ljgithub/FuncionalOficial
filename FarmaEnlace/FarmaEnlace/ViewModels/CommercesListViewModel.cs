using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.ViewModels
{
    public class CommercesListViewModel : BaseViewModel
    {

        #region Services
        List<Commerce> commerces;
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        DataService dataService;

        #endregion

        #region Atributtes
        bool _isVisibleStock;
        bool _isRefreshing;
        bool _isToggled = false;
        bool _isVisible = false;
        bool _isVisibleLabel = false;
        double _longitude;
        double _latitude;
        string _filter;
        string _filterList;
        string _switchOffColor;
        string _switchOnColor;
        string _switchThumbColor;
        string _textoResultadoColor;
        string _textoResultado;
        string _typeSale;

        string _phoneNumber;

        ObservableCollection<Commerce> _commerces;

        private bool isCallVoz;
        #endregion

        #region Properties


        public string PhoneNumber

        {
            get { return this._phoneNumber; }

            set { SetValue(ref this._phoneNumber, value); }
        }

        public string TypeSale
        {
            get { return this._typeSale; }
            set { SetValue(ref this._typeSale, value); }
        }
        public string SwitchOffColor
        {
            get { return this._switchOffColor; }
            set { SetValue(ref this._switchOffColor, value); }
        }
        public string SwitchOnColor
        {
            get { return this._switchOnColor; }
            set { SetValue(ref this._switchOnColor, value); }
        }
        public string SwitchThumbColor
        {
            get { return this._switchThumbColor; }
            set { SetValue(ref this._switchThumbColor, value); }
        }
        public string TextoResultadoColor
        {
            get { return this._textoResultadoColor; }
            set { SetValue(ref this._textoResultadoColor, value); }
        }
        public string TextoResultado
        {
            get { return this._textoResultado; }
            set { SetValue(ref this._textoResultado, value); }
        }

        public double Latitude
        {
            get { return this._latitude; }
            set { SetValue(ref this._latitude, value); }
        }

        public double Longitude
        {
            get { return this._longitude; }
            set {  SetValue(ref this._longitude, value); }
        }

        public bool IsToggled
        {
            get { return this._isToggled; }
            set
            {
                SetValue(ref this._isToggled, value);
            }
        }

        public List<StockProduct> StockProducts
        {
            get;
            set;
        }

        public bool IsVisibleStock
        {
            get { return this._isVisibleStock; }
            set { SetValue(ref this._isVisibleStock, value); }
        }

        public bool IsVisible
        {
            get { return this._isVisible; }
            set { SetValue(ref this._isVisible, value); }
        }


        public bool NearbyPharmacies
        {
            get;
            set;
        }


        public string Filter
        {
            get { return this._filter; }
            set { SetValue(ref this._filter, value); }
        }
        public string FilterList
        {
            get { return this._filterList; }
            set { SetValue(ref this._filterList, value); }
        }
        
        public bool TwentyFourHours
        {
            get;
            set;
        }

        public ObservableCollection<Commerce> CommercesCollection
        {
            get { return this._commerces; }
            set { SetValue(ref this._commerces, value); }
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
        
        private List<Pin> Pins
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public CommercesListViewModel()
        {
            instance = this;
            NearbyPharmacies = false;
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            navigationService = new NavigationService();
            MapCommerceCommand = new Command<int>(MapCommerce);
            CommercesCollection = new ObservableCollection<Commerce>();
            LoadColors();
            LoadPins();

            isCallVoz = false;
        }
        #endregion

        #region Sigleton
        static CommercesListViewModel instance;

        public static CommercesListViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CommercesListViewModel();
            }

            return instance;
        }


        #endregion

        #region Methods
       

        async public void LoadPins()
        {
            IsVisibleLabel = false;
            var mainViewModel = MainViewModel.GetInstance();
            UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
            Pins = new List<Pin>();

            try
            {
                CommercesCollection = new ObservableCollection<Commerce>();
                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    commerces = dataService.Get<Commerce>(true);
                    if (commerces.Count == 0)
                    {
                        await dialogService.ShowMessage(
                             Resources.Resource.Error,
                             Resources.Resource.NoInformation);
                        return;
                    }
                }
                else
                {
                    commerces = new List<Commerce>();
                    if (NearbyPharmacies)
                    {
                        TextoResultado = "FARMACIAS CERCANAS";
                        IsVisible = true;
                        IsVisibleStock = false;
                        var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                        var response = await apiService.GetList<Commerce>(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/Commerces?" +
                            "searchCode=" + mainViewModel.Brand.SearchCode +
                            "&shiftPharmacy=" + IsToggled +
                            "&latitude=" + Latitude.ToString(CultureInfo.InvariantCulture) +
                            "&longitude=" + Longitude.ToString(CultureInfo.InvariantCulture));

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            return;
                        }
                        commerces = (List<Commerce>)response.Result;
                    }
                    else if (!NearbyPharmacies && !string.IsNullOrEmpty(Filter))
                    {
                        IsVisibleStock = false;
                        IsVisible = false;
                        TextoResultado = "RESULTADO: " + Filter.ToUpperInvariant();
                        var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                        var response = await apiService.GetList<Commerce>(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/Commerces?" +
                            "searchCode=" + mainViewModel.Brand.SearchCode +
                            "&parameter=" + Helpers.Common.RemoveAccentsWithNormalization(Filter) +
                            "&shiftPharmacy=" + IsToggled +
                            "&latitude=" + Latitude.ToString(CultureInfo.InvariantCulture) +
                            "&longitude=" + Longitude.ToString(CultureInfo.InvariantCulture));

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            return;
                        }

                        commerces = (List<Commerce>)response.Result;

                    }
                    else if (!NearbyPharmacies && TwentyFourHours)
                    {
                        IsVisibleStock = false;
                        IsVisible = false;
                        TextoResultado = "FARMACIAS 24 HORAS";
                        var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                        var response = await apiService.GetList<Commerce>(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/Commerces?" +
                            "searchCode=" + mainViewModel.Brand.SearchCode +
                            "&parameter=" + Filter +
                            "&twentyFourSeven=" + TwentyFourHours +
                            "&latitude=" + Latitude.ToString(CultureInfo.InvariantCulture) +
                            "&longitude=" + Longitude.ToString(CultureInfo.InvariantCulture));

                        if (!response.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                Resources.Resource.Error,
                                response.Message);
                            return;
                        }
                        commerces = (List<Commerce>)response.Result;
                    }
                    else
                    {
                        IsVisibleStock = true;
                        foreach (var item in StockProducts)
                        {
                            //Add stock product
                            item.Commerce.Stock = item.Stock;
                            item.Commerce.State = TypeSale;
                            commerces.Add(item.Commerce);
                        }
                    }
                    SaveCommercesOnDB();
                }
                if (commerces != null && commerces.Count() == 0)
                {
                    IsVisibleLabel = true;
                    await dialogService.ShowMessageBrand(
                                Resources.Resource.Error,
                                "No existen farmacias que cumplan su criterio de búsqueda",
                                "iconinfo",
                                mainViewModel.Brand.SearchCode);
                    return;
                }
               
                foreach (var commerce in commerces)
                {
                    Pins.Add(new Pin
                    {
                        Address = commerce.Address,
                        Label = commerce.Name,
                        Position = new Position(
                            commerce.Latitude,
                            commerce.Longitude),
                        Type = PinType.Place,
                    });
                }
                CommercesCollection = new ObservableCollection<Commerce>(commerces.OrderBy(x => x.Distance));
                
            }
            catch
            {
                await dialogService.ShowMessageBrand(
                               Resources.Resource.Error,
                               Resources.Resource.TryAgain,
                               "iconinfo",
                               mainViewModel.Brand.SearchCode);
            }
            finally
            {
                Filter = string.Empty;
                UserDialogs.Instance.HideLoading();
            }
            
        }

        public void LoadColors()
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand.SearchCode == "002")//Medicity
            {
                SwitchOffColor = "#595959";
                SwitchOnColor = "#80ba27";
                SwitchThumbColor = "#ffffff";
                TextoResultadoColor = "#0071ba";
                
            }
            else if (mainViewModel.Brand.SearchCode == "010")//Punto Natural
            {
                SwitchOffColor = "#595959";
                SwitchOnColor = "#95d600";
                SwitchThumbColor = "#ffffff";
                TextoResultadoColor = "#ff5000";
                
            }
            else //Economicas
            {
                SwitchOffColor = "#595959";
                SwitchOnColor = "#ed1c2e";
                SwitchThumbColor = "#ffffff";
                TextoResultadoColor = "#ed1c2e";
            }
        }

        public ICommand ToggledChangeCommand
        {
            get
            {
                return new RelayCommand(ToggledChange);
            }
        }
        void ToggledChange()
        {
            if (IsToggled)
                IsToggled = false;
            else
                IsToggled = true;
        }

        public ICommand VozCommand
        {
            get
            {
                return new RelayCommand(Voz);
            }
        }

        public void SearchVoz(string cad)
        {
            FilterList = cad;
            if (!string.IsNullOrEmpty(FilterList))
            {
                SearchCommerce();
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
                                //FilterList = await DependencyService.Get<IVoice>().GetVoice();
                                //if (!string.IsNullOrEmpty(FilterList))
                                //{
                                //    SearchCommerce();
                                //}
                                View contentPage = new VoiceRecognition_iOS("CommerceListViewModel");
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
                        FilterList = await DependencyService.Get<IVoice>().GetVoice();
                        if (!string.IsNullOrEmpty(FilterList))
                        {
                            SearchCommerce();
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
        public ICommand SearchCommerceCommand
        {
            get
            {
                return new RelayCommand(SearchCommerce);
            }
        }

        void SearchCommerce()
        {
            IsRefreshing = true;
            NearbyPharmacies = false;
            Filter = FilterList;
            FilterList = string.Empty;
            TwentyFourHours = false;
            IsVisible = true;
            LoadPins();
            IsRefreshing = false;
        }


        async void Maplink()
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                if (Pins != null && Pins.Count > 0)
                {
                    mainViewModel.CommercesListMap = CommercesListMapViewModel.GetInstance();
                    mainViewModel.CommercesListMap.Pins = new ObservableCollection<Pin>(Pins);
                    await navigationService.NavigateOnMaster("CommercesListMapView");
                }
                else
                {
                    await dialogService.ShowMessageBrand(
                    Resources.Resource.Error,
                    "Por favor, realice una nueva búsqueda",
                    "iconinfo",
                    mainViewModel.Brand.SearchCode);
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

        public ICommand MaplinkCommand
        {
            get
            {
                return new RelayCommand(Maplink);
            }
        }

        public ICommand MapCommerceCommand { get; private set; }

        private async void MapCommerce(int idCommerce)
        {
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                if (!string.IsNullOrEmpty(idCommerce.ToString()) && idCommerce != 0)
                {
                    var commerce = commerces.Find(x => x.CommerceId == idCommerce);
                    var Pin = Pins.Where(x => x.Label == commerce.Name);


                    mainViewModel.CommerceMap = CommerceMapViewModel.GetInstance();
                    mainViewModel.CommerceMap.Commerce = commerce;
                    mainViewModel.CommerceMap.IsVisibleInfo = !string.IsNullOrEmpty(commerce.Description);
                    mainViewModel.CommerceMap.Pins = new ObservableCollection<Pin>(Pin);
                    await navigationService.NavigateOnMaster("CommerceMapView");
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

        void SaveCommercesOnDB()
        {
            foreach (var commerce in commerces)
            {
                dataService.InsertOrUpdate(commerce);
            }
        }
        #endregion
    }
}
