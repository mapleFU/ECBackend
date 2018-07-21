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
    public class VIPsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/VIPs
        public IQueryable<VIP> GetVIPs()
        {
            return db.VIPs;
        }

        // GET: api/VIPs/5
        [ResponseType(typeof(VIP))]
        public async Task<IHttpActionResult> GetVIP(int id)
        {
            VIP vIP = await db.VIPs.FindAsync(id);
            if (vIP == null)
            {
                return NotFound();
            }

            return Ok(vIP);
        }

        // PUT: api/VIPs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVIP(int id, VIP vIP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vIP.VIPID)
            {
                return BadRequest();
            }

            db.Entry(vIP).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VIPExists(id))
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

        // POST: api/VIPs
        [ResponseType(typeof(VIP))]
        public async Task<IHttpActionResult> PostVIP(VIP vIP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VIPs.Add(vIP);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vIP.VIPID }, vIP);
        }

        // DELETE: api/VIPs/5
        [ResponseType(typeof(VIP))]
        public async Task<IHttpActionResult> DeleteVIP(int id)
        {
            VIP vIP = await db.VIPs.FindAsync(id);
            if (vIP == null)
            {
                return NotFound();
            }

            db.VIPs.Remove(vIP);
            await db.SaveChangesAsync();

            return Ok(vIP);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VIPExists(int id)
        {
            return db.VIPs.Count(e => e.VIPID == id) > 0;
        }
    }
}