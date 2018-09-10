using System;
using System.Threading.Tasks;
using FarmaEnlace.Android.Helper;
using FarmaEnlace.Android.Implementations;
using FarmaEnlace.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Voice))]
namespace FarmaEnlace.Android.Implementations
{
    public class Voice : IVoice
    {
        public Task<string> GetVoice()
        {
            var task = new TaskCompletionSource<string>();
            try
            {
                IntentHelper.GetRecognition((path) =>
                {
                    task.SetResult(path);
                });
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }
    }
}