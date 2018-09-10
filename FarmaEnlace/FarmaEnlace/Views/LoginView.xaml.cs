using System;
using FarmaEnlace.Services;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            //FarmaEnlace.Helpers.UpdateUIResponsiveUtil.ChangeUIThisPage(this.Content,2);
            ChangueFontSizeLabelsGrid();
        }

        private void ChangueFontSizeLabelsGrid()
        {
            if (App.ScreenWidth >= 1024)
            {
                LabelFontSizeDymanic1.FontSize = 40;
                LabelFontSizeDymanic2.FontSize = 40;
            }
            else if (App.ScreenWidth >= 768)
            {
                //iPad Air 2
                LabelFontSizeDymanic1.FontSize = 30;
                LabelFontSizeDymanic2.FontSize = 30;
            }
            else if (App.ScreenWidth >= 414)
            {
                LabelFontSizeDymanic1.FontSize = 16;
                LabelFontSizeDymanic2.FontSize = 16;
            }
            else if (App.ScreenWidth >= 320)
            {
                //iphone 4
                LabelFontSizeDymanic1.FontSize = 14;
                LabelFontSizeDymanic2.FontSize = 14;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            NavigationService navigationService = new NavigationService();
            Device.BeginInvokeOnMainThread(async () => {
                await navigationService.BackOnBrand();
            });
            return true;
        }
    }
}
