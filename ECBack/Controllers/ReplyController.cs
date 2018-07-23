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
using Newtonsoft.Json.Linq;

namespace ECBack.Controllers
{
    public class ReplyController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        //GET:api/ReplyEntities
        public IQueryable<Reply> GetReplies()
        {
            return db.Replies;
        }
     
        /// <summary>
        /// 某个特定问题的所有回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //GET:api/Replies/5
      //  public IQueryable<Reply>GetReplies(int id)//回复要不要分页呢，不分页的
      //  {

           // return db.Replies.Where(u => u.QuestionID == id);
                    
      //  }



        //PUT:api/Replies/5
       // [ResponseType(typeof(void))]
      //  public async Task<IHttpActionResult> PutReply(int id, Reply reply)
       // {
         //   if (!ModelState.IsValid)
       //     {
          //      return BadRequest(ModelState);
         //   }
          //  if (id != reply.ReplyID)
          //  {
          //      return BadRequest();
         //   }
         //   db.Entry(reply).State = EntityState.Modified;
         //   try
          //  {
          //      await db.SaveChangesAsync();
         //   }
         //   catch (DbUpdateConcurrencyException)
          //  {
          //      if (!ReplyExists(id))
          //      {
           //         return NotFound();
           //     }
            //    else
           //     {
           //         throw;
           //     }
          //  }
            //return StatusCode(HttpStatusCode.NoContent);
      //  }


         //添加回复
        //POST:api/Replies


        [Route("api/Replies")]
        [HttpPost]      
        public HttpResponseMessage PostReply([FromBody]JObject obj)//添加：问题id&回复内容
        {
            HttpResponseMessage response;
            int question_id = int.Parse(obj["Question_id"].ToString());
            string detail = obj["ReplyDeatil"].ToString();
            Question question = db.Questions.Find(question_id);
            if (question == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the question not exists");
            }
            else
            {
               
                Reply reply = new Reply();
                reply.ReplyDetail = detail;
                reply.UserReplyTime = DateTime.Now;
                db.Replies.Add(reply);
                question.Replies.Add(reply);//这玩意加到问题的list
                db.SaveChangesAsync();
                response = Request.CreateResponse(HttpStatusCode.OK, "Created");
            }

            return response;
        }

        //Delete:api/Replies/5
      
       // [ResponseType(typeof(Reply))]
        //public async Task<IHttpActionResult> DeleteReply(int id)
       // {
        //    Reply reply = await db.Replies.FindAsync(id);
         //   if (reply == null)
          //      return NotFound();
           // db.Replies.Remove(reply);
          //  await db.SaveChangesAsync();

          //  return Ok(reply);

       // }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReplyExists(int id)//
        {
            return db.Replies.Count(e => e.ReplyID == id) > 0;
        }

        private int ReplyAmount(int id)//
        {
            return db.Replies.Count(e => e.ReplyID == id);
        }

     
    }
}
