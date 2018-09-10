using System;
using System.Threading.Tasks;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.Views
{
    public partial class CommercesListView : ContentPage
    {


        #region Constructors
        public CommercesListView()
        {
            InitializeComponent();
        }
        #endregion
        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var commercesListViewModel = CommercesListViewModel.GetInstance();
            commercesListViewModel.LoadPins();
          
        }
        #region MyRegion


      

      
        #endregion

    }
}
