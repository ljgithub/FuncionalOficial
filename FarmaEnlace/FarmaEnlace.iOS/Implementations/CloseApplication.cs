using System.Threading;
using FarmaEnlace.Interfaces;

public class CloseApplication : ICloseApplication
{
    public void closeApplication()
    {
        Thread.CurrentThread.Abort();
    }
}