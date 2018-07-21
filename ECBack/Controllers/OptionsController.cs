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
    public class OptionsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Options
        public IQueryable<Option> GetOptions()
        {
            return db.Options;
        }

        // GET: api/Options/5
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetOption(int id)
        {
            Option option = db.Options.Find(id);
            if (option == null)
            {
                return NotFound();
            }

            return Ok(option);
        }

        // PUT: api/Options/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOption(int id, Option option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != option.OptionID)
            {
                return BadRequest();
            }

            db.Entry(option).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OptionExists(id))
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

        // POST: api/Options
        [ResponseType(typeof(Option))]
        public IHttpActionResult PostOption(Option option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Options.Add(option);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = option.OptionID }, option);
        }

        // DELETE: api/Options/5
        [ResponseType(typeof(Option))]
        public IHttpActionResult DeleteOption(int id)
        {
            Option option = db.Options.Find(id);
            if (option == null)
            {
                return NotFound();
            }

            db.Options.Remove(option);
            db.SaveChanges();

            return Ok(option);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OptionExists(int id)
        {
            return db.Options.Count(e => e.OptionID == id) > 0;
        }
    }
}