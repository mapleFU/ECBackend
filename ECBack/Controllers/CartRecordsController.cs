using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECBack.Models;

namespace ECBack.Controllers
{
    public class CartRecordsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/CartRecords
        public IQueryable<CartRecord> GetCartRecords()
        {
            return db.CartRecords;
        }

        // GET: api/CartRecords/5
        [ResponseType(typeof(CartRecord))]
        public async Task<IHttpActionResult> GetCartRecord(int id)
        {
            CartRecord cartRecord = await db.CartRecords.FindAsync(id);
            if (cartRecord == null)
            {
                return NotFound();
            }

            return Ok(cartRecord);
        }

        // PUT: api/CartRecords/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCartRecord(int id, CartRecord cartRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cartRecord.CartRecordID)
            {
                return BadRequest();
            }

            db.Entry(cartRecord).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartRecordExists(id))
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

        // POST: api/CartRecords
        [ResponseType(typeof(CartRecord))]
        public async Task<IHttpActionResult> PostCartRecord(CartRecord cartRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CartRecords.Add(cartRecord);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cartRecord.CartRecordID }, cartRecord);
        }

        // DELETE: api/CartRecords/5
        [ResponseType(typeof(CartRecord))]
        public async Task<IHttpActionResult> DeleteCartRecord(int id)
        {
            CartRecord cartRecord = await db.CartRecords.FindAsync(id);
            if (cartRecord == null)
            {
                return NotFound();
            }

            db.CartRecords.Remove(cartRecord);
            await db.SaveChangesAsync();

            return Ok(cartRecord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartRecordExists(int id)
        {
            return db.CartRecords.Count(e => e.CartRecordID == id) > 0;
        }
    }
}