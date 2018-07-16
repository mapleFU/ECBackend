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
    public class GoodAttributesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/GoodAttributes
        public IQueryable<GoodAttribute> GetGoodAttributes()
        {
            return db.GoodAttributes;
        }

        // GET: api/GoodAttributes/5
        [ResponseType(typeof(GoodAttribute))]
        public async Task<IHttpActionResult> GetGoodAttribute(int id)
        {
            GoodAttribute goodAttribute = await db.GoodAttributes.FindAsync(id);
            if (goodAttribute == null)
            {
                return NotFound();
            }

            return Ok(goodAttribute);
        }

        // PUT: api/GoodAttributes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGoodAttribute(int id, GoodAttribute goodAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != goodAttribute.GoodAttributeID)
            {
                return BadRequest();
            }

            db.Entry(goodAttribute).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoodAttributeExists(id))
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

        // POST: api/GoodAttributes
        [ResponseType(typeof(GoodAttribute))]
        public async Task<IHttpActionResult> PostGoodAttribute(GoodAttribute goodAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GoodAttributes.Add(goodAttribute);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = goodAttribute.GoodAttributeID }, goodAttribute);
        }

        // DELETE: api/GoodAttributes/5
        [ResponseType(typeof(GoodAttribute))]
        public async Task<IHttpActionResult> DeleteGoodAttribute(int id)
        {
            GoodAttribute goodAttribute = await db.GoodAttributes.FindAsync(id);
            if (goodAttribute == null)
            {
                return NotFound();
            }

            db.GoodAttributes.Remove(goodAttribute);
            await db.SaveChangesAsync();

            return Ok(goodAttribute);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GoodAttributeExists(int id)
        {
            return db.GoodAttributes.Count(e => e.GoodAttributeID == id) > 0;
        }
    }
}