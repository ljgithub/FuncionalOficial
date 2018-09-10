using System.Windows.Input;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;


namespace FarmaEnlace.ViewModels
{

    public class BrandItemViewModel : Brand
    {
        

        #region Services
        NavigationService navigationService;
        #endregion

        #region Constructor
        public BrandItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region ICommand
        public ICommand RedirectionMainCommand
        {
            get
            {
                return new RelayCommand(RedirectionMain);
            }
        }

        async void RedirectionMain()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Brand = this;
            mainViewModel.IsVisibleMyMenu = true;
            mainViewModel.IsVisibleMyMenuUser = false;
            mainViewModel.DetailBrands = new DetailBrandsViewModel();
            await navigationService.SetMainPage("MasterView");
            await navigationService.NavigateOnMaster("DetailBrandsView");
        }
        #endregion



    }
}
