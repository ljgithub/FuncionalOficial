using FarmaEnlace.Interfaces;
using Android.Provider;
using FarmaEnlace.Android.Implementations;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(UniqueIdAndroid))]
namespace FarmaEnlace.Android.Implementations
{
    public class UniqueIdAndroid : IDevice
    {
        public string GetIdentifier()
        {
            return Settings.Secure.GetString(Forms.Context.ContentResolver, Settings.Secure.AndroidId);
        }
    }
}