using System;
using UIKit;
using Xamarin.Forms;

using CustomSliderDemo.iOS;
using Xamarin.Forms.Platform.iOS;
using FarmaEnlace.Renderers;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(MySwitchRendererd))]
namespace CustomSliderDemo.iOS
{
    public class MySwitchRendererd : SwitchRenderer
    {
        Version version = new Version(ObjCRuntime.Constants.Version);
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            var view = (CustomSwitch)Element;
            if (!string.IsNullOrEmpty(view.SwitchThumbImage))
            {
                if (version > new Version(6, 0))
                {   //n iOS 6 and earlier, the image displayed when the switch is in the on position.
                    Control.OnImage = UIImage.FromFile(view.SwitchThumbImage.ToString());
                    //n iOS 6 and earlier, the image displayed when the switch is in the off position.
                    Control.OffImage = UIImage.FromFile(view.SwitchThumbImage.ToString());
                }
                else
                {
                    Control.ThumbTintColor = view.SwitchThumbColor.ToUIColor();
                }
            }

            Element.Toggled += ElementToggled;
            if (Control != null)
            {
                UpdateUiSwitchColor();
            }

        }

        private void ElementToggled(object sender, ToggledEventArgs e)
        {
            UpdateUiSwitchColor();
        }

        private void UpdateUiSwitchColor()
        {
            var temp = Element as Switch;
            var view = (CustomSwitch)Element;

            if (temp.IsToggled)
            {
                Control.ThumbTintColor = view.SwitchThumbColor.ToUIColor();
                Control.OnTintColor = view.SwitchOnColor.ToUIColor();
            }
            else
            {
                Control.ThumbTintColor = view.SwitchOffColor.ToUIColor();
                Control.TintColor = view.SwitchOffColor.ToUIColor();
            }
        }
    }
}