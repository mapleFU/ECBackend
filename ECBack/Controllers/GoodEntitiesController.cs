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
using ECBack.Filters;
using ECBack.Models;

namespace ECBack.Controllers
{
    public class GoodEntitiesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        private const int PageDataNumber = 15;

        //[NonAction]
        //public async Task<List<T>> GetEntityWithData<T>(IQueryable<T> dataSet, CategoryQuery data)
        //{
        //    IQueryable<T> searchSet;
        //    if (data.Kw == null)
        //    {
        //        searchSet = dataSet;
        //    } else
        //    {
        //        searchSet = db.GoodEntities.Where(u => u.GoodName.ToLower().Contains(data.Kw.ToLower()));
        //    }
        //}

        [HttpGet]
        [Route("api/Categories/{CategoryID:int}/GoodEntities")]
        public async Task<IHttpActionResult> GetRelatedEntities(int CategoryID,  [FromUri] CategoryQuery data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            int pn = data.Pn ?? 1;
            IQueryable<GoodEntity> fullEntities;
            IQueryable<GoodEntity> goodEntities;
            var cate = await db.Categories.FindAsync(CategoryID);
            db.Entry(cate).Reference(c => c.GoodEntities).Load();
            fullEntities = cate.GoodEntities.AsQueryable();
            if (data.Kw == null)
            {
                goodEntities = fullEntities;
            }
            else
            {
                goodEntities = fullEntities.Where(u => u.GoodName.ToLower().Contains(data.Kw.ToLower()));
            }

            var rs = await goodEntities.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    result_num = rs.Count(),
                    goodentities = rs,
                    page_num = pn
                }));
        }

        // GET: api/GoodEntities/5
        [ResponseType(typeof(GoodEntity))]
        public IHttpActionResult GetGoodEntity(int id)
        {
            GoodEntity goodEntity = db.GoodEntities.Find(id);
            if (goodEntity == null)
            {
                return NotFound();
            }

            return Ok(goodEntity);
        }
        // GET: api/GoodEntities
        public async Task<IHttpActionResult> GetGoodEntities([FromUri] CategoryQuery data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (data.Kw == null)
            {
                return BadRequest("No Kw in your URI");
            }
            int pn = data.Pn ?? 1;
            IQueryable<GoodEntity> goodEntities;
            if (data.Kw == null)
            {
                goodEntities = db.GoodEntities;
            }
            else
            {
                goodEntities = db.GoodEntities.Where(u => u.GoodName.ToLower().Contains(data.Kw.ToLower()));
            }
           
            var rs = await goodEntities.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();
            
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    result_num = rs.Count(),
                    goodentities = rs,
                    page_num = pn
                }));
        }

        // GET: api/GoodEntities/5
        //[ResponseType(typeof(GoodEntity))]
        //public async Task<IHttpActionResult> GetGoodEntity(int id)
        //{
        //    GoodEntity goodEntity = await db.GoodEntities.FindAsync(id);
        //    if (goodEntity == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(goodEntity);
        //}

        // PUT: api/GoodEntities/5
        [ResponseType(typeof(void))]
        [AuthenticationFilter]
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
                db.SaveChanges();
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
        [AuthenticationFilter]
        public async Task<IHttpActionResult> PostGoodEntity(GoodEntity goodEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GoodEntities.Add(goodEntity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = goodEntity.GoodEntityID }, goodEntity);
        }

        // DELETE: api/GoodEntities/5
        [ResponseType(typeof(GoodEntity))]
        public IHttpActionResult DeleteGoodEntity(int id)
        {
            GoodEntity goodEntity = db.GoodEntities.Find(id);
            if (goodEntity == null)
            {
                return NotFound();
            }

            db.GoodEntities.Remove(goodEntity);
            db.SaveChanges();

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