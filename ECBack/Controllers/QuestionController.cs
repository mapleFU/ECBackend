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

        //GET:api/QuestionEntities
        public IQueryable<Question> GetQuestions()
        {
            return db.Questions;
        }

        //GET:api/Questions/5
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult>GetQuestion(int id)
        {
            Question question = await db.Questions.FindAsync(id);
            if(question==null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        //PUT:api/Questions/5
        [ResponseType(typeof(void))]
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

       
        [Route("api/Questions")]
        [HttpPost]
        public HttpResponseMessage PostQuestion([FromBody] JObject obj )
        {
           
            int Display_id = int.Parse(obj["Display_id"].ToString());
            string QuestionDetail = obj["qu_detail"].ToString();
            HttpResponseMessage response;

            DisplayEntity Display = db.DisplayEntities.Find(Display_id);
           
            if (Display == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the DisplayEntity not exists");
            }
            else
            {
               
                Question question = new Question();
                question.Detail = QuestionDetail;
                question.DisplayEntityID = Display_id;
                db.Questions.Add(question);
              
                response = Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            db.SaveChanges();
            return response;
        }

       

        //Delete:api/Questions/5
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> DeleteAddress(int id)
        {
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
                return NotFound();
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

        private int QuestionAmount(int id)//
        {
            return db.Questions.Count(e => e.QuestionID == id);
        }

        
    }
}
