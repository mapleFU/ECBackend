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
using ECBack.Models;

namespace ECBack.Controllers
{
    public class CouponsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Coupons
        public IQueryable<Coupons> GetCoupons()
        {
            return db.Coupons;
        }

        // GET: api/Coupons/5
        [ResponseType(typeof(Coupons))]
        public IHttpActionResult GetCoupons(int id)
        {
            Coupons coupons = db.Coupons.Find(id);
            if (coupons == null)
            {
                return NotFound();
            }

            return Ok(coupons);
        }

        // PUT: api/Coupons/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCoupons(int id, Coupons coupons)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coupons.CouponID)
            {
                return BadRequest();
            }

            db.Entry(coupons).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponsExists(id))
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

        // POST: api/Coupons
        [ResponseType(typeof(Coupons))]
        public IHttpActionResult PostCoupons(Coupons coupons)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Coupons.Add(coupons);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = coupons.CouponID }, coupons);
        }

        // DELETE: api/Coupons/5
        [ResponseType(typeof(Coupons))]
        public IHttpActionResult DeleteCoupons(int id)
        {
            Coupons coupons = db.Coupons.Find(id);
            if (coupons == null)
            {
                return NotFound();
            }

            db.Coupons.Remove(coupons);
            db.SaveChanges();

            return Ok(coupons);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CouponsExists(int id)
        {
            return db.Coupons.Count(e => e.CouponID == id) > 0;
        }
    }
}