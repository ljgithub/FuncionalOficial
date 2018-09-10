using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public class PopUpViewAlwaysVisible : PopupPage
    {
        public PopUpViewAlwaysVisible(View contentBody, string marca)
        {
            //string backgroundColor1 = "#FFFFFF";
            //switch(marca)
            //{
            //    case "003":
            //        {
            //            backgroundColor1 = "#000000";
            //        }
            //        break;
            //    case "002":
            //        {
            //            backgroundColor1 = "#FFFFFF";
            //        }
            //        break;
            //    case "010":
            //        {
            //            backgroundColor1 = "#FFFFFF";
            //        }
            //        break;
            //}


            Content = new StackLayout()
            {
                Children =
                {
                   new Frame()
                   {
                        CornerRadius = 15,
                        Padding = 2,
                        BackgroundColor = Color.FromHex("#808080"),
                         Content = new StackLayout()
                         {
                                        Children =
                                        {
                                            new Frame()
                                            {
                                                CornerRadius = 15,
                                                Padding = new Thickness(20, 20, 20, 20),
                                                Content = contentBody
                                            }
                                        }
                         }
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
