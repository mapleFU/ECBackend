using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        public string DetailImages { get; set; }
        // public decimal Price { get; set; }
        public int? Stock { get; set; } //
        public string Brief { get; set; }
        public int BrandID { get; set; }
        public decimal GoodPrice { get; set; }
        //public int AllNum { get; set; }
        //public int PageNum { get; set; }
    }

    public class GoodEntitiesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        private const int PageDataNumber = 15;

        [HttpPost]
        [Route("api/GoodEntities")]
        [SellerAuthFilter]
        public async Task<IHttpActionResult> AddGoodEntity([FromBody] GoodEntitySchema goodEntitySchema)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Seller seller = (Seller)HttpContext.Current.User;
            GoodEntity goodEntity = new GoodEntity()
            {
                SellerID = seller.SellerID,
                Seller = seller,
                BrandID = goodEntitySchema.BrandID,
                Stock = goodEntitySchema.Stock ?? 0,
                Brief = goodEntitySchema.Brief,
                DetailImages = goodEntitySchema.DetailImages,
                FavoriteNum = 0,
                GoodEntityState = 0,
                SellProvince = "上海",

            };

            goodEntity.SellerID = seller.SellerID;
            db.GoodEntities.Add(goodEntity);
            await db.SaveChangesAsync();
            var resp = Request.CreateResponse(HttpStatusCode.NoContent);
            resp.Headers.Add("Location", "api/GoodEntities/" + goodEntity.GoodEntityID);
            return ResponseMessage(resp);
        }


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
            await db.Entry(cate).Collection(c => c.GoodEntities).LoadAsync();
            fullEntities = cate.GoodEntities.AsQueryable();
            if (data.Kw == null)
            {
                goodEntities = fullEntities;
            }
            else
            {
                goodEntities = fullEntities.Where(u => u.GoodName.ToLower().Contains(data.Kw.ToLower()));
            }
            int allNum = (await goodEntities.CountAsync()) / PageDataNumber;
            if (allNum % PageDataNumber != 0)
            {
                allNum += 1;
            }

            var rs = await goodEntities.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();
            int resultNum = rs.Count();
            List<GoodEntitySchema> resultSchema = new List<GoodEntitySchema>();
            

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    ResultNum = rs.Count(),
                    GoodEntities = rs,
                    PageNum = pn,
                   
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

        // GET: api/GoodEntities
        [HttpGet]
        [Route("api/GoodEntities")]
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
                System.Diagnostics.Debug.WriteLine("data is not null.");
                goodEntities = db.GoodEntities;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("data is null.");
                if (db.GoodEntities == null)
                {
                    System.Diagnostics.Debug.WriteLine("db.GE is null.");
                    goodEntities = null;
                } else
                {
                    System.Diagnostics.Debug.WriteLine("db.GE is not null.");

                    goodEntities = db.GoodEntities.Where(u => u.GoodName.ToLower().Contains(data.Kw.ToLower()));
                    System.Diagnostics.Debug.WriteLine("Get search result.");
                    int cnt = await goodEntities.CountAsync();

                    if (cnt == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Cnt is zero");
                        goodEntities = null;
                    } else
                    {
                        System.Diagnostics.Debug.WriteLine("cnt is " + cnt);
                    }

                }
            }
            List<GoodEntitySchema> resultSchema = new List<GoodEntitySchema>();
            if (goodEntities == null)
            {
                System.Diagnostics.Debug.WriteLine("goodentity is null ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       ResultNum = resultSchema.Count(),
                       GoodEntities = resultSchema,
                       PageNum = pn,
                       AllNum = 0
                   }));
            }
            System.Diagnostics.Debug.WriteLine("ge is not null");
            goodEntities = goodEntities.Include(ge => ge.GAttributes).OrderBy(entity => entity.GoodEntityID);
            System.Diagnostics.Debug.WriteLine("ge is still not null");
            int allNum = await goodEntities.CountAsync();
            System.Diagnostics.Debug.WriteLine("ge get success");
            int pageNum = allNum / PageDataNumber;
            if (allNum % 15 != 0)
            {
                pageNum += 1;
            }
            System.Diagnostics.Debug.WriteLine("ge cnt success");
            if (goodEntities == null)
            {
                System.Diagnostics.Debug.WriteLine("ge is null here, die");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       ResultNum = resultSchema.Count(),
                       GoodEntities = resultSchema,
                       PageNum = pn,
                       AllNum = allNum
                   }));
            }
            System.Diagnostics.Debug.WriteLine("See rs!");

            try
            {
                var rs = await goodEntities.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();
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
                        image = entity.DescribeImages[0];
                    }
                    catch
                    {
                        image = null;
                    }
                    decimal min_price = entity.SaleEntities.Select(se => se.Price).Min();
                    resultSchema.Add(new GoodEntitySchema()
                    {
                        GoodName = entity.GoodName,
                        GoodEntityID = entity.GoodEntityID,
                        DetailImages = image,
                        GoodPrice = min_price,

                    });

                    // load attrs

                }



                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                    new
                    {
                        ResultNum = resultSchema.Count(),
                        GoodEntities = resultSchema,
                        PageNum = pn,
                        AllNum = allNum
                    }));
            } catch (InvalidOperationException)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       ResultNum = resultSchema.Count(),
                       GoodEntities = resultSchema,
                       PageNum = pn,
                       AllNum = allNum
                   }));
            }
            

            
            
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