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
    public class OrderformsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Orderforms
        public IQueryable<Orderform> GetOrderforms()
        {
            return db.Orderforms;
        }

        // GET: api/Orderforms/5
        [ResponseType(typeof(Orderform))]
        public async Task<IHttpActionResult> GetOrderform(int id)
        {
            Orderform orderform = await db.Orderforms.FindAsync(id);
            if (orderform == null)
            {
                return NotFound();
            }

            return Ok(orderform);
        }

        // PUT: api/Orderforms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderform(int id, Orderform orderform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderform.OrderformID)
            {
                return BadRequest();
            }

            db.Entry(orderform).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderformExists(id))
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

        // POST: api/Orderforms
        [ResponseType(typeof(Orderform))]
        public async Task<IHttpActionResult> PostOrderform(Orderform orderform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orderforms.Add(orderform);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderform.OrderformID }, orderform);
        }

        // DELETE: api/Orderforms/5
        [ResponseType(typeof(Orderform))]
        public async Task<IHttpActionResult> DeleteOrderform(int id)
        {
            Orderform orderform = await db.Orderforms.FindAsync(id);
            if (orderform == null)
            {
                return NotFound();
            }

            db.Orderforms.Remove(orderform);
            await db.SaveChangesAsync();

            return Ok(orderform);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderformExists(int id)
        {
            return db.Orderforms.Count(e => e.OrderformID == id) > 0;
        }
    }
}