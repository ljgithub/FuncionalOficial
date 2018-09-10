namespace FarmaEnlace.Views
{
    using FarmaEnlace.Renderers;
    using FarmaEnlace.ViewModels;
    using Xamarin.Forms;

	public partial class NewCustomerView : ContentPage
    {
        public NewCustomerView()
        {
            InitializeComponent();
            RadiouGroup.ItemsSource = new[]
            {
                "Cédula",
                "RUC",
                "Pasaporte"
            };
            RadiouGroup.SelectedIndex = 0;
            RadiouGroup.CheckedChanged += RadiouGroup_CheckedChanged;
            Inicializar();
        }
        
        private void RadiouGroup_CheckedChanged(object sender, int e)
        {
            CustomRadioButton radio = sender as CustomRadioButton;
            if (radio == null || radio.Id == -1) return;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.NewCustomer = NewCustomerViewModel.GetInstance();
            mainViewModel.NewCustomer.TypeId = radio.Text;
        }

        private void Inicializar()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.NewCustomer = NewCustomerViewModel.GetInstance();
            mainViewModel.NewCustomer.TypeId = "Cédula";
        }

    }
}
