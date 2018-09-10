using System;
using Xamarin.Forms;

namespace FarmaEnlace.Interfaces
{
    public interface IShare
    {
        void Share(string subject, string message, ImageSource image);
    }
}
