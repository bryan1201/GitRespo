using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Barcelona.Models;
using Controllers.General;
using Microsoft.ServiceBus.Notifications;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Providers;

namespace VS2012MVC4.Controllers
{
    public class CustMailMessageController : Controller
    {
        private DBContext db = new DBContext();

        public string GetMessageJsonstring(string Receivers, string Subject, string Body)
        {
            string toast = "{ \"data\" :{\"Receiver\":\"" + Receivers +
                            "\",\"Subject\": \"" + Subject +
                            "\", \"Body\":\"" + Body +
                            "\",\"cdt\":\"" + DateTime.Now.ToUniversalTime().ToString("yyyy/MM/dd hh:mm:ss") + "\"} }";
            return toast;
        }

        public string GetJsonMessageBody(string Body)
        {
            string rslt = "";
            List<AndroidMessageBody> custList = new List<AndroidMessageBody>();
            AndroidMessageBody Item = new AndroidMessageBody();
            Item.message = Body;
            custList.Add(Item);

            //rslt = Newtonsoft.Json.JsonConvert.SerializeObject(custList);
            rslt = "{ \"data\" : { \"message\" : \"" + Body + "\" } }";
            return rslt;
        }

        private string GetJsonNotification(string Receivers, string Subject, string Body)
        {
            string rslt = "";

            List<CustNotification> custList = new List<CustNotification>();
            CustNotification Item = new CustNotification();
            Item.Receivers = Receivers;
            Item.Subject = Subject;
            Item.Body = Body;
            custList.Add(Item);

            rslt= Newtonsoft.Json.JsonConvert.SerializeObject(custList);

            return rslt;
        }

        private void SendNotificationAsync(string Receivers, string Subject, string Body)
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            string notificationHubPath = ConfigurationManager.AppSettings["Microsoft.ServiceBus.NotificationHubPath"];

            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHubPath);
            //var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Hello from a .NET App!</text></binding></visual></toast>";
            //await hub.SendWindowsNativeNotificationAsync(toast);
            //hub.SendGcmNativeNotificationAsync("{ \"data\" : { \"message\" : \"Hello from Windows Azure!\" } }");
            string toast = GetJsonMessageBody(Body);//GetMessageJsonstring(Receivers, Subject, Body);
            hub.SendGcmNativeNotificationAsync(toast);
        }

        //
        // GET: /CustMailMessage/

        public ActionResult Index()
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            if (Editor.Equals(""))
                return RedirectToAction("Login", "Account");
            else
                return View(db.custmailmessage.ToList());
        }

        //
        // GET: /CustMailMessage/Details/5

        public ActionResult Details(Guid id)
        {
            CustMailMessage custmailmessage = db.custmailmessage.Find(id);
            if (custmailmessage == null)
            {
                return HttpNotFound();
            }
            return View(custmailmessage);
        }

        //
        // GET: /CustMailMessage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustMailMessage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create(CustMailMessage custmailmessage)
        {
            DefaultMembershipProvider membersShip = new DefaultMembershipProvider();
            int i=0;
            membersShip.FindUsersByName(User.Identity.Name,1,1, out  i);
            if (ModelState.IsValid)
            {
                string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
                custmailmessage.Sender = db.vuser.Where(x => x.UserName.Equals(Editor)).FirstOrDefault().Email;
                List<string> Receiver = Method.StringToList(custmailmessage.Receiver);
                const string mediafile = @"http://barcelonamedia.azurewebsites.net/MediaFile/ShareFileUser";
                const string space = " ";
                string Body = custmailmessage.Body + space + mediafile;
                custmailmessage.IsSuccess = Method.SendMail(Receiver, custmailmessage.Subject, Body);
                SendNotificationAsync(custmailmessage.Receiver, custmailmessage.Subject, Body);

                custmailmessage.Id = Guid.NewGuid();
                custmailmessage.cdt = DateTime.Now;
                db.custmailmessage.Add(custmailmessage);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(custmailmessage);
        }

        //
        // GET: /CustMailMessage/Edit/5

        public ActionResult Edit(Guid id)
        {
            CustMailMessage custmailmessage = db.custmailmessage.Find(id);
            if (custmailmessage == null)
            {
                return HttpNotFound();
            }
            return View(custmailmessage);
        }

        //
        // POST: /CustMailMessage/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustMailMessage custmailmessage)
        {
            if (ModelState.IsValid)
            {
                string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
                custmailmessage.Sender = db.vuser.Where(x => x.UserName.Equals(Editor)).FirstOrDefault().Email;
                List<string> Receiver = Method.StringToList(custmailmessage.Receiver);
                const string mediafile = @"http://barcelonamedia.azurewebsites.net/MediaFile";
                const string space = " ";
                string Body = custmailmessage.Body + space + mediafile;
                custmailmessage.IsSuccess = Method.SendMail(Receiver, custmailmessage.Subject, Body);
                SendNotificationAsync(custmailmessage.Receiver, custmailmessage.Subject, Body);

                custmailmessage.cdt = DateTime.Now.ToUniversalTime();
                db.Entry(custmailmessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(custmailmessage);
        }

        //
        // GET: /CustMailMessage/Delete/5

        public ActionResult Delete(Guid id)
        {
            CustMailMessage custmailmessage = db.custmailmessage.Find(id);
            if (custmailmessage == null)
            {
                return HttpNotFound();
            }
            return View(custmailmessage);
        }

        //
        // POST: /CustMailMessage/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CustMailMessage custmailmessage = db.custmailmessage.Find(id);
            db.custmailmessage.Remove(custmailmessage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}