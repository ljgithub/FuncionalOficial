using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Helpers;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class ChangeUserViewModel : INotifyPropertyChanged
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
        string _name;
        string _email;
        string _phone;
        
        #endregion

        #region Properties

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

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Phone)));
                }
            }
        }

         public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }


        #endregion

        #region Constructors
        public ChangeUserViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            var mainViewModel = MainViewModel.GetInstance();
            Load();
            IsEnabled = true;
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

            
        async void Load()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                IsRunning = true;
                IsEnabled = false;
                Customer customer = new Customer();
                var mainViewModel = MainViewModel.GetInstance();
                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    await dialogService.ShowMessage(
                            Resources.Resource.Error,
                            Resources.Resource.ErrorConection);
                    await navigationService.BackOnMaster();
                    return;
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
                    customer = (Customer)response.Result;
                    Email = customer.Email;
                    Phone = customer.Phone;
                    Name = customer.FullName;
                }


                IsRunning = false;
                IsEnabled = true;
            }
            catch (System.Exception)
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


        async void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    await dialogService.ShowMessage(
                        Resources.Resource.Error,
                        Resources.Resource.EnterEmail);
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
                var mainViewModel = MainViewModel.GetInstance();
                var changeUser = new ChangeUser
                {
                    Identification = mainViewModel.Token.UserName,
                    Email = Email,
                    Phone = Phone,
                };

                var urlAPI = Application.Current.Resources["URLAPI"].ToString();

                var response = await apiService.ChangeUser(
                    urlAPI,
                    Application.Current.Resources["PrefixAPI"].ToString(),
                    "/Customers/ChangeUser",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken,
                    changeUser);

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

                UserDialogs.Instance.HideLoading();
                await dialogService.ShowMessage(
                    Resources.Resource.Info,
                    "Su perfil ha sido actualizado correctamente", "iconperfactualic.png");


                await navigationService.BackOnMaster();
                IsRunning = false;
                IsEnabled = true;

            }
            catch (System.Exception)
            {
                await dialogService.ShowMessage(
                 Resources.Resource.Error,
                  "Estimado usuario, por favor, reintente su busqueda");
                UserDialogs.Instance.HideLoading();
            }
            
        }
        #endregion
    }
}
