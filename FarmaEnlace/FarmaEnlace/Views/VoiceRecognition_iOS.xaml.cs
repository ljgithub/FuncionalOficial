using FarmaEnlace.Helpers;
using FarmaEnlace.Interfaces;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;

namespace FarmaEnlace.Views
{
	public partial class VoiceRecognition_iOS : ContentView
	{
        public delegate ContentPage GetEditorInstance(string InitialEditorText);
        static public GetEditorInstance EditorFactory;
        static ISpeechToText speechRecognitionInstnace;
        private bool isCall;
        private string ViewModel;

        public VoiceRecognition_iOS ( string viewModel)
		{
			InitializeComponent ();
            speechRecognitionInstnace = DependencyService.Get<ISpeechToText>();
            speechRecognitionInstnace.textChanged += OnTextChange;
            OnStart();
            isCall = false;
            ViewModel = viewModel;

            //var mainViewModel = ViewModels.MainViewModel.GetInstance();
            //if (mainViewModel.Brand == null || string.IsNullOrEmpty(mainViewModel.Brand.SearchCode))
            //{                
            //    switch(mainViewModel.Brand.SearchCode)
            //    {
            //        case "003":
            //            {
            //                ButtonBuscar.TextColor = Color.FromHex("#808080");
            //                //StackBackground.BackgroundColor = Color.FromHex("#808080");
            //                StackBackground.BackgroundColor = Color.FromHex("#808080");
            //            }
            //            break;
            //        case "002": {
            //                ButtonBuscar.TextColor = Color.FromHex("#80ba27");
            //                StackBackground.BackgroundColor = Color.FromHex("#80ba27");
            //            } break;
            //        case "010":
            //            {
            //                ButtonBuscar.TextColor = Color.FromHex("#95d600");
            //                StackBackground.BackgroundColor = Color.FromHex("#95d600");
            //            }
            //            break;
            //    }
            //}
        }

        public void OnStart()
        {
            speechRecognitionInstnace.Start();
        }

        public void OnStop(Object sender, EventArgs args)
        {
            speechRecognitionInstnace.Stop();
            Search(textLabeliOS.Text);

        }
        public void OnTextChange(object sender, EventArgsVoiceRecognition e)
        {
            textLabeliOS.Text = e.Text;
            //se llama cada vez que se habla.... si se quiere se puede ir concatenando las palabras....

            Console.WriteLine(e.Text);           

            if(e.IsFinal && !string.IsNullOrEmpty(e.Text) && isCall == false)
            {
                speechRecognitionInstnace.Stop();
                Search(e.Text);
            }
        }

        private void Search(string cad)
        {            
            isCall = true;
            ClosePopUp();
            //if (ViewModel == "CategoriesViewModel")
            //{
            //    var categoriesViewModel = ViewModels.CategoriesViewModel.GetInstance();
            //    categoriesViewModel.SearhProduct(cad);
            //}
            //else
            //{
            //    var productsViewModel = ViewModels.ProductsViewModel.GetInstance();
            //    productsViewModel.BarCode = string.Empty;
            //    productsViewModel.ProductName = cad;
            //    productsViewModel.LoadProducts();
            //}
            switch(ViewModel)
            {
                case "CategoriesViewModel":{
                        var categoriesViewModel = ViewModels.CategoriesViewModel.GetInstance();
                        categoriesViewModel.SearhProduct(cad);
                    }
                    break;
                case "ProductsViewModel": {
                        var productsViewModel = ViewModels.ProductsViewModel.GetInstance();
                        productsViewModel.BarCode = string.Empty;
                        productsViewModel.ProductName = cad;
                        productsViewModel.LoadProducts();
                    }break;
                case "CommercesSearchViewModel": {
                        var commercesSeachViewModel = ViewModels.CommercesSearchViewModel.GetInstance();
                        commercesSeachViewModel.SearchVoz(cad);
                    } break;
                case "CommerceListViewModel": {
                        var commerceListViewModel = ViewModels.CommercesListViewModel.GetInstance();
                        commerceListViewModel.SearchVoz(cad);
                    } break;
                case "CommerceListMapViewModel": {
                        var commerceListMapViewModel = ViewModels.CommercesListMapViewModel.GetInstance();
                        commerceListMapViewModel.SearchVoz(cad);
                    } break;
                case "CommerceMapViewModel": {
                        var commerceMapViewModel = ViewModels.CommerceMapViewModel.GetInstance();
                        commerceMapViewModel.SearchVoz(cad);
                    } break;
            }
        }

        private async void ClosePopUp()
        {
            await PopupNavigation.PopAsync();
        }
    }
}