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
    public class SaleEntitiesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/SaleEntities/5
        [ResponseType(typeof(SaleEntity))]
        [Route("api/GoodEntities/{GoodID:int}/SaleEntities/{SaleID:int}")]
        public async Task<IHttpActionResult> GetSaleEntity(int GoodID, int SaleID)
        {
            SaleEntity saleEntity = await db.SaleEntities.FindAsync(SaleID);
            GoodEntity goodEntity = await db.GoodEntities.FindAsync(GoodID);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            HttpResponseMessage resp;
            if (saleEntity == null || goodEntity == null)
            {
                if (saleEntity == null)
                {
                    dict.Add("error", "SaleEntity " + SaleID + " not exists");
                } else
                {
                    dict.Add("error", "GoodEntity " + GoodID + " not exists");
                }
                resp = Request.CreateResponse(HttpStatusCode.NotFound, dict);
            } else
            {
                dict.Add("SaleEntity", saleEntity);
                resp = Request.CreateResponse(HttpStatusCode.OK, dict);
            }

            return ResponseMessage(resp);
        }

        // PUT: api/SaleEntities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSaleEntity(int id, SaleEntity saleEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != saleEntity.SaleEntityID)
            {
                return BadRequest();
            }

            db.Entry(saleEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleEntityExists(id))
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

        // POST: api/SaleEntities
        [ResponseType(typeof(SaleEntity))]
        public async Task<IHttpActionResult> PostSaleEntity(SaleEntity saleEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SaleEntities.Add(saleEntity);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = saleEntity.SaleEntityID }, saleEntity);
        }

        // DELETE: api/SaleEntities/5
        [ResponseType(typeof(SaleEntity))]
        public async Task<IHttpActionResult> DeleteSaleEntity(int id)
        {
            SaleEntity saleEntity = await db.SaleEntities.FindAsync(id);
            if (saleEntity == null)
            {
                return NotFound();
            }

            db.SaleEntities.Remove(saleEntity);
            await db.SaveChangesAsync();

            return Ok(saleEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SaleEntityExists(int id)
        {
            return db.SaleEntities.Count(e => e.SaleEntityID == id) > 0;
        }
    }
}