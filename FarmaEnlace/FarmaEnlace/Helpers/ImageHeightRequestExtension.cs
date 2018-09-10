
using FarmaEnlace.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaEnlace.Helpers
{
    [ContentProperty("HeightRequest")]
    public class ImageHeightRequestExtension : IMarkupExtension
    {
        public string HeightRequest { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (HeightRequest == null)
            {
                return null;
            }
            if (App.ScreenHeight >= 1024)
            {     //este seria el tamanio en un Ipad           
                HeightRequest = "200";
            }
            else if (App.ScreenHeight >= 900)
            {
                //telefono
                HeightRequest = "150";
            }
            else if (App.ScreenHeight >= 700)
            {
                //telefono
                HeightRequest = "120";
            }
            else if (App.ScreenHeight >= 550)
            {
                //telefono
                HeightRequest = "100";
            }
            
            return HeightRequest;
        }
        
    }
}
