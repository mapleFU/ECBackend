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
    public class CommentsQuery
    {
        public int GoodID { get; set; }
        public int? Pn { get; set; }
    }

    public class CommentQuery
    {
        public int GoodID { get; set; }
        public int UserID { get; set; }
    }

    public class CommentU
    {
        public int SaleEntityID { get; set; }
        public string Detail { get; set; }
        public DateTime UserCommentTime { get; set; }
        public string UserName { get; set; }
        public int LevelRank { get; set; }
    }
    public class CommentsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        private const int PageDataNumber = 15;
        /// <summary>
        /// 获取特定商品一页评论
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Comments")]
        public HttpResponseMessage GetRelatedComments([FromUri] CommentsQuery data)
        {
            int pn = data.Pn ?? 1;

            var good = db.GoodEntities.Find(data.GoodID);
            db.Entry(good).Collection(ge => ge.SaleEntities).Load();
            List<Comment> comments = new List<Comment>();
            foreach (var se in good.SaleEntities)
            {
                db.Entry(se).Collection(saleE => saleE.Comments).Load();
                foreach (var s_comment in se.Comments)
                {
                    comments.Add(s_comment);
                }
            }

            List<CommentU> dataList = new List<CommentU>();
            var rs = comments.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToList();
            int res = comments.Count() / PageDataNumber;
            if (comments.Count() % PageDataNumber != 0)
                res = res + 1;
            foreach (var VARIABLE in rs)
            {
                CommentU tmp = new CommentU();
                tmp.SaleEntityID = VARIABLE.SaleEntityID;
                tmp.Detail = VARIABLE.Detail;
                tmp.LevelRank = VARIABLE.LevelRank;
                tmp.UserCommentTime = VARIABLE.UserCommentTime;
                IQueryable<User> users;
                users = db.Users;
                List<User> t = users.ToList();
                foreach (var va in t)
                {
                    if (va.UserID == VARIABLE.UserID)
                        tmp.UserName = va.NickName;
                }
                dataList.Add(tmp);
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    ResultNum = res,
                    Comments = dataList,
                    PageNum = pn
                });
        }

        [AuthenticationFilter]
        [Route("api/GoodEntity/{GoodID:int}/Comments")]
        public async Task<IHttpActionResult> GetGEComments(int GoodID)
        {
            if (HttpContext.Current.User.Identity != null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Favorites Null");
                return ResponseMessage( Request.CreateResponse((HttpStatusCode)403) );
            }
            var good = await db.GoodEntities.FindAsync(GoodID);
            User requestUser = (User)HttpContext.Current.User;
            await db.Entry(good).Collection(ge => ge.SaleEntities).LoadAsync();
            List<Comment> comments = new List<Comment>();
            foreach (var se in good.SaleEntities)
            {
                await db.Entry(se).Collection(saleE => saleE.Comments).LoadAsync();
                foreach (var s_comment  in se.Comments)
                {
                    comments.Add(s_comment);
                }
            }
            return Ok(comments);
        }

        /// <summary>
        /// 单个评论
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [AuthenticationFilter]
        [ResponseType(typeof(Comment))]
        [Route("api/Comments/find")]
        public HttpResponseMessage GetComment([FromUri] int GoodID)
        {
            if (HttpContext.Current.User.Identity != null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Favorites Null");
                return Request.CreateResponse((HttpStatusCode)403);
            }
            User requestUser = (User)HttpContext.Current.User;
            int user_id = requestUser.UserID;

            IQueryable<Comment> Comments;
            Comments = db.Comments;
            List<Comment> tt = Comments.ToList();


            Comment comment = null;
            foreach (var VARIABLE in tt)
            {
                if (VARIABLE.UserID == user_id && VARIABLE.SaleEntityID == GoodID)
                    comment = VARIABLE;
            }

            return Request.CreateResponse(HttpStatusCode.OK, comment);
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
        [HttpPost]
        [Route("api/Comments")]
        public IHttpActionResult PostComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IQueryable<User> users = db.Users;
            IQueryable<SaleEntity> sales = db.SaleEntities;
            IQueryable<Comment> comments = db.Comments;
            foreach (var VARIABLE in comments)
            {
                if (VARIABLE.UserID == comment.UserID && VARIABLE.SaleEntityID == comment.SaleEntityID)
                {
                    VARIABLE.Detail = comment.Detail;
                    VARIABLE.LevelRank = comment.LevelRank;
                    VARIABLE.UserCommentTime = comment.UserCommentTime;
                    db.SaveChanges();
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted,"oka"));
                }
            }
            if (users.Where(d => d.UserID == comment.UserID).Count() == 0)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, "UserID not found"));
            }
            if (sales.Where(d => d.SaleEntityID == comment.SaleEntityID).Count() == 0)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, "SaleEntityID not found"));
            }
            db.Comments.Add(comment);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, "oka"));
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