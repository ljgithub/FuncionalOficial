using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaEnlace.Interfaces
{
    public interface IGeoLocatorService
    {
        void findLocation(bool hasInternetAccess);
    }
}
