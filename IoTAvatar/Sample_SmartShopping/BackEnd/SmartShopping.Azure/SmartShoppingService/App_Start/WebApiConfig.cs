using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SmartShoppingDemoService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //
            // Routing by Action Name
            // http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
            //
            /*
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            */
        }
    }
}
