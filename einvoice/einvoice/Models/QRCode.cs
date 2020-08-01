using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.tradevan.qrutil;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace einvoice.Models
{
    public class QRCode
    {
        private string strrndnum = string.Empty;
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTime { get; set; }
        /*
        public string RandomNumber { get {
                return GenerateRandomNo().ToString("D4"); ;
            } set {
            this.strrndnum = GenerateRandomNo().ToString("D4");
            }
        }
        */
        public string RandomNumber{ get; set; }
        public int SalesAmount { get; set; }
        public int TaxAmount { get; set; }
        public int TotalAmount { get; set; }
        public string BuyerIdentifier { get; set; }  
        public string RepresentIdentifier { get; set; }
        public string SellerIdentifier { get; set; }
        public string BusinessIdentifier { get; set; }

        public string AESKey {
            get
            {
                return Constant.S_AESTestCode;
            }
        }
        public int errorCode { get; set; }
        public string QREncrypterString(bool blTest=true)
        {
            string result = string.Empty;
            string aesKey = AESKey;

            com.tradevan.qrutil.QREncrypter qrEncrypter = new com.tradevan.qrutil.QREncrypter();
            try
            {
                if(blTest==true)
                {
                    QRCode qr = InitTestQRCode();
                    result = qrEncrypter.QRCodeINV(
                        qr.InvoiceNumber,qr.InvoiceDate, qr.InvoiceTime,
                        qr.RandomNumber,qr.SalesAmount, qr.TaxAmount, qr.TotalAmount,
                        qr.BuyerIdentifier, qr.RepresentIdentifier,qr.SellerIdentifier,qr.BuyerIdentifier,
                        qr.AESKey
                        );
                }
                else
                {
                    result = qrEncrypter.QRCodeINV(
                        this.InvoiceNumber, this.InvoiceDate, this.InvoiceTime,
                        this.RandomNumber, this.SalesAmount, this.TaxAmount, this.TotalAmount,
                        this.BuyerIdentifier, this.RepresentIdentifier, this.SellerIdentifier, this.BuyerIdentifier,
                        this.AESKey
                        );
                }
            }
            catch(Exception ex)
            {
                throw(ex);
            }
            return result;
        }

        private int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public QRCode InitTestQRCode()
        {
            var TaiwanCalendar = new System.Globalization.TaiwanCalendar();
            DateTime dt = System.DateTime.Now;
            QRCode qr = new QRCode();
            qr.InvoiceNumber = "IT23258592";
            qr.InvoiceDate = string.Format("{0}{1}",TaiwanCalendar.GetYear(dt), dt.ToString("MMdd"));
            qr.InvoiceTime = dt.ToString("HHmmss");
            qr.RandomNumber = GenerateRandomNo().ToString("D4");
            qr.SalesAmount = 100;
            qr.TaxAmount = 5;
            qr.TotalAmount = qr.SalesAmount + qr.TaxAmount;
            qr.BuyerIdentifier = "12345678";
            qr.BusinessIdentifier = Constant.S_BusinessIdentifier;
            qr.RepresentIdentifier = "87654321";
            qr.SellerIdentifier = qr.BuyerIdentifier;
            qr.errorCode = 0;

            return qr;
        }

        public string ToImage(string code, int size = 180)
        {
            BarcodeWriter writer = new BarcodeWriter();
            QrCodeEncodingOptions qr = new QrCodeEncodingOptions()
            {
                CharacterSet = "UTF-8",
                ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H,
                Height = size,
                Width = size,
            };
            writer.Options = qr;
            writer.Format = BarcodeFormat.QR_CODE;
            Bitmap bitmap = writer.Write(code);

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            //imgCtrl.Src = "data:image/gif;base64," + base64Data;
            return base64Data;
        }
    }
}