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
    public class GoodEntitiesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/GoodEntities
        public IQueryable<GoodEntity> GetGoodEntities()
        {
            return db.GoodEntities;
        }

        // GET: api/GoodEntities/5
        [ResponseType(typeof(GoodEntity))]
        public async Task<IHttpActionResult> GetGoodEntity(int id)
        {
            GoodEntity goodEntity = await db.GoodEntities.FindAsync(id);
            if (goodEntity == null)
            {
                return NotFound();
            }

            return Ok(goodEntity);
        }

        // PUT: api/GoodEntities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGoodEntity(int id, GoodEntity goodEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != goodEntity.GoodEntityID)
            {
                return BadRequest();
            }

            db.Entry(goodEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoodEntityExists(id))
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

        // POST: api/GoodEntities
        [ResponseType(typeof(GoodEntity))]
        public async Task<IHttpActionResult> PostGoodEntity(GoodEntity goodEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GoodEntities.Add(goodEntity);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = goodEntity.GoodEntityID }, goodEntity);
        }

        // DELETE: api/GoodEntities/5
        [ResponseType(typeof(GoodEntity))]
        public async Task<IHttpActionResult> DeleteGoodEntity(int id)
        {
            GoodEntity goodEntity = await db.GoodEntities.FindAsync(id);
            if (goodEntity == null)
            {
                return NotFound();
            }

            db.GoodEntities.Remove(goodEntity);
            await db.SaveChangesAsync();

            return Ok(goodEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GoodEntityExists(int id)
        {
            return db.GoodEntities.Count(e => e.GoodEntityID == id) > 0;
        }
    }
}