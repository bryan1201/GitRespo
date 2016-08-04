using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartShoppingDemoService.Models;

namespace SmartShoppingDemoService.Controllers
{
    public class HomeController : Controller
    {
        private SmartShoppingDemoServiceContext db = new SmartShoppingDemoServiceContext();

        private static List<SelectListItem> storeList = new List<SelectListItem>();

        public ActionResult Index()
        {
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

        public ActionResult TestCount(string StoreId, string BeaconId, string DeviceId)
        {
            int curStoreId = 0;

            if (StoreId != null)
            {
                // this procedure will add two advertisement records which will later be processed and regarded as an event of "a device step in a specific hot zone".
                db.Database.ExecuteSqlCommand("Exec ProcAddNotification N'" + BeaconId  + "', N'" + DeviceId + "'");
            }

            // Create Store List
            if (storeList.Count == 0)
            {
                foreach (var store in db.Stores)
                {
                    storeList.Add(new SelectListItem { Text = store.StoreId, Value = store.StoreId, Selected = false });
                }
                storeList[0].Selected = true;
            }
            ViewBag.StoreId = storeList;

            // Get index for the current selected StoreId
            for (int i = 0; i < storeList.Count; i++)
            {
                if (storeList[i].Selected == true)
                {
                    curStoreId = i;
                    break;
                }
            }
        
            // Create Beacon List based on the current selected StoreId
            List<SelectListItem> beaconList = new List<SelectListItem>();
            string curStore = storeList[curStoreId].Text;
            foreach (var beacon in db.Beacons.Where(data => data.StoreId == curStore).ToList())
            {
                beaconList.Add(new SelectListItem { Text = beacon.BeaconId, Value = beacon.BeaconId });
            }
            ViewBag.BeaconId = beaconList;

            // Create Device List
            List<SelectListItem> deviceList = new List<SelectListItem>();
            foreach (var dev in db.Devices)
            {
                if (dev.IsUsed)
                    deviceList.Add(new SelectListItem { Text = dev.DeviceId, Value = dev.DeviceId });
            }
            ViewBag.DeviceId = deviceList;

            return View();
        }

        public ActionResult GetCurrentStore(int id)
        {
            for (int i = 0; i < storeList.Count; i++)
            {
                if (id == i)
                    storeList[i].Selected = true;
                else
                    storeList[i].Selected = false;
            }

            return RedirectToAction("TestCount");
        }
    }
}