using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FarmaEnlace.Renderers
{
    public class ImageOptimizedRenderer : Image
    {


        //private ICommand TransitionCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            this.AnchorX = 0.48;
        //            this.AnchorY = 0.48;
        //            await this.ScaleTo(0.9, 50, Easing.Linear);
        //            await Task.Delay(50);
        //            await this.ScaleTo(1, 50, Easing.Linear);
        //        });
        //    }
        //}

        //public ImageOptimizedRenderer()
        //{
        //    Initialize();
        //}


        //public void Initialize()
        //{
        //    GestureRecognizers.Add(new TapGestureRecognizer()
        //    {
        //        Command = TransitionCommand
        //    });
        //}


        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create<ImageOptimizedRenderer, string>(p => p.ImageSource, string.Empty, BindingMode.Default, null,
                OnImageSourceChanged);

        private static void OnImageSourceChanged(BindableObject bindable, string oldvalue, string newvalue)
        {
            if (Device.RuntimePlatform == Device.Android) return;
            ImageOptimizedRenderer image = (ImageOptimizedRenderer)bindable;
            Image baseImage = (Image)bindable;
            baseImage.Source = image.ImageSource;
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
    }
}
