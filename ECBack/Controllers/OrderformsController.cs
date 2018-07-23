using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [NotMapped]
    public class SaleSingleRecord {
        public int SaleEntityID { get; set; }
        public int? Number { get; set; }

        public SaleSingleRecord()
        {
            Number = 1;
        }
    }

    public class AddPostFormRequest
    {
        public List<SaleSingleRecord> SaleSingleRecords { get; set; }
        public int AddressID { get; set; }
    }

    public class OrderformsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/User/{UserID:int}/Orderforms
        [AuthenticationFilter]
        [HttpPost]
        [Route("api/Orderforms")]
        public async Task<IHttpActionResult> AddPostForm([FromBody]AddPostFormRequest addPostFormRequest) {
            if (addPostFormRequest == null || addPostFormRequest.SaleSingleRecords == null)
            {
                return BadRequest();
            }
            var currentUsr = (User)HttpContext.Current.User;
            // TODO: find out how to cascade create!!!!
            // create orderform and it's logistic
            Orderform orderform = new Orderform();
            Logistic logistic = new Logistic();
            logistic.Orderform = orderform;
            orderform.Logistic = logistic;
            logistic.State = 0;
            
            orderform.UserID = currentUsr.UserID;
            orderform.AddressID = addPostFormRequest.AddressID;
            // orderform.AddressID
            db.Orderforms.Add(orderform);
            db.Logistics.Add(logistic);

            float totalPrice = 0;

            foreach (var record in addPostFormRequest.SaleSingleRecords)
            {
                var seRecord = new SaleEntityRecord()
                {
                    SaleEntityID = record.SaleEntityID,
                    EntityNum = record.Number ?? 1,

                };
                totalPrice += (float)(await db.SaleEntities.FindAsync(seRecord.SaleEntityID)).Price;
                db.SaleEntityRecords.Add(seRecord);

            }
            orderform.TotalPrice = totalPrice;

            await db.SaveChangesAsync();
            var resp = Request.CreateResponse(HttpStatusCode.NoContent);
            resp.Headers.Add("Location", "/api/Orderforms/" + orderform.OrderformID);
            return ResponseMessage(resp);
        }
        

        // GET: api/Orderforms/5
        [ResponseType(typeof(Orderform))]
        public async Task<IHttpActionResult> GetOrderform(int id)
        {
            Orderform orderform = await db.Orderforms.FindAsync(id);
            if (orderform == null)
            {
                return NotFound();
            }

            return Ok(orderform);
        }


        // POST: api/Orderforms
        //[AuthenticationFilter]
        //[ResponseType(typeof(Orderform))]
        //public async Task<IHttpActionResult> PostOrderform([FromBody] OrderformRequest orderformRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    Orderform orderform = new Orderform()
        //    {
                
        //    }
            
        //}

        // DELETE: api/Orderforms/5
        [ResponseType(typeof(Orderform))]
        public async Task<IHttpActionResult> DeleteOrderform(int id)
        {
            Orderform orderform = await db.Orderforms.FindAsync(id);
            if (orderform == null)
            {
                return NotFound();
            }

            db.Orderforms.Remove(orderform);
            await db.SaveChangesAsync();

            return Ok(orderform);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderformExists(int id)
        {
            return db.Orderforms.Count(e => e.OrderformID == id) > 0;
        }
    }
}