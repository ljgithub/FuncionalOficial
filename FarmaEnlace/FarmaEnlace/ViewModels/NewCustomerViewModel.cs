using System;
using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Interfaces;
using GalaSoft.MvvmLight.Command;
using FarmaEnlace.Helpers;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{


    public class NewCustomerViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        DataService dataService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        bool _isVisiblePasaporte;
        bool _isVisibleRuc;
        bool _isVisibleCedula;
        bool _isEnabledIdentification;
        string _typeId;
        string _identification;
        string _ruc;
        string _passport;
        string _cedula;
        #endregion

        #region Properties


        public string TypeId
        {
            get
            {
                return _typeId;
            }
            set
            {
                if (_typeId != value)
                {
                    _typeId = value;
                    SetKeyboard(value);
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TypeId)));
                }
            }
        }


        public bool IsVisibleRuc
        {
            get
            {
                return _isVisibleRuc;
            }
            set
            {
                if (_isVisibleRuc != value)
                {
                    _isVisibleRuc = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsVisibleRuc)));
                }
            }
        }

        public bool IsVisibleCedula
        {
            get
            {
                return _isVisibleCedula;
            }
            set
            {
                if (_isVisibleCedula != value)
                {
                    _isVisibleCedula = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsVisibleCedula)));
                }
            }
        }

        public bool IsVisiblePasaporte
        {
            get
            {
                return _isVisiblePasaporte;
            }
            set
            {
                if (_isVisiblePasaporte != value)
                {
                    _isVisiblePasaporte = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsVisiblePasaporte)));
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

        public bool IsEnabledIdentification
        {
            get
            {
                return _isEnabledIdentification;
            }
            set
            {
                if (_isEnabledIdentification != value)
                {
                    _isEnabledIdentification = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabledIdentification)));
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




        public string FullName
        {
            get;
            set;
        }




        public string Identification
        {
            get
            {
                return _identification;
            }
            set
            {
                if (_identification != value)
                {
                    _identification = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Identification)));
                }
            }
        }

        public string RUC
        {
            get
            {
                return _ruc;
            }
            set
            {
                if (_ruc != value)
                {
                    _ruc = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(RUC)));
                }
            }
        }

        public string Cedula
        {
            get
            {
                return _cedula;
            }
            set
            {
                if (_cedula != value)
                {
                    _cedula = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Cedula)));
                }
            }
        }

        public string Passport
        {
            get
            {
                return _passport;
            }
            set
            {
                if (_passport != value)
                {
                    _passport = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Passport)));
                }
            }
        }



        public string Email
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Confirm
        {
            get;
            set;
        }

        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        #endregion

        #region Constructors
        public NewCustomerViewModel()
        {
            instance = this;
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            dataService = new DataService();
            Email = string.Empty;
            Password = string.Empty;
            Confirm = string.Empty;
            FullName = string.Empty;
            Identification = string.Empty;
            IsEnabled = true;
            IsVisiblePasaporte = true;
            IsVisibleRuc = false;
            IsVisibleCedula = false;

        }
        #endregion

        #region Sigleton
        static NewCustomerViewModel instance;
        public static NewCustomerViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new NewCustomerViewModel();
            }

            return instance;
        }
        #endregion


        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        public ICommand ShownConfirmacionCommand
        {
            get
            {
                return new RelayCommand(ShownConfirmation);
            }
        }


        /// <summary>
        /// Validación parametrizable para comparar dos atributos.
        /// </summary>
        /// <param name="value">Atributo validado</param>
        /// <param name="validationContext">Resultado de la validación</param>
        /// <returns>Retorna un mensaje de validación exitosa o un mensaje de error que puede ser customizable</returns>
        protected bool IsValidID(string ced)
        {
            int isNumeric;
            int total = 0;
            const int tamañoDNI = 10;
            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            const int numeroProvincias = 24;
            const int tercerDigito = 6;

            if (int.TryParse(ced.Trim(), out isNumeric) && ced.Trim().Length == tamañoDNI)
            {
                int provincia = Convert.ToInt32(string.Concat(ced[0], ced[1], string.Empty));
                int digitoTres = Convert.ToInt32(ced[2] + string.Empty);
                if ((provincia > 0 && provincia <= numeroProvincias || provincia == 30) && digitoTres < tercerDigito)
                {
                    int digitoVerificadorRecibido = Convert.ToInt32(ced[9] + string.Empty);
                    for (int k = 0; k < coeficientes.Length; k++)
                    {
                        int valor = Convert.ToInt32(coeficientes[k] + string.Empty) * Convert.ToInt32(ced[k] + string.Empty);
                        total = valor >= 10 ? total + (valor - 9) : total + valor;
                    }
                    int digitoVerificadorObtenido = total >= 10 ? (total % 10) != 0 ? 10 - (total % 10) : (total % 10) : total;
                    bool resultado = digitoVerificadorObtenido == digitoVerificadorRecibido;

                    if (resultado)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            return false;
        }

        public void SetKeyboard(string TypeId)
        {
            if (!string.IsNullOrEmpty(TypeId))
            {

                if (TypeId.Equals("Pasaporte", StringComparison.InvariantCultureIgnoreCase))
                {
                    Passport = string.Empty;
                    IsEnabledIdentification = true;
                    IsVisiblePasaporte = true;
                    IsVisibleRuc = false;
                    IsVisibleCedula = false;
                    return;
                }
                else if (TypeId.Equals("Ruc", StringComparison.InvariantCultureIgnoreCase))
                {
                    RUC = string.Empty;
                    IsEnabledIdentification = true;
                    IsVisiblePasaporte = false;
                    IsVisibleRuc = true;
                    IsVisibleCedula = false;
                    return;
                }
                else if (TypeId.Equals("Cedula", StringComparison.InvariantCultureIgnoreCase) || TypeId.Equals("Cédula", StringComparison.InvariantCultureIgnoreCase))
                {
                    Cedula = string.Empty;
                    IsEnabledIdentification = true;
                    IsVisiblePasaporte = false;
                    IsVisibleRuc = false;
                    IsVisibleCedula = true;
                    return;
                }
                else
                {
                    Cedula = string.Empty;
                    TypeId = "Cedula";
                    IsEnabledIdentification = true;
                    IsVisiblePasaporte = false;
                    IsVisibleRuc = false;
                    IsVisibleCedula = true;
                    return;

                }
            }
        }


        async void Search()
        {
            try
            {

                if (string.IsNullOrEmpty(TypeId))
                {
                    await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.IdentificationTypeValidation);
                    return;
                }

                if (TypeId.Equals("Cedula", StringComparison.InvariantCultureIgnoreCase) ||
                    TypeId.Equals("Cédula", StringComparison.InvariantCultureIgnoreCase))
                {

                    if (string.IsNullOrEmpty(Cedula))
                    {

                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            Resources.Resource.IdentificationValidation);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }

                    if (!IsValidID(Cedula))
                    {
                        await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.IDValidation);
                        return;
                    }
                    Identification = Cedula;
                }
                if (TypeId.Equals("Ruc", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(RUC))
                    {

                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            Resources.Resource.IdentificationValidation);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                    if (RUC.Length != 13)
                    {
                        await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.RucValidation);
                        return;
                    }

                    if (RUC.Substring(RUC.Length - 3, 3) != "001" ||
                        !IsValidID(RUC.Substring(0, RUC.Length - 3)))
                    {
                        await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.RucValidation);
                        return;
                    }
                    Identification = RUC;
                }
                if (TypeId.Equals("Pasaporte", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(Passport))
                    {
                        await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            Resources.Resource.IdentificationValidation);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                    if (!RegexUtilities.IsValidPassport(Passport))
                    {
                        await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.PasaportValidation);
                        return;
                    }
                    Identification = Passport;
                }

                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);

                IsRunning = true;
                IsEnabled = false;

                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        connection.Message);
                    return;
                }

                var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                var response = await apiService.Get<Customer>(
                    urlAPI,
                    Application.Current.Resources["PrefixAPI"].ToString(),
                    "/Customers/GetCustomer?Identification=" + Identification.ToString());

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        response.Message);
                    return;
                }

                Customer customerNew = (Customer)response.Result;
                if (string.IsNullOrEmpty(customerNew.FullName))
                {

                    IsRunning = false;
                    IsEnabled = true;
                    await dialogService.ShowMessage(
                     Resources.Resource.Error,
                     Resources.Resource.ErrorUserNotExist);
                    return;
                }

                Email = customerNew.Email;
                Phone = customerNew.Phone;
                FullName = customerNew.FullName;
                Day = customerNew.Day == 0 ? string.Empty : customerNew.Day.ToString();
                Month = customerNew.Month == 0 ? string.Empty : customerNew.Month.ToString();
                Year = customerNew.Year == null ? string.Empty : customerNew.Year.ToString();

                await navigationService.NavigateOnLogin("NewCustomerStep2View");


            }
            catch (Exception)
            {
                await dialogService.ShowMessage(
                     Resources.Resource.Error,
                      "Estimado usuario, por favor, reintente su busqueda");
            }
            finally
            {
                SetKeyboard(TypeId);
                UserDialogs.Instance.HideLoading();
                IsRunning = false;
                IsEnabled = true;
            }



        }

        async void Save()
        {
            if (string.IsNullOrEmpty(FullName))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.EnterName);
                return;
            }

            if (string.IsNullOrEmpty(Phone))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.EnterCellPhone);
                return;
            }

            if (!RegexUtilities.IsValidCellPhone(Phone))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.CellPhoneValidation);
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.EnterEmail);
                return;
            }

            if (!RegexUtilities.IsValidEmail(Email))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.EmailValidation);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.PasswordValidation);
                return;
            }

            if (Password.Length < 4 || Password.Length > 6)
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.NewPasswordLengthValidation);
                return;
            }

            if (string.IsNullOrEmpty(Confirm))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.PasswordConfirmValidation);
                return;
            }

            if (!Password.Equals(Confirm))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.NewPasswordConfirmNotMatch);
                return;
            }

            if (string.IsNullOrEmpty(Day))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.DayValidation);
                return;
            }

            if (string.IsNullOrEmpty(Month))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.MonthValidation);
                return;
            }

            if (!IsValidFormatNumberInt(Day, 2))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.DayValidation);
                return;
            }

            if (!IsValidFormatNumberInt(Month, 2) || (int.Parse(Month) > 12 || int.Parse(Month) < 1))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.BirthDayValidation);
                return;
            }

            int year = DateTime.Today.Year;

            if (!string.IsNullOrEmpty(Year) && (!IsValidFormatNumberInt(Year, 4) || int.Parse(Year) > year || Year.Length < 4))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.BirthDayValidation);
                return;
            }

            if (!string.IsNullOrEmpty(Year))
            {
                year = int.Parse(Year);
            }

            int dayMonth = System.DateTime.DaysInMonth(year, int.Parse(Month));

            if ((int.Parse(Month) == 2) && (int.Parse(Day) > 29 || int.Parse(Day) < 1) && string.IsNullOrEmpty(Year))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.BirthDayValidation);
                return;
            }
            else
            {
                if (int.Parse(Day) > dayMonth || int.Parse(Day) < 1)
                {
                    await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.BirthDayValidation);
                    return;
                }
            }


            UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    connection.Message);
                UserDialogs.Instance.HideLoading();
                return;
            }

            string deviceIdentifier = DependencyService.Get<IDevice>().GetIdentifier();

            var customer = new Customer
            {
                Identification = Identification,
                TypeId = TypeId,
                Email = Email,
                FullName = FullName,
                Password = Password.ToUpperInvariant(),
                Phone = Phone,
                Imei = deviceIdentifier,
                TokenPush = string.Empty,
                IdentificationGroup = string.Empty,
                Day = int.Parse(Day),
                Month = int.Parse(Month),
                AuthorizationPromotions = false
            };

            if (!string.IsNullOrEmpty(Year))
            {
                customer.Year = int.Parse(Year);
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var response = await apiService.Post(
                urlAPI,
                Application.Current.Resources["PrefixAPI"].ToString(),
                "/Customers",
                customer);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    response.Message);
                UserDialogs.Instance.HideLoading();
                return;
            }

            Customer customerNew = (Customer)response.Result;

            var response2 = await apiService.GetToken(
                urlAPI,
                Identification,
                Password.ToUpperInvariant());

            if (response2 == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.ErrorService);
                Password = null;
                UserDialogs.Instance.HideLoading();
                return;
            }

            if (string.IsNullOrEmpty(response2.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    response2.ErrorDescription);
                Password = null;
                UserDialogs.Instance.HideLoading();
                return;
            }


            IsRunning = false;
            IsEnabled = true;
            UserDialogs.Instance.HideLoading();


            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = response2;
            MainViewModel.GetInstance().Login = new LoginViewModel();
            await navigationService.SetMainPage("LoginView");

            //se va a llamar a la confirmacion de envio de promociones
            await dialogService.ShowAuthorizationPromotion(customerNew);

        }

        async void ShownConfirmation()
        {
            MainViewModel.GetInstance().Login = new LoginViewModel();
            await navigationService.SetMainPage("LoginView");
        }

        #endregion

        #region validaciones
        private bool IsValidFormatNumberInt(string num, int length)
        {
            bool valid = true;

            if (string.IsNullOrEmpty(num) || string.IsNullOrWhiteSpace(num)
                || (num.Split('.').Length > 1) || (num.Split(',').Length > 1)
                || (num.Split('-').Length > 1))
                valid = false;

            return valid;
        }
        #endregion
    }
}
