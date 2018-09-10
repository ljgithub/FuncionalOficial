
using FarmaEnlace.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaEnlace.Helpers
{
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Brand != null)
            {
                if (mainViewModel.Brand.BrandId == 3)
                    Source = "med_" + Source;
                else if (mainViewModel.Brand.BrandId == 4)
                    Source = "pun_" + Source;
            }
            return Source;
        }
        
    }
}
