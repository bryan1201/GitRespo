using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CERLLAB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "LabInformation", action = "Home", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "F_CERLCreate",
                url: "F_CERL/Create/{id}/{FlowCode}/{State}",
                defaults: new { controller = "F_CERL", action = "Create"}
            );
        }
    }
}