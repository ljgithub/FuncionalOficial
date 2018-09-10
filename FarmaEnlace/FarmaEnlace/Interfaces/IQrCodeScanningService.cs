using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmaEnlace.Interfaces
{
    public interface IQrCodeScanningService
    {
        Task<string> ScanAsync();
    }

}
