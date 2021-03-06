﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartShoppingDemoService.Models
{
    public class SmartShoppingDemoServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public SmartShoppingDemoServiceContext() : base("name=SmartShoppingSampleDB")
        {
        }  

        public System.Data.Entity.DbSet<SmartShoppingDemoService.Models.Device> Devices { get; set; }

        public System.Data.Entity.DbSet<SmartShoppingDemoService.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<SmartShoppingDemoService.Models.Beacon> Beacons { get; set; }

        public System.Data.Entity.DbSet<SmartShoppingDemoService.Models.Advertisement> Advertisements { get; set; }

        public System.Data.Entity.DbSet<SmartShoppingDemoService.Models.BuyInfo> BuyInfoes { get; set; }

        public System.Data.Entity.DbSet<SmartShoppingDemoService.Models.Store> Stores { get; set; }
    }
}
