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
        public SaleSingleRecord SaleSingleRecord { get; set; }
        // public List<SaleSingleRecord> SaleSingleRecords { get; set; }
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
            if (addPostFormRequest == null || addPostFormRequest.SaleSingleRecord == null)
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
            logistic.ToAddress = (await db.Addresses.FindAsync(addPostFormRequest.AddressID)).DetailAddress;
            // orderform.AddressID
            db.Orderforms.Add(orderform);
            db.Logistics.Add(logistic);

            float totalPrice = 0;

            var record = addPostFormRequest.SaleSingleRecord;
            {
                var seRecord = new SERecord()
                {
                    SaleEntityID = record.SaleEntityID,
                    EntityNum = record.Number ?? 1,

                };
                
                orderform.SERecord = seRecord;
                var saleEntity = await db.SaleEntities.FindAsync(seRecord.SaleEntityID);
                totalPrice += (float)(saleEntity).Price;
                // load good
                // await db.Entry(saleEntity).Reference(s => s.GoodEntity).LoadAsync();
                logistic.FromAddress = (await db.GoodEntities.FindAsync(saleEntity.GoodEntityID)).SellProvince;
                
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
        [AuthenticationFilter]
        public async Task<IHttpActionResult> GetOrderform(int id)
        {
            Orderform orderform = await db.Orderforms.FindAsync(id);
            if (orderform == null)
            {
                return NotFound();
            }
            await db.Entry(orderform).Reference(odf => odf.SERecord).LoadAsync();
            return Ok(orderform);
        }

        [NonAction]
        private async Task<IHttpActionResult> SetState(int OrderformID, int state)
        {
            User usr = (User)HttpContext.Current.User;
            var orderform = await db.Orderforms.FindAsync(OrderformID);
            if (orderform == null)
            {
                return NotFound();
            }
            HttpResponseMessage httpResponse;
            if (orderform.UserID != usr.UserID)
            {
                httpResponse = Request.CreateResponse((HttpStatusCode)403, "No Author");
            }
            else
            {
                orderform.State = state;
                httpResponse = Request.CreateResponse(HttpStatusCode.NoContent);
            }

            return ResponseMessage(httpResponse);
        }

        [Route("api/Orderforms/{OrderformID:int}/Pay")]
        [AuthenticationFilter]
        [HttpPost]
        public async Task<IHttpActionResult> PayForOrderform(int OrderformID)
        {
            return await SetState(OrderformID, 1);
        }

        [Route("api/Orderforms/{OrderformID:int}/Arrived")]
        [AuthenticationFilter]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyOrderform(int OrderformID)
        {
            var result = await SetState(OrderformID ,2);
            return result;
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