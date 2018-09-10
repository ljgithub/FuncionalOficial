using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.ViewModels
{
    public class DetailCommerceViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        List<Commerce> listCommerces;
        #endregion

        #region Properties
        public ObservableCollection<Pin> Pins
        {
            get;
            set;
        }
        public List<Commerce> ListCommerces
        {
            get;
            set;
        }
        
        #endregion

        #region Constructors
        public DetailCommerceViewModel()
        {
            instance = this;
            apiService = new ApiService();
            dialogService = new DialogService();
        }
        #endregion

        #region Sigleton
        static DetailCommerceViewModel instance;

        public static DetailCommerceViewModel GetInstance()
        {
            if (instance == null)
            {
                return new DetailCommerceViewModel();
            }

            return instance;
        }

        #endregion

        #region Methods
        public async void LoadPins(double latitude, double longitude)
        {
            Pins = new ObservableCollection<Pin>();
            foreach (var ubication in ListCommerces)
            {
                Pins.Add(new Pin
                {
                    Address = ubication.Address,
                    Label = ubication.Name,
                    Position = new Position(
                        ubication.Latitude, 
                        ubication.Longitude),
                    Type = PinType.Place,
                });
            }
        }

        
        #endregion
    }
}
