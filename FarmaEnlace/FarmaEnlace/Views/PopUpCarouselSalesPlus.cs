using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public class PopUpCarouselSalesPlus: PopupPage
    {            

        public PopUpCarouselSalesPlus(View contentBody)
        {          
            Content = new StackLayout()
            {
                Children =
                {
                   new Frame()
                   {
                        CornerRadius = 15,
                        Padding = 2,
                        BackgroundColor = Color.FromHex("#FFFFFF"),
                        Content = contentBody
                   }
                },
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(20, 20, 20, 20),
                Margin = new Thickness(25, 0, 25, 0)
            };
        }

        protected override Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(1);
        }

        protected override Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}
