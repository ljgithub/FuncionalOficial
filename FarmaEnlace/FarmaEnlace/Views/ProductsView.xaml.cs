using FarmaEnlace.Services;
using FarmaEnlace.ViewModels;
using System;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public partial class ProductsView : ContentPage
    {
        private bool isRowEven;

        public int AppearingEvent { get; }

        public ProductsView()
        {
            InitializeComponent();
        }

        private void Cell_OnAppearing(object sender, EventArgs e)
        {
            if (this.isRowEven)
            {
                var viewCell = (ViewCell)sender;
                if (viewCell.View != null)
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#f0f0f0");
                }
            }

            this.isRowEven = !this.isRowEven;
        }

    }
}
