using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaEnlace.Interfaces
{
    public interface IPermisosGPS
    {
        bool estaActivo { get; }
        void verificarPermisosGPS();
    }
}
