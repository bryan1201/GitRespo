using System;
using System.Web.Mvc;
using einvoice.QRCodeReference;
namespace einvoice.Controllers
{
    public class QRToolController : Controller
    {
        // GET: QRTool
        public ActionResult Index()
        {
            Models.QRCode qr = new Models.QRCode();
            qr = qr.InitTestQRCode();
            return View(qr);
        }

        public ActionResult Details(QRCodeReference.QRCode qr)
        {
            return View(qr);
        }

        // GET: QRTool/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                QRCodeReference.QREncryptServiceSoapClient qrc = new QRCodeReference.QREncryptServiceSoapClient();
                
                QRCodeReference.QRCode qr = new QRCodeReference.QRCode();
                qr = qrc.InitQRCode();            
                /*
                qr.InvoiceNumber = collection["InvoiceNumber"]; //"IT23258592";
                qr.InvoiceDate = collection["InvoiceDate"].ToString();// string.Format("{0}{1}", TaiwanCalendar.GetYear(dt), dt.ToString("MMdd"));
                qr.InvoiceTime = collection["InvoiceTime"];//dt.ToString("HHmmss");
                qr.SalesAmount = int.Parse(collection["SalesAmount"]); //100;
                qr.TaxAmount =int.Parse(collection["TaxAmount"]); //5;
                qr.TotalAmount = int.Parse(collection["TotalAmount"]); //qr.SalesAmount + qr.TaxAmount;
                qr.BuyerIdentifier = collection["BuyerIdentifier"]; //"12345678";
                qr.RepresentIdentifier = collection["RepresentIdentifier"]; //"87654321";
                qr.SellerIdentifier = qr.BuyerIdentifier;
                qr.errorCode = 0;
                */
                var QREncryptString = qrc.QREncrypt(qr);
                ViewBag["QREncryptString"] = QREncryptString;
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
