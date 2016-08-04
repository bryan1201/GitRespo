using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartShoppingDemoWeb.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Configuration;

namespace SmartShoppingDemoWeb.Controllers
{
    public class HomeController : Controller
    {
        private SmartShoppingDemoWebContext db = new SmartShoppingDemoWebContext();

        private static string activeDevice1 = String.Empty;
        private static string activeDevice2 = String.Empty;

        //Settings for GetToken

        //To learn how to get a client app ID, see Register a client app (https://msdn.microsoft.com/en-US/library/dn877542.aspx#clientID)
        private static string clientID = ConfigurationManager.AppSettings["PowerBIClientAppId"];

        

        //RedirectUri you used when you registered your app.
        //For a client app, a redirect uri gives AAD more details on the specific application that it will authenticate.
        //private static string redirectUri = "https://login.live.com/oauth20_desktop.srf";

        //Resource Uri for Power BI API
        private static string resourceUri = "https://analysis.windows.net/powerbi/api";

        //OAuth2 authority Uri
        private static string authority = "https://login.windows.net/common/oauth2/authorize";
        private static AuthenticationContext authContext = null;
        private static string token = String.Empty;

        public class ProdInfo
        {
            public string Prod1 { get; set; }
            public string Prod2 { get; set; }
            public string Count01 { get; set; }
            public string Count02 { get; set; }
            public string Count03 { get; set; }
            public string Count04 { get; set; }
            public string Count05 { get; set; }
            public string Count06 { get; set; }
        }

        /// <summary>
        /// Use AuthenticationContext to get an access token
        /// </summary>
        /// <returns></returns>
        private string AccessToken()
        {

            if (token == String.Empty)
            {
                //string userName = "(PBI Account Name)";
                //string password = "(PBI Password)";
                string userName = ConfigurationManager.AppSettings["PowerBIUsername"];
                string password = ConfigurationManager.AppSettings["PowerBIPassword"];

                // Create an instance of TokenCache to cache the access token
                TokenCache TC = new TokenCache();

                // Create an instance of AuthenticationContext to acquire an Azure access token
                authContext = new AuthenticationContext(authority, TC);

                // Call AcquireToken to get an Azure token from Azure Active Directory token issuance endpoint
                // token = authContext.AcquireToken(resourceUri, clientID, new Uri(redirectUri), PromptBehavior.RefreshSession).AccessToken;
                token = authContext.AcquireToken(resourceUri, clientID, new UserCredential(userName, password)).AccessToken;
            }
            else
            {
                // Get the token in the cache
                token = authContext.AcquireTokenSilent(resourceUri, clientID).AccessToken;
            }

            return token;
        }

