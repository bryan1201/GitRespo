//
// References:
//
//  Getting Started with Entity Framework 6 Code First using MVC 5
//      http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
//
//  Execute Stored Procedure using DBContext
//      http://www.entityframeworktutorial.net/EntityFramework4.3/execute-stored-procedure-using-dbcontext.aspx
//
//  Using DbContext in EF 4.1 Part 10: Raw SQL Queries
//      http://blogs.msdn.com/b/adonet/archive/2011/02/04/using-dbcontext-in-ef-feature-ctp5-part-10-raw-sql-queries.aspx
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

namespace SmartShoppingDemoWeb.Models
{
    public class SmartShoppingDemoWebInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SmartShoppingDemoWebContext>
    {
        protected override void Seed(SmartShoppingDemoWebContext context)
        {
            // For Advertisements Table
            var advertisements = new List<Advertisement>
            {
                new Advertisement{
                    BeaconId = "Beacon01",
                    TargetDeviceId = "Device001",
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