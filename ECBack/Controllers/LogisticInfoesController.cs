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
    public class LogisticInfoesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/LogisticInfoes
        public IQueryable<LogisticInfo> GetLogisticInfoes()
        {
            return db.LogisticInfoes;
        }

        // GET: api/LogisticInfoes/5
        [ResponseType(typeof(LogisticInfo))]
        public async Task<IHttpActionResult> GetLogisticInfo(int id)
        {
            LogisticInfo logisticInfo = await db.LogisticInfoes.FindAsync(id);
            if (logisticInfo == null)
            {
                return NotFound();
            }

            return Ok(logisticInfo);
        }

        // PUT: api/LogisticInfoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLogisticInfo(int id, LogisticInfo logisticInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logisticInfo.LogisticInfoId)
            {
                return BadRequest();
            }

            db.Entry(logisticInfo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogisticInfoExists(id))
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

        // POST: api/LogisticInfoes
        [ResponseType(typeof(LogisticInfo))]
        public async Task<IHttpActionResult> PostLogisticInfo(LogisticInfo logisticInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LogisticInfoes.Add(logisticInfo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = logisticInfo.LogisticInfoId }, logisticInfo);
        }

        // DELETE: api/LogisticInfoes/5
        [ResponseType(typeof(LogisticInfo))]
        public async Task<IHttpActionResult> DeleteLogisticInfo(int id)
        {
            LogisticInfo logisticInfo = await db.LogisticInfoes.FindAsync(id);
            if (logisticInfo == null)
            {
                return NotFound();
            }

            db.LogisticInfoes.Remove(logisticInfo);
            await db.SaveChangesAsync();

            return Ok(logisticInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogisticInfoExists(int id)
        {
            return db.LogisticInfoes.Count(e => e.LogisticInfoId == id) > 0;
        }
    }
}