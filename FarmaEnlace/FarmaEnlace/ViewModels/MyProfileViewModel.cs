using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class MyProfileViewModel : INotifyPropertyChanged
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

        public string CurrentPassword
        {
            get;
            set;
        }


        public string NewPassword
        {
            get;
            set;
        }

        public string ConfirmPassword
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MyProfileViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

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

        async void Save()
        {
            
            if (string.IsNullOrEmpty(NewPassword))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.NewPasswordValidation);
                return;
            }

            if (NewPassword.Length < 4 || NewPassword.Length > 6) 
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.NewPasswordLengthValidation);
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.PasswordConfirmValidation);
                return;
            }

            if (!NewPassword.Equals(ConfirmPassword))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.NewPasswordConfirmNotMatch);
                return;
            }

            UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
            var mainViewModel = MainViewModel.GetInstance();

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

            var changePasswordRequest = new ChangePasswordRequest
            {
                CurrentPassword = mainViewModel.Token.Password,
                Identification = mainViewModel.Token.UserName,
                NewPassword = NewPassword.ToUpperInvariant(),
            };

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.ChangePassword(
                urlAPI,
                Application.Current.Resources["PrefixAPI"].ToString(),
                "/Customers/ChangePassword",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                changePasswordRequest);

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

            mainViewModel.Token.Password = NewPassword;
            dataService.Update(mainViewModel.Token);

            UserDialogs.Instance.HideLoading();
            await dialogService.ShowMessage(
                Resources.Resource.Info,
                Resources.Resource.PasswordChangeSuccessful, "iconcontrcok.png");

            
            await navigationService.BackOnMaster();

            IsRunning = false;
            IsEnabled = true;
            
            
        }
        #endregion
    }
}
