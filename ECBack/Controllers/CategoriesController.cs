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
    /// <summary>
    /// 标识Category的查询类型
    /// </summary>
    public class CategoryQuery
    {
        /// <summary>
        /// KeyWord
        /// </summary>
        public string Kw { get; set; }

        public int? Pn { get; set;}
    }
    public class CategoriesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        /// <summary>
        /// 单个页面的记录数目
        /// 用KW 查询
        /// </summary>
        
        // 默认15条
        private const int PageDataNumber = 15;
        // GET: api/Categories
        [HttpGet]
        [Route("api/Categories")]
        public async Task<IHttpActionResult> GetCategories([FromBody] CategoryQuery data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (data.Kw == null)
            {
                return BadRequest("No Kw in your URI");
            }
            int pn = data.Pn ?? 1 ;
            IQueryable<Category> categories;
            if (data.Kw == null)
            {
                categories =  db.Categories;
            } else
            {
                categories = db.Categories.Where(u => u.Name.ToLower().Contains(data.Kw.ToLower()));
            }
            var rs = await categories.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    result_num = rs.Count(),
                    categories = rs,
                    page_num = pn
                }));
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [AuthenticationFilter]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}