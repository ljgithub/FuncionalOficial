using System;
using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceSO))]
namespace FarmaEnlace.iOS.Implementations
{
    public class DeviceSO : IDeviceSO
    {
        public string GetDeviceSO()
        {
            return UIDevice.CurrentDevice.SystemVersion;
        }
    }
}