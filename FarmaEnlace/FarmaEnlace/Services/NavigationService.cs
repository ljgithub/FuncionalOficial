using System.Threading.Tasks;
using FarmaEnlace.Models;
using FarmaEnlace.Renderers;
using FarmaEnlace.ViewModels;
using FarmaEnlace.Views;
using Xamarin.Forms;

namespace FarmaEnlace.Services
{
    public class NavigationService
    {
        public async Task SetMainPage(string pageName)
        {
            var mainViewModel = MainViewModel.GetInstance();
            switch (pageName)
            {
                case "BrandsView":
                    Application.Current.MainPage = new BrandsView();
                    break;
                case "LoginView":
                    Application.Current.MainPage = new NavigationPageCardRenderer(new LoginView());
                    break;
                case "MasterView":
                    Application.Current.MainPage = new MasterView();
                    break;
                case "DetailPromotionView":
                    Application.Current.MainPage = new DetailPromotionView();
                    break;
                case "DetailBrandsView":
                    if (mainViewModel.Brand.BrandId == 3)
                        Application.Current.MainPage = new NavigationPageRenderer(new DetailBrandsView(), mainViewModel.Brand.Color, mainViewModel.Brand.TextColor, "med_logo.png");
                    else if (mainViewModel.Brand.BrandId == 4)
                        Application.Current.MainPage = new NavigationPageRenderer(new DetailBrandsView(), mainViewModel.Brand.Color, mainViewModel.Brand.TextColor, "pun_logo.png");
                    else
                        Application.Current.MainPage = new NavigationPageRenderer(new DetailBrandsView(), mainViewModel.Brand.Color, mainViewModel.Brand.TextColor, "eco_logo.png");
                    break;
                case "LoadingView":
                    Application.Current.MainPage = new LoadingView();
                    break;
            }
        }

        public async Task NavigateOnMaster(string pageName)
        {
            var mainViewModel = MainViewModel.GetInstance();
            string c = string.Empty;

            switch (pageName)
            {
                case "CategoriesLineView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoriesLineView());
                    break;
                case "CategoriesView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoriesView(),false);
                    break;
                case "CategoriesSub1View":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoriesSub1View(), false);
                    break;
                case "CategoriesSub2View":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoriesSub2View(), false);
                    break;
                case "CategoriesSub3View":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoriesSub3View(), false);
                    break;
                case "CategoriesSub4View":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoriesSub4View(), false);
                    break;
                case "ProductsView":
                    await Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
                    break;
                case "SyncView":
                    await Application.Current.MainPage.Navigation.PushAsync(new SyncView());
                    break;
                case "MyProfileView":
                    App.Master.IsPresented = false;
                    await  App.Navigator.PushAsync(new MyProfileView());
                    break;
                case "LoginView":
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
                    break;
                case "VirtualCardView":
                    App.Master.IsPresented = false;
                    await Application.Current.MainPage.Navigation.PushAsync(new VirtualCardView());
                    break;
                case "DetailBrandsView":
                    await Application.Current.MainPage.Navigation.PushAsync(new DetailBrandsView());
                    break;
                case "DetailPromotionMenuView":
                    await Application.Current.MainPage.Navigation.PushAsync(new DetailPromotionMenuView());
                    break;
                case "DetailProductView":
                    await Application.Current.MainPage.Navigation.PushAsync(new DetailProductView());
                    break;
                case "DetailCommerceView":
                    await Application.Current.MainPage.Navigation.PushAsync(new DetailCommerceView());
                    break;
                case "CommercesListView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CommercesListView());
                    break;
                case "CommercesListMapView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CommercesListMapView());
                    break;
                case "CommercesView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CommercesView());
                    break;
                case "ChangeUserView":
                    App.Master.IsPresented = false;
                    await App.Navigator.PushAsync(new ChangeUserView());
                    break;
                case "CommercesSearchView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CommercesSearchView());
                    break;
                case "CommerceMapView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CommerceMapView());
                    break;
            }
        }

        public async Task NavigateOnLogin(string pageName)
        {
            switch (pageName)
            {
                case "NewCustomerView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewCustomerView());
                    break;
                case "NewCustomerStep2View":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewCustomerStep2View());
                    break;
                case "PasswordRecoveryView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new PasswordRecoveryView());
                    break;
            }
        }

        public async Task BackOnMaster()
        {
            await App.Navigator.PopToRootAsync();
            //await Application.Current.MainPage.Navigation.PopToRootAsync();
        }
        public async Task BackOnNavigation()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }


        public async Task BackOn()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
            //await App.Navigator.PopAsync();
        }

        public async Task BackOnBrand()
        {
            Application.Current.MainPage = new BrandsView();
        }

        public async Task BackOnLogin()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        public async Task Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    
    }
}