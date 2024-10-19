using OtpNet;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    using OtpNet;
    using QRCoder;
    using System;
    using System.IO;

    namespace Core.Helper
    {
        public class TOTPHelper
        {
            public static string GenerateTotpSecret()
            {
                var key = KeyGeneration.GenerateRandomKey(20);
                var base32String = Base32Encoding.ToString(key);
                return base32String;
            }

            public static bool ValidateTotp(string secret, string code)
            {
                var key = Base32Encoding.ToBytes(secret);
                var totp = new Totp(key);
                return totp.VerifyTotp(code, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);
            }

            public static byte[] GenerateQrCode(string secret, string issuer, string accountName)
            {
                var keyUri = new OtpUri(OtpType.Totp, secret, accountName, issuer);
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(keyUri.ToString(), QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }

            public static string GenerateQrCodeBase64(string secret, string issuer, string accountName)
            {
                var qrCodeBytes = GenerateQrCode(secret, issuer, accountName);
                return Convert.ToBase64String(qrCodeBytes);
            }
        }
    }
}
