using System;
using System.Threading.Tasks;
using FarmaEnlace.Interfaces;
using ZXing.Mobile;


[assembly: Xamarin.Forms.Dependency(typeof(FarmaEnlace.Android.Implementations.QrCodeScanningService))]
namespace FarmaEnlace.Android.Implementations
{
    public class QrCodeScanningService : IQrCodeScanningService
    {
        public async Task<string> ScanAsync()
        {
            var options = new MobileBarcodeScanningOptions();
            var scanner = new MobileBarcodeScanner();
            var scanResults = await scanner.Scan(options);
            return scanResults.Text;
        }
    }

}