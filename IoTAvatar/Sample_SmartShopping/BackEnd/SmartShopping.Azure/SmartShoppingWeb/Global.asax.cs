using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SmartShoppingDemoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string ProcessUrl = String.Format("{0}/Home/SetFilter", ConfigurationManager.AppSettings["ProcessBaseUrl"]);
        public static string ServiceUrl = String.Format("{0}/Beacons", ConfigurationManager.AppSettings["ServiceBaseUrl"]);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ProcessDevice.CreateDevices();
        }
    }
}
