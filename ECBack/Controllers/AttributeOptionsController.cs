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
    public class AttributeOptionsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/AttributeOptions
        public IQueryable<AttributeOption> GetAttributeOptions()
        {
            return db.AttributeOptions;
        }

        // GET: api/AttributeOptions/5
        [ResponseType(typeof(AttributeOption))]
        public async Task<IHttpActionResult> GetAttributeOption(int id)
        {
            AttributeOption attributeOption = await db.AttributeOptions.FindAsync(id);
            if (attributeOption == null)
            {
                return NotFound();
            }

            return Ok(attributeOption);
        }

        // PUT: api/AttributeOptions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAttributeOption(int id, AttributeOption attributeOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attributeOption.GoodAttributeID)
            {
                return BadRequest();
            }

            db.Entry(attributeOption).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttributeOptionExists(id))
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

        // POST: api/AttributeOptions
        [ResponseType(typeof(AttributeOption))]
        public async Task<IHttpActionResult> PostAttributeOption(AttributeOption attributeOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AttributeOptions.Add(attributeOption);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AttributeOptionExists(attributeOption.GoodAttributeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = attributeOption.GoodAttributeID }, attributeOption);
        }

        // DELETE: api/AttributeOptions/5
        [ResponseType(typeof(AttributeOption))]
        public async Task<IHttpActionResult> DeleteAttributeOption(int id)
        {
            AttributeOption attributeOption = await db.AttributeOptions.FindAsync(id);
            if (attributeOption == null)
            {
                return NotFound();
            }

            db.AttributeOptions.Remove(attributeOption);
            await db.SaveChangesAsync();

            return Ok(attributeOption);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttributeOptionExists(int id)
        {
            return db.AttributeOptions.Count(e => e.GoodAttributeID == id) > 0;
        }
    }
}