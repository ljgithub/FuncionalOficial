using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        NavigationService navigationService;
        #endregion
        

        #region Constructors
        public MasterViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands

        public ICommand ReturnHomeCommand
        {
            get
            {
                return new RelayCommand(ReturnHome);
            }
        }

        async void ReturnHome()
        {
            await navigationService.BackOnNavigation();
        }
        #endregion
    }
}
