using System.Threading.Tasks;
using Xamarin.Forms;
using FarmaEnlace.Views;
using Rg.Plugins.Popup.Extensions;
using FarmaEnlace.ViewModels;
using FarmaEnlace.Models;

namespace FarmaEnlace.Services
{
    public class DialogService
    {
        public DialogService()
        {
            
        }


        public async Task ShowMessage(string title, string message, string image)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpView(title, message, image));
        }
        public async Task ShowMessageBrand(string title, string message, string image, string brand)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpView(title, message, image, brand));
        }        
        public async Task ShowMessage(string title, string message)
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand == null || string.IsNullOrEmpty(mainViewModel.Brand.SearchCode))
                await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpView(title, message, "iconadvertencia.png", string.Empty));
            else
                await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpView(title, message, "iconadvertencia.png", mainViewModel.Brand.SearchCode));
        }
        public async Task ShowAuthorizationPromotion(Customer customer)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpView(customer));
        }
        public async Task ShowCarouselSalePlus(View contenPage)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpCarouselSalesPlus(contenPage));
        }

        public async Task ShowPopUpViewAlwaysVisible(View contenPage)
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand == null || string.IsNullOrEmpty(mainViewModel.Brand.SearchCode))
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpViewAlwaysVisible(contenPage, string.Empty));
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(new PopUpViewAlwaysVisible(contenPage, mainViewModel.Brand.SearchCode));
            }
                
        }

        public async Task<bool> ShowConfirm(string title, string message)
		{
            return await Application.Current.MainPage.DisplayAlert(
				title,
				message,
				"Si",
                "No");
		}

        public async Task<bool> ShowNotification(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                Resources.Resource.Accept);
            return true;
        }

    }
}
