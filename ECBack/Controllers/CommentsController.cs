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
    public class CommentsQuery
    {
        public int GoodID { get; set; }
        public int? Pn { get; set; }
    }
    public class CommentsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        private const int PageDataNumber = 15;
        /// <summary>
        /// 获取特定商品特定页
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Comments")]
        public async Task<IHttpActionResult> GetRelatedEntities([FromUri] CommentsQuery data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int pn = data.Pn ?? 1;
            IQueryable<Comment> Comments;
            var cate = await db.SaleEntities.FindAsync(data.GoodID);
            db.Entry(cate).Reference(c => c.Comments).Load();
            Comments = cate.Comments.AsQueryable();

            var rs = await Comments.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    ResultNum = rs.Count(),
                    Comments = rs,
                    PageNum = pn
                }));
        }
        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(int GoodID, int UserID)
        {
            IQueryable<Comment> Comments;
            var cate = db.SaleEntities.Find(GoodID);
            db.Entry(cate).Reference(c => c.Comments).Load();
            Comments = cate.Comments.AsQueryable();
            var comments = Comments.ToList();
            Comment comment = null;
            foreach (var VARIABLE in comments)
            {
                if (VARIABLE.UserID == UserID)
                    comment = VARIABLE;
            }
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.CommentID)
            {
                return BadRequest();
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        [ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Comments.Add(comment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment.CommentID }, comment);
        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.CommentID == id) > 0;
        }
    }
}