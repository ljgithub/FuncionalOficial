using FarmaEnlace.ViewModels;

namespace FarmaEnlace.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main
        {
            get;
            set;
        }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
