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
    public class NavigationPageCardRenderer : NavigationPage
    {
        public string Logo { get; set; }
        public NavigationPageCardRenderer(Page root) : base(root)
        {
            BarBackgroundColor = Color.FromHex("#1F2A6D");
            BarTextColor = Color.FromHex("#ffffff");
        }        
    }
}
