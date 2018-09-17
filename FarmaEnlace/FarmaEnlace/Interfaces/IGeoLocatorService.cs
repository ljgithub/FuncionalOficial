using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmaEnlace.Interfaces
{
    public interface IGeoLocatorService
    {
        //findLocation: Busca una ubicacion GPS y la escribe en GeolocatorServices.Lat y Long, si lo puso hacer retorna true, caso contrario false
        bool findLocation(bool hasInternetAccess);

        //requestLocationUpdates: Va a ejecutarse en segundo plano y cada vez que encuentre una ubicacion la va a escribir en GeolocatorServices.Lat y Long
        void requestLocationUpdates(bool hasInternetAccess);

    }
}
