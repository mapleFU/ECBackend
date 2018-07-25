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
        // public SaleSingleRecord SaleSingleRecord { get; set; }
        public List<SaleSingleRecord> SaleSingleRecords { get; set; }
        public int AddressID { get; set; }
    }

    public class OrderformsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        /// <summary>
        /// 单个页面需要
        /// </summary>
        private const int SinglePageQuery = 6;

        [AuthenticationFilter]
        [HttpGet]
        [Route("api/Orderforms")]
        public async Task<IHttpActionResult> ViewOrderform([FromUri] QueryPageURI queryData)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            bool usePn = true;
            if (queryData == null || queryData.UsePag == false)
            {
                usePn = false;
            }

            var usr = (User)HttpContext.Current.User;
            var entities = db.Orderforms.Where(odf => odf.UserID == usr.UserID).OrderBy(odf => odf.TransacDateTime);
            if (usePn)
            {
                int pn = queryData.Pn ?? 1;
                var results = await entities.Skip((pn - 1) * SinglePageQuery).Take(SinglePageQuery).ToListAsync();
                return Ok(new
                {
                    Orderforms = results,
                    ResultNums = results.Count(),
                    Pn = pn
                });
            } else
            {
                return Ok(new
                {
                    Orderforms = entities,
                    ResultNums = entities.Count()
                });
            }
            // await goodEntities.Skip((pn - 1) * PageDataNumber).Take(PageDataNumber).ToListAsync();
            
        }

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
            var address = await db.Addresses.FindAsync(addPostFormRequest.AddressID);
            if (address == null)
            {
                return NotFound();
            }
            logistic.ToAddress = address.DetailAddress;
            // orderform.AddressID
            db.Orderforms.Add(orderform);
            db.Logistics.Add(logistic);

            float totalPrice = 0;
            System.Diagnostics.Debug.WriteLine("Get has:" + currentUsr.NickName);
            foreach (var record in addPostFormRequest.SaleSingleRecords)
            {
                var seRecord = new SERecord()
                {
                    SaleEntityID = record.SaleEntityID,
                    EntityNum = record.Number ?? 1,

                };
                System.Diagnostics.Debug.WriteLine("ADD ONE");
                seRecord.OrderformID = orderform.OrderformID;
                seRecord.SaleEntityID = seRecord.SaleEntityID;
                // orderform.SERecord = seRecord;
                var saleEntity = await db.SaleEntities.FindAsync(seRecord.SaleEntityID);
                totalPrice += (float)(saleEntity).Price;
                // load good
                // await db.Entry(saleEntity).Reference(s => s.GoodEntity).LoadAsync();
                logistic.FromAddress = (await db.GoodEntities.FindAsync(saleEntity.GoodEntityID)).SellProvince;
                
                db.SaleEntityRecords.Add(seRecord);

            }
            orderform.TotalPrice = totalPrice;
            System.Diagnostics.Debug.WriteLine("Total Price " + totalPrice);
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
            await db.Entry(orderform).Collection(odf => odf.SERecords).LoadAsync();
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