//
// References:
//
//  Getting Started with Entity Framework 6 Code First using MVC 5
//      http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
//
//  Note:
//      Add <databaseInitializer type="SmartShoppingDemoWeb.Models.SmartShoppingDemoWebInitializer, SmartShoppingDemoWeb" />
//      in Web.config for "Code First".
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SmartShoppingDemoProcess.Models
{
    public class SmartShoppingDemoProcessInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SmartShoppingDemoProcessContext>
    {
        protected override void Seed(SmartShoppingDemoProcessContext context)
        {
            // For Advertisements Table
            var advertisements = new List<Advertisement>
            {
                new Advertisement{
                    BeaconId = "Device01",
                    TargetDeviceId = "Device02",
                    SignalStrength = 0,
                    Timestamp = DateTime.UtcNow,
                    CreatedTime = DateTime.UtcNow
                }
            };

            advertisements.ForEach(s => context.Advertisements.Add(s));
            context.SaveChanges();
        }
    }
}