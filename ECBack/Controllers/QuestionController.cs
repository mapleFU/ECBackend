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
using Newtonsoft.Json.Linq;

namespace ECBack.Controllers
{
    public class QuestionController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        private const int PageDataNumber = 10;

        //GET:api/QuestionEntities
        [Route("api/Questions")]
        [HttpGet]
        public IQueryable<Question> GetQuestions()
        {
            return db.Questions;
        }

        //根据当前商品id得到当前商品的所有问题，前端传过来的是SKU1的id还有当前页数。。，然后返回所有这个商品的问题记录
        //GET:api/Questions/5
    
        [ResponseType(typeof(Question))]
        [Route("api/Questions/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetQuestion(int id,[FromUri]int pn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            IQueryable<Question> questions;
            var ques = await db.GoodEntities.FindAsync(id);
            db.Entry(ques).Collection(c => c.Questions).Load();          
            questions = ques.Questions.AsQueryable();
            foreach (var que in ques.Questions)
            {
                await db.Entry(que).Collection(a => a.Replies).LoadAsync();
            }
            var QuestionEntities = await questions.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new
            {
                PageCount=QuestionEntities.Count()/PageDataNumber,
                QuestionEntities
                
            }));
           
        }

        //PUT:api/Questions/5.......其实我感觉put没必要啊，问题提出了不能修改了
        [ResponseType(typeof(void))]
        [Route("api/Questions/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutQuestion(int id,[FromBody]Question question)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id!=question.QuestionID)
            {
                return BadRequest();
            }
            db.Entry(question).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        //添加问题的,需要有当前商品的SKU1和新添加的问题的详细信息,
        //Post:api/Questions
        [Route("api/Questions")]
        [HttpPost]
        public HttpResponseMessage PostQuestion([FromBody] JObject obj )
        {
          
            int Goodid = int.Parse(obj["GoodID"].ToString());
            string QuestionDetail = obj["QuestionDetail"].ToString();
            Question question = new Question();
            question.Detail = QuestionDetail;
            
            HttpResponseMessage response;

            //找到这个商品的东东
            GoodEntity goodEntity = db.GoodEntities.Find(Goodid);
            goodEntity.Questions.Add(question);
           
           
            if (goodEntity == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the GoodEntity not exists");
            }
            else
            {
                db.Questions.Add(question);                     
                response = Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            db.SaveChanges();
            return response;
        }

       
        //删除问题的，我觉得可能用不到就先写在这里,需要有问题的id
        //Delete:api/Questions/5
        [ResponseType(typeof(Question))]
        [Route("api/Questions/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAddress(int id)
        {
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            db.Questions.Remove(question);
            
            await db.SaveChangesAsync();

            return Ok(question);
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)//
        {
            return db.Questions.Count(e => e.QuestionID == id)>0;
        }

        private int QuestionAmount(int id)//问题数，不知道前面要不要，先放这里
        {
            return db.Questions.Count(e => e.QuestionID == id);
        }

        
    }
}
