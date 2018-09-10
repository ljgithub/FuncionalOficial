using FarmaEnlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaEnlace.Views
{
	public partial class CarouselSalePlus : ContentView
	{
		public CarouselSalePlus ()
		{
			InitializeComponent ();
            var detailVirturalCardViewModel = ViewModels.VirtualCardViewModel.GetInstance();

            CarouselImages.ItemSelected += (sender, args) =>
            {
                var image = args.SelectedItem as ImageBrand;
                if (image == null)
                    return;

                detailVirturalCardViewModel.ItemSelected = image;
            };

            ChangeHeightRequestPopUp();
        }

        private void ChangeHeightRequestPopUp()
        {
            if (App.ScreenHeight >= 1024)
            {     //este seria el tamanio en un Ipad           
                VirtualCardName.HeightRequest = 800;
            }
            else if (App.ScreenHeight >= 900)
            {
                //telefono
                VirtualCardName.HeightRequest = 800;
            }
            else if (App.ScreenHeight >= 700)
            {
                //telefono
                VirtualCardName.HeightRequest = 600;
            }
        }
    }
}