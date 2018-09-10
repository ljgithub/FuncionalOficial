using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace FarmaEnlace.ViewModels
{
    public class DetailPromotionViewModel : BaseViewModel
    {


        #region Atributes
        ImageBrand _imageBrand;
        string _textColorDesc;
        string _textColorTitle;
        #endregion

        #region Properties
        public ImageBrand ImageBrand
        {
            get { return this._imageBrand; }
            set { SetValue(ref this._imageBrand, value); }
        }
        public string TextColorTitle
        {
            get { return this._textColorTitle; }
            set { SetValue(ref this._textColorTitle, value); }
        }

        public string TextColorDesc
        {
            get { return this._textColorDesc; }
            set { SetValue(ref this._textColorDesc, value); }
        }
        #endregion

        #region Services
        NavigationService navigationService;
        #endregion


        #region Constructor
        public DetailPromotionViewModel()
        {
            navigationService = new NavigationService();
            instance = this;
            LoadColors();
        }
        #endregion

        #region Sigleton
        static DetailPromotionViewModel instance;

        public static DetailPromotionViewModel GetInstance()
        {
            if (instance == null)
            {
                return new DetailPromotionViewModel();
            }

            return instance;
        }
        #endregion
        #region Methods
        public void LoadColors()
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand.SearchCode == "002")//Medicity
            {
                TextColorTitle = "#0071ba";// Azul
                TextColorDesc = "#80ba27"; //Verde
            }
            else if (mainViewModel.Brand.SearchCode == "010")//Punto Natural
            {
                TextColorDesc = "#95d600"; // Verde fosoforesente
                TextColorTitle = "#ff5000"; //Tomate
            }
            else //Economicas
            {
                TextColorDesc = "#595959"; //plomo 
                TextColorTitle = "#ed1c2e"; // rojo
            }
        }
        #endregion

        #region Commands

        public ICommand ShareCommand
        {
            get
            {
                return new RelayCommand(Share);
            }
        }

        
        async void Share()
        {
            UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
            await Task.Delay(500);
            Image img = new Image()
            {
                Source = ImageBrand.ImageFullPath,
                Aspect = Aspect.AspectFit
            };
            DependencyService.Get<FarmaEnlace.Interfaces.IShare>().Share(ImageBrand.ImageName, ImageBrand.Remarks, img.Source);
            await Task.Delay(500);
            UserDialogs.Instance.HideLoading();
        }



        #endregion


    }
}
