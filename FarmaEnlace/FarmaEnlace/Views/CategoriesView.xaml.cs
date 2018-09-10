using FarmaEnlace.ViewModels;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public partial class CategoriesView : ContentPage
    {
        public CategoriesView()
        {
            InitializeComponent();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    var categoriesViewModel = CategoriesViewModel.GetInstance();
        //    if (categoriesViewModel.PreviousCategory != null) // retorno a la vista de lineas
        //    {
        //        if (!categoriesViewModel.CurrentCategory.CategoryId.Equals(categoriesViewModel.PreviousCategory.CategoryId))
        //        {
        //            categoriesViewModel.CategoriesCollection = null;
        //            categoriesViewModel.CurrentCategory = categoriesViewModel.PreviousCategory;
        //            categoriesViewModel.LoadCategories();
        //        }
        //        else if (categoriesViewModel.CurrentCategory.CategoryId.Equals(categoriesViewModel.CategorySelected.CategoryId))
        //        {
        //            categoriesViewModel.CategoriesCollection = null;
        //            categoriesViewModel.PreviousCategory = null;
        //        }
        //        else if (categoriesViewModel.CurrentCategory.CategoryId.Equals(categoriesViewModel.PreviousCategory.CategoryId))
        //        {
        //            categoriesViewModel.CategoriesCollection = null;
        //            categoriesViewModel.CurrentCategory = categoriesViewModel.CategorySelected;
        //            categoriesViewModel.PreviousCategory = null;
        //            categoriesViewModel.LoadCategories();
        //        }

        //    }
        //    return true;
        //}
      
    }
}
