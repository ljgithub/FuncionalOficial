using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged 
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        string _identification;
        string _password;
        bool _isToggled;
        bool _isRunning;
        bool _isEnabled;
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

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsToggled)));
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Password)));
                }
            }
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
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            Identification = string.Empty;
            Password = string.Empty;
            IsEnabled = true;
            IsToggled = true;
        }
        #endregion

        #region Commands
        public ICommand RecoverPasswordCommand
        {
            get
            {
                return new RelayCommand(RecoverPassword);
            }
        }

        async void RecoverPassword()
        {
            MainViewModel.GetInstance().PasswordRecovery =
                new PasswordRecoveryViewModel();
            await navigationService.NavigateOnLogin("PasswordRecoveryView");
        }



        public ICommand RegisterNewUserCommand
		{
			get
			{
				return new RelayCommand(RegisterNewUser);
			}
		}

		async void RegisterNewUser()
		{
			MainViewModel.GetInstance().NewCustomer = new NewCustomerViewModel();
            await navigationService.NavigateOnLogin("NewCustomerView");
		}

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
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
        async void Login()
        {
            if (string.IsNullOrEmpty(Identification))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.IdentificationValidation);
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

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetToken(
                urlAPI,
                Identification,
                Password.ToUpperInvariant());

            if (response == null)
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

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    response.ErrorDescription);
                Password = null;
                UserDialogs.Instance.HideLoading();
                return;
            }

            response.IsRemembered = IsToggled;
            response.Password = Password.ToUpperInvariant();
            dataService.DeleteAllAndInsert(response);

            Identification = null;
            Password = null;
            IsRunning = false;
            IsEnabled = true;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.VirtualCard = new VirtualCardViewModel();
            mainViewModel.Token = response;
            UserDialogs.Instance.HideLoading();
            
            await navigationService.SetMainPage("MasterView");
        }
        #endregion
    }
}
