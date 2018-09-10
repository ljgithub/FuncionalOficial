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
    public class PasswordRecoveryViewModel : INotifyPropertyChanged
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
        string _email;
        private string emailUser;
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


        

        public string Identification
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public PasswordRecoveryViewModel()
        {
            apiService = new ApiService();
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

            if (string.IsNullOrEmpty(Identification))
            {
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.IdentificationValidation);
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

            var response = await apiService.PasswordRecovery(
                urlAPI,
                Application.Current.Resources["PrefixAPI"].ToString(),
                "/Customers/PasswordRecovery",
                Identification,
                Email 
                );

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    Resources.Resource.Error,
                    Resources.Resource.EmailErrorSent);
                UserDialogs.Instance.HideLoading();
                return;
            }
            UserDialogs.Instance.HideLoading();
            await dialogService.ShowMessage(
                Resources.Resource.Info,
                Resources.Resource.PasswordSentEmail, "iconoenviado.png");

            
            await navigationService.BackOnLogin();

            IsRunning = false;
            IsEnabled = true;
           
        }
        #endregion
    }
}
