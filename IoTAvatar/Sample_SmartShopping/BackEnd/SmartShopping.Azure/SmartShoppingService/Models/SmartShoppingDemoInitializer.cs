//
// References:
//
//  Getting Started with Entity Framework 6 Code First using MVC 5
//      http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
//
//  Note:
//      Add <databaseInitializer type="FactoryWebApp.Models.SmartShoppingDemoInitializer, FactoryWebApp" />
//      in Web.config for "Code First".
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SmartShoppingDemoService.Models
{
    public class SmartShoppingDemoServiceInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SmartShoppingDemoServiceContext>
    {
        protected override void Seed(SmartShoppingDemoServiceContext context)
        {
            /*
            var devices = new List<Device>
            {
                new Device{
                    DeviceId = "<DeviceId>",
                    DeviceKey = "<Key>",
                    ConnectionString = "<ConnectionS>",
                    IsUsed = false,
                    TimeStamp = DateTime.UtcNow
                }
            };

            devices.ForEach(s => context.Devices.Add(s));
            context.SaveChanges();
            */
        }
    }
}