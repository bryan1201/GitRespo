using System;
using System.Web.Mvc;
using System.Web.Routing;
//using einvoice.QRCodeReference;
namespace einvoice.Controllers
{
    public class QRToolController : Controller
    {
        // GET: QRTool
        public ActionResult Index()
        {
            Models.QRCode qrCode = new Models.QRCode();
            qrCode = qrCode.InitTestQRCode();
            var QREncryptString = qrCode.QREncrypterString(false);
            var qrCodeImgBase64String = qrCode.ToImage(QREncryptString,180);
            ViewData["QREncryptString"] = QREncryptString;
            ViewData["qrCodeImgBase64String"] = qrCodeImgBase64String;
            return View(qrCode);
        }

        public ActionResult Details(Models.QRCode qrCode)
        {
            var QREncryptString = qrCode.QREncrypterString(false);
            var qrCodeImgBase64String = qrCode.ToImage(QREncryptString);
            ViewData["QREncryptString"] = QREncryptString;
            ViewData["qrCodeImgBase64String"] = qrCodeImgBase64String;
            return View(qrCode);
        }

        // GET: QRTool/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QRTool/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                //QRCodeReference.QREncryptServiceSoapClient qrc = new QRCodeReference.QREncryptServiceSoapClient();
                //var QREncryptString = qrc.QREncryptTest();

                Models.QRCode qr = new Models.QRCode();
                
                qr.InvoiceNumber = collection["InvoiceNumber"]; //"IT23258592";
                qr.InvoiceDate = collection["InvoiceDate"].ToString();// string.Format("{0}{1}", TaiwanCalendar.GetYear(dt), dt.ToString("MMdd"));
                qr.InvoiceTime = collection["InvoiceTime"];//dt.ToString("HHmmss");
                qr.SalesAmount = int.Parse(collection["SalesAmount"]); //100;
                qr.TaxAmount =int.Parse(collection["TaxAmount"]); //5;
                qr.TotalAmount = qr.SalesAmount + qr.TaxAmount;
                qr.BuyerIdentifier = collection["BuyerIdentifier"]; //"12345678";
                qr.RepresentIdentifier = collection["RepresentIdentifier"]; //"87654321";
                qr.SellerIdentifier = collection["SellerIdentifier"]; ;
                qr.BusinessIdentifier = collection["BusinessIdentifier"];
                qr.errorCode = 0;
                return RedirectToAction("Details", qr);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return View();
            }
        }

        // GET: QRTool/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QRTool/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: QRTool/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QRTool/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
