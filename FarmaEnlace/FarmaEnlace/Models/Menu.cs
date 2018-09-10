using System.Windows.Input;
using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using FarmaEnlace.Views;
using GalaSoft.MvvmLight.Command;

namespace FarmaEnlace.Models
{
    public class Menu
    {
        #region Services
        DataService dataService; 
        NavigationService navigationService; 
        #endregion

        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Constructors
        public Menu()
        {
            dataService = new DataService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        async void Navigate()
        {
            switch (PageName)
            {
                case "LoginView":
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token.IsRemembered = false;
                    dataService.Update(mainViewModel.Token);
                    mainViewModel.Brands = new BrandsViewModel();
                    await navigationService.BackOnBrand();
                    break;
                case "CommercesListView":
                    MainViewModel.GetInstance().CommercesList =
                        new CommercesListViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "CommercesView":
                    MainViewModel.GetInstance().Commerces =
                        new CommercesViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "CategoriesView":
                    MainViewModel.GetInstance().Categories =
                        new CategoriesViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "CategoriesLineView":
                    MainViewModel.GetInstance().Categories =
                        new CategoriesViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "SyncView":
                    MainViewModel.GetInstance().Sync = new SyncViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "MyProfileView":
                    MainViewModel.GetInstance().MyProfile = 
                        new MyProfileViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "ChangeUserView":
                    MainViewModel.GetInstance().ChangeUser =
                        new ChangeUserViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "ProductsView":
                    MainViewModel.GetInstance().Products =
                        new ProductsViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;

                case "VirtualCardView":
                    MainViewModel.GetInstance().VirtualCard =
                        new VirtualCardViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
            }
        }
        #endregion
    }
}
