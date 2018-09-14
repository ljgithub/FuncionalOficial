using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaEnlace.Interfaces
{
    public interface IPermisosGPS
    {
         bool estaActivoGPS { get; set; }
         bool estaActivoRED { get; set; }      
        void activatePermissions();
    }
}
