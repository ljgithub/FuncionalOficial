using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaEnlace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommercesSearchView : ContentPage
    {
        public CommercesSearchView()
        {
            InitializeComponent();
            FarmaEnlace.Helpers.UpdateUIResponsiveUtil.ChangeUIThisPage(this.Content, 1);
        }
    }
}