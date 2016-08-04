using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SmartShoppingDemoService.Models;

namespace SmartShoppingDemoService.Controllers
{
    public class DevicesDataController : ApiController
    {
        private SmartShoppingDemoServiceContext db = new SmartShoppingDemoServiceContext();

        // GET: api/DevicesData
        public IQueryable<Device> GetDevices()
        {
            return db.Devices;
        }

        // GET: api/DevicesData/5
        [ResponseType(typeof(Device))]
        public IHttpActionResult GetDevice(int id)
        {
            /*
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
            */

            foreach (Device device in db.Devices)
            {
                if (device.IsUsed == false)
                {
                    return Ok(device);
                }
            }
            return NotFound();
        }

        // PUT: api/DevicesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDevice(int id, Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.DevId)
            {
                return BadRequest();
            }

            db.Entry(device).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DevicesData
        [ResponseType(typeof(Device))]
        public IHttpActionResult PostDevice(Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Devices.Add(device);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = device.DevId }, device);
        }

        // DELETE: api/DevicesData/5
        [ResponseType(typeof(Device))]
        public IHttpActionResult DeleteDevice(int id)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            db.Devices.Remove(device);
            db.SaveChanges();

            return Ok(device);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceExists(int id)
        {
            return db.Devices.Count(e => e.DevId == id) > 0;
        }
    }
}