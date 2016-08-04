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
    public class BeaconsDataController : ApiController
    {
        private SmartShoppingDemoServiceContext db = new SmartShoppingDemoServiceContext();

        // GET: api/BeaconsData
        public IQueryable<Beacon> GetBeacons()
        {
            return db.Beacons;
        }

        // GET: api/BeaconsData/5
        [ResponseType(typeof(Beacon))]
        public IHttpActionResult GetBeacon(long id)
        {
            Beacon beacon = db.Beacons.Find(id);
            if (beacon == null)
            {
                return NotFound();
            }

            return Ok(beacon);
        }

        // PUT: api/BeaconsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeacon(long id, Beacon beacon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beacon.Id)
            {
                return BadRequest();
            }

            db.Entry(beacon).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeaconExists(id))
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

        // POST: api/BeaconsData
        [ResponseType(typeof(Beacon))]
        public IHttpActionResult PostBeacon(Beacon beacon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Beacons.Add(beacon);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = beacon.Id }, beacon);
        }

        // DELETE: api/BeaconsData/5
        [ResponseType(typeof(Beacon))]
        public IHttpActionResult DeleteBeacon(long id)
        {
            Beacon beacon = db.Beacons.Find(id);
            if (beacon == null)
            {
                return NotFound();
            }

            db.Beacons.Remove(beacon);
            db.SaveChanges();

            return Ok(beacon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeaconExists(long id)
        {
            return db.Beacons.Count(e => e.Id == id) > 0;
        }
    }
}