using Android.App;
using FarmaEnlace.Interfaces;
using Xamarin.Forms;

public class CloseApplication : ICloseApplication
{
    public void closeApplication()
    {
        var activity = (Activity)Forms.Context;
        activity.FinishAffinity();
    }
}