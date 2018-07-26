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
    public class GAttributesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();



        // PUT: api/GAttributes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGAttribute(int id, GAttribute gAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gAttribute.GAttributeID)
            {
                return BadRequest();
            }

            db.Entry(gAttribute).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GAttributeExists(id))
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

        // POST: api/GoodEntities/{GoodID}/GAttributes
        [ResponseType(typeof(GAttribute))]
        [Route("api/GoodEntities/{GoodID}/GAttributes")]
        [HttpPost]
        public IHttpActionResult PostGAttribute(GAttribute gAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            throw new NotImplementedException();
        }

        // DELETE: api/GAttributes/5
        [ResponseType(typeof(GAttribute))]
        public IHttpActionResult DeleteGAttribute(int id)
        {
            GAttribute gAttribute = db.GAttributes.Find(id);
            if (gAttribute == null)
            {
                return NotFound();
            }

            db.GAttributes.Remove(gAttribute);
            db.SaveChanges();

            return Ok(gAttribute);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GAttributeExists(int id)
        {
            return db.GAttributes.Count(e => e.GAttributeID == id) > 0;
        }
    }
}