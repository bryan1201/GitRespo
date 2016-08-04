using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmartShoppingDemoService.Models;

namespace SmartShoppingDemoService.Controllers
{
    public class DevicesController : Controller
    {
        private SmartShoppingDemoServiceContext db = new SmartShoppingDemoServiceContext();

        // GET: Devices
        public ActionResult Index()
        {
            return View(db.Devices.ToList().OrderByDescending(dev => dev.DeviceId));
        }

        // GET: Devices/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            //return View();
            int count = db.Devices.Count<Device>();
            Device entity = null;
            int maxId = count;

            if (count > 0)
            {
                entity = db.Devices.OrderByDescending(x => x.DeviceId).First();
                if (!int.TryParse(entity.DeviceId.Substring(6), out maxId))
                {
                    maxId = count;
                }
            }

            string deviceId = "Device" + (maxId + 1).ToString("D3");

            // Add the specified device to IoT Hub
            var task = Task.Run(async () =>
            {
                await DeviceManagement.AddDeviceAsync(deviceId);
            });
            task.Wait();

            Device device = new Device
            {
                DeviceId = deviceId,
                DeviceKey = DeviceManagement.GetDeviceKey(),
                ConnectionString = DeviceManagement.GetIothubConnectionString(),
                IsUsed = false,
                IsActive = false,
                TimeStamp = DateTime.UtcNow
            };

            // Add the specified device to [Devices] table
            db.Devices.Add(device);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceId,DeviceKey,ConnectionString,IsUsed,IsActive,TimeStamp")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DevId,DeviceId,DeviceKey,ConnectionString,IsUsed,IsActive,TimeStamp")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
            db.SaveChanges();

            // Remove the specified device from IoT Hub
            var task = Task.Run(async () =>
            {
                await DeviceManagement.RemoveDeviceAsync(device.DeviceId);
            });
            task.Wait();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