        public ActionResult Index()
        {
            // Ref: http://www.asp.net/mvc/overview/getting-started/introduction/adding-a-view
            ViewBag.DeviceKey1 = "Device Key for Avatar01 = "
                            + ProcessDevice.GetDeviceKey("Avatar01");

            ViewBag.DeviceKey2 = "Device Key for Avatar02 = "
                            + ProcessDevice.GetDeviceKey("Avatar02");

            ViewBag.IothubConnectionString = ProcessDevice.GetIothubConnectionString();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Report()
        {
            ViewBag.Token = AccessToken();
            ViewBag.ReportId = ConfigurationManager.AppSettings["PowerBIReportId"];
            return View();
        }

        public ActionResult ReportAuto()
        {
            ViewBag.Token = AccessToken();
            ViewBag.ReportId = ConfigurationManager.AppSettings["PowerBIReportId"];
            return View();
        }

        public ActionResult Tile()
        {
            ViewBag.Token = AccessToken();
            return View();
        }

        private void GetAllProductCount(string store)
        {
            try
            {
                ViewBag.Count01 = db.Database.SqlQuery<int>("Exec ProcCountNotification N'" + store + "', N'Product01'").FirstOrDefault().ToString();
                ViewBag.Count02 = db.Database.SqlQuery<int>("Exec ProcCountNotification N'" + store + "', N'Product02'").FirstOrDefault().ToString();
                ViewBag.Count03 = db.Database.SqlQuery<int>("Exec ProcCountNotification N'" + store + "', N'Product03'").FirstOrDefault().ToString();
                ViewBag.Count04 = db.Database.SqlQuery<int>("Exec ProcCountNotification N'" + store + "', N'Product04'").FirstOrDefault().ToString();
                ViewBag.Count05 = db.Database.SqlQuery<int>("Exec ProcCountNotification N'" + store + "', N'Product05'").FirstOrDefault().ToString();
                ViewBag.Count06 = db.Database.SqlQuery<int>("Exec ProcCountNotification N'" + store + "', N'Product06'").FirstOrDefault().ToString();
            }
            catch (Exception ex)
            {
                ViewBag.Count01 = "11";
                ViewBag.Count02 = "22";
                ViewBag.Count03 = "33";
                ViewBag.Count04 = "44";
                ViewBag.Count05 = "55";
                ViewBag.Count06 = "66";
                string mst = ex.Message;
            }
        }

        // Get Top 2 Active Devices
        private void GetActiveDevices()
        {
            try
            {
                var devList = db.Database.SqlQuery<string>("Exec ProcGetActiveDevices;").ToList();

                if (devList.Count > 0) activeDevice1 = devList[0];
                activeDevice1 = (devList.Count > 0) ? devList[0] : "Device000";
                activeDevice2 = (devList.Count > 1) ? devList[1] : "Device000";
            }
            catch
            {
                activeDevice1 = "Device000";
                activeDevice2 = "Device000";
            }
        }

        [HttpGet]
        public ActionResult UpdateProducts(string store)
        {
            if (String.IsNullOrEmpty(store))
                store = "Store01";

            // Get product Id 
            string notifiedProd1 = String.Empty;
            string notifiedProd2 = String.Empty;

            try
            {
                notifiedProd1 = db.Database.SqlQuery<string>("Exec ProcNotifiedProductByDevice N'" + store + "', N'" + activeDevice1 + "';").FirstOrDefault();
                notifiedProd2 = db.Database.SqlQuery<string>("Exec ProcNotifiedProductByDevice N'" + store + "', N'" + activeDevice2 + "';").FirstOrDefault();
            }
            catch
            {
                notifiedProd1 = "Product00";
                notifiedProd2 = "Product00";
            }

            if (String.IsNullOrEmpty(notifiedProd1))
                notifiedProd1 = "Product00";

            if (String.IsNullOrEmpty(notifiedProd2))
                notifiedProd2 = "Product00";

            // Get notification count for the all products
            GetAllProductCount(store);

            var info = JsonConvert.SerializeObject(
                       new ProdInfo
                       {
                           Prod1 = notifiedProd1,
                           Prod2 = notifiedProd2,
                           Count01 = ViewBag.Count01,
                           Count02 = ViewBag.Count02,
                           Count03 = ViewBag.Count03,
                           Count04 = ViewBag.Count04,
                           Count05 = ViewBag.Count05,
                           Count06 = ViewBag.Count06
                       }
                       );

            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Products(string store)
        {
            if (!String.IsNullOrEmpty(store) && store == "Store02")
                return RedirectToAction("Products_2");
            else
                return RedirectToAction("Products_1");
        }

        public ActionResult Products_1()
        {
            // Get Top 2 Active Devices
            GetActiveDevices();

            GetAllProductCount("Store01");

            return View();
        }

        public ActionResult Products_2()
        {
            // Get Top 2 Active Devices
            GetActiveDevices();

            GetAllProductCount("Store02");

            return View();
        }

        public ActionResult ProductsIndex(string ProductId, string store)
        {
            //  Execute Stored Procedure using DBContext
            //  http://www.entityframeworktutorial.net/EntityFramework4.3/execute-stored-procedure-using-dbcontext.aspx

            switch (store)
            {
                case "Store02":
                    ViewBag.Store = "Store02";
                    break;

                default:
                    ViewBag.BackPage = "Store01";
                    break;
            }

            if (ProductId == null)
                return View(db.Database.SqlQuery<Advertisement>("Exec ProcProductList N'" + store + "', N'Product01'").ToList());

            return View(db.Database.SqlQuery<Advertisement>("Exec ProcProductList N'" + store + "', N'" + ProductId + "'").ToList());
        }

        public ActionResult Notifications()
        {
            return View(db.Database.SqlQuery<Advertisement>("Exec ProcGetNotifications;").ToList());
        }
    }
}