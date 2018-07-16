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
    public class LogisticsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Logistics
        public IQueryable<Logistic> GetLogistics()
        {
            return db.Logistics;
        }

        // GET: api/Logistics/5
        [ResponseType(typeof(Logistic))]
        public async Task<IHttpActionResult> GetLogistic(int id)
        {
            Logistic logistic = await db.Logistics.FindAsync(id);
            if (logistic == null)
            {
                return NotFound();
            }

            return Ok(logistic);
        }

        // PUT: api/Logistics/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLogistic(int id, Logistic logistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logistic.LogisticID)
            {
                return BadRequest();
            }

            db.Entry(logistic).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogisticExists(id))
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

        // POST: api/Logistics
        [ResponseType(typeof(Logistic))]
        public async Task<IHttpActionResult> PostLogistic(Logistic logistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Logistics.Add(logistic);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = logistic.LogisticID }, logistic);
        }

        // DELETE: api/Logistics/5
        [ResponseType(typeof(Logistic))]
        public async Task<IHttpActionResult> DeleteLogistic(int id)
        {
            Logistic logistic = await db.Logistics.FindAsync(id);
            if (logistic == null)
            {
                return NotFound();
            }

            db.Logistics.Remove(logistic);
            await db.SaveChangesAsync();

            return Ok(logistic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogisticExists(int id)
        {
            return db.Logistics.Count(e => e.LogisticID == id) > 0;
        }
    }
}