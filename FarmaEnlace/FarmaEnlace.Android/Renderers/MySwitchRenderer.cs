using Android.Content;
using Android.Graphics;
using Android.Widget;
using FarmaEnlace.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(MySwitchRendererd))]
public class MySwitchRendererd : SwitchRenderer
{
    private CustomSwitch view;
    public MySwitchRendererd(Context context) : base(context)
    {

    }
    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
    {
        base.OnElementChanged(e);
        if (e.OldElement != null || e.NewElement == null)
            return;
        view = (CustomSwitch)Element;
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
        {
            if (this.Control != null)
            {
                if (this.Control.Checked)
                {
                    this.Control.TrackDrawable.SetColorFilter(view.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                else
                {
                    this.Control.TrackDrawable.SetColorFilter(view.SwitchOffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                this.Control.CheckedChange += this.OnCheckedChange;
                
                this.Element.IsToggled = Control.Checked;
            }
            Control.ThumbDrawable.SetColorFilter(view.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
        }
    }


    
    private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
    {
        if (this.Control.Checked)
        {
            this.Control.TrackDrawable.SetColorFilter(view.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            this.Control.ThumbDrawable.SetColorFilter(view.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
        }
        else
        {
            
            this.Control.TrackDrawable.SetColorFilter(view.SwitchOffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            this.Control.ThumbDrawable.SetColorFilter(Android.Graphics.Color.LightGray, PorterDuff.Mode.Multiply);
        }
        this.Element.IsToggled = Control.Checked;
    }
    protected override void Dispose(bool disposing)
    {
        this.Control.CheckedChange -= this.OnCheckedChange;
        base.Dispose(disposing);
    }
}
