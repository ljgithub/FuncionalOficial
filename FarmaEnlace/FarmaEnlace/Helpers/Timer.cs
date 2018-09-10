using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    public class Timer
    {
        private readonly Action callback;

        private CancellationTokenSource cancellation;

        public Timer(Action callback)
        {
            this.callback = callback;
            this.cancellation = new CancellationTokenSource();
        }

        public void Start(int time)
        {
            CancellationTokenSource cts = this.cancellation; // safe copy
            Device.StartTimer(TimeSpan.FromSeconds(time),
                () => {
                    if (cts.IsCancellationRequested) return false;
                    this.callback.Invoke();
                    return false; // or true for periodic behavior
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}
