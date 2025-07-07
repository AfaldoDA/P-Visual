using QRCoder;
using System.Drawing;

namespace GuestRegistrationApp.Services;
public class BarcodeService
{
    public Image GenerateQRCode(string text)
    {
        using (var qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
        using (var qrCode = new QRCode(qrCodeData))
        {
            return qrCode.GetGraphic(20);
        }
    }
}
