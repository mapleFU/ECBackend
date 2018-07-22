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
    public class ImagesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Images
        public IQueryable<Image> GetImages()
        {
            return db.Images;
        }

        // GET: api/Images/5
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> GetImage(int id)
        {
            Image image = await db.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        // PUT: api/Images/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutImage(int id, Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.ImageID)
            {
                return BadRequest();
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/Images
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> PostImage(Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Images.Add(image);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = image.ImageID }, image);
        }

        // DELETE: api/Images/5
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> DeleteImage(int id)
        {
            Image image = await db.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            db.Images.Remove(image);
            await db.SaveChangesAsync();

            return Ok(image);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int id)
        {
            return db.Images.Count(e => e.ImageID == id) > 0;
        }
    }
}