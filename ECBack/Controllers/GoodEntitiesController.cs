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
    public class GoodEntitySchema
    {
        public int GoodEntityID { get; set; }
        public string GoodName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

    }

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
            int resultNum = rs.Count();
            List<GoodEntitySchema> resultSchema = new List<GoodEntitySchema>();
            

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    ResultNum = rs.Count(),
                    GoodEntities = rs,
                    PageNum = pn
                }));
        }

        // GET: api/GoodEntities/5
        [ResponseType(typeof(GoodEntity))]
        public async Task<IHttpActionResult> GetGoodEntity(int id)
        {
            GoodEntity entity = db.GoodEntities.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            await db.Entry(entity).Collection(ge => ge.Images).LoadAsync();
            await db.Entry(entity).Collection(ge => ge.SaleEntities).LoadAsync();
            await db.Entry(entity).Collection(ge => ge.GAttributes).LoadAsync();
            foreach (var attr in entity.GAttributes)
            {
                await db.Entry(attr).Collection(a => a.Options).LoadAsync();
            }
            return Ok(entity);
        }

        // GET: api/GoodEntitie
        public async Task<IHttpActionResult> GetGoodEntities([FromUri] CategoryQuery data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int pn = 1;
            if (data != null)
            {
                pn = data.Pn ?? 1;
            }
            
            IQueryable<GoodEntity> goodEntities;
            if (data != null && data.Kw == null)
            {
                goodEntities = db.GoodEntities;
            }
            else
            {
                goodEntities = db.GoodEntities.Where(u => u.GoodName.ToLower().Contains(data.Kw.ToLower()));
            }
            goodEntities = goodEntities.Include(ge => ge.GAttributes).OrderBy(entity => entity.GoodEntityID);
            var rs = await goodEntities.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();

            List<GoodEntitySchema> resultSchema = new List<GoodEntitySchema>();
            foreach (var entity in rs)
            {
                // TODO: only load one image
                // https://stackoverflow.com/questions/3356541/entity-framework-linq-query-include-multiple-children-entities
                await db.Entry(entity).Collection(ge => ge.Images).LoadAsync();
                await db.Entry(entity).Collection(ge => ge.SaleEntities).LoadAsync();
                //await db.Entry(entity).Collection(ge => ge.GAttributes).LoadAsync();
                //foreach (var attr in entity.GAttributes)
                //{
                //    await db.Entry(attr).Collection(a => a.Options).LoadAsync();
                //}
                string image;
                try
                {
                    image = entity.Images.First().ImageURL;
                } catch
                {
                    image = null;
                }
                decimal min_price = entity.SaleEntities.Select(se => se.Price).Min();
                resultSchema.Add(new GoodEntitySchema()
                {
                    GoodName = entity.GoodName,
                    GoodEntityID = entity.GoodEntityID,
                    Image = image,
                    Price = min_price
                });
                
                // load attrs

            }



            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    ResultNum = rs.Count(),
                    GoodEntities = resultSchema,
                    PageNum = pn
                }));
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