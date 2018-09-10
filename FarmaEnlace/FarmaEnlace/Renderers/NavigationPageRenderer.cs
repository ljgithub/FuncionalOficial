using FarmaEnlace.ViewModels;
using FarmaEnlace.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FarmaEnlace.Renderers
{
    public class NavigationPageRenderer : NavigationPage
    {
        public string Logo { get; set; }
        public NavigationPageRenderer(Page root, string color, string TextColor, string logo) : base(root)
        {
            BarBackgroundColor = Color.FromHex(color);
            BarTextColor = Color.FromHex(TextColor);
            Logo = logo;
        }
    }
}
