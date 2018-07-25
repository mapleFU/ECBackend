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
        [Route("api/Replies")]
        [HttpGet]
        public IQueryable<Reply> GetReplies()
        {
            return db.Replies;
        }
     
        /// <summary>
        /// 某个特定问题的所有回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
      
         //添加回复
        //POST:api/Replies
        [Route("api/Replies")]
        [HttpPost]      
        public HttpResponseMessage PostReply([FromBody]JObject obj)//添加：问题id&回复内容
        {
            HttpResponseMessage response;
            int question_id = int.Parse(obj["QuestionID"].ToString());
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
                reply.QuestionID = question_id;
                db.SaveChangesAsync();
                response = Request.CreateResponse(HttpStatusCode.OK, "Created");
            }

            return response;
        }

     

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
