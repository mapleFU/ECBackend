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
    public class CartRequest
    {
        public int? Number { get; set; }
    }
    /// <summary>
    /// Page URI
    /// </summary>
    public class QueryPageURI
    {
        public bool? UsePag { get; set; }
        public int? Pn { get; set; }
    }
    public class CartsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        //[NonAction]
        //private void VerifyUser(int UserID)
        //{
        //    User requestUser = (User)HttpContext.Current.User;
        //    if (requestUser.UserID != UserID)
        //    {
        //        throw new HttpException(403, "You can't view other's information");
        //    }
        //}

        [AuthenticationFilter]
        [HttpDelete]
        [Route("api/Carts/SaleEntities/{GoodID:int}")]
        public async Task<IHttpActionResult> DeleteSaleEntity(int GoodID, [FromBody] CartRequest cartRequest)
        {
            var usr = (User)HttpContext.Current.User;
            
            int deleteNum = -1;
            if (cartRequest != null && cartRequest.Number != null)
            {
                deleteNum = cartRequest.Number ?? -1;
            }
            SaleEntity saleEntity = await db.SaleEntities.FindAsync(GoodID);
            if (saleEntity == null)
            {
                
                return NotFound();
            }
            
            CartRecord record;

            try
            {
                record = await db.CartRecords.FirstAsync(u => u.SaleEntityID == saleEntity.SaleEntityID);
                
            }
            catch (InvalidOperationException _)
            {
                return NotFound();
            }
            if (deleteNum == -1)
            {
                db.CartRecords.Remove(record);
            } else
            {
                record.RecordNum -= deleteNum;
                if (record.RecordNum <= 0)
                {
                    db.CartRecords.Remove(record);
                }
            }
            
            await db.SaveChangesAsync();
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        [AuthenticationFilter]
        [HttpGet]
        [Route("api/Carts/Count")]
        public async Task<IHttpActionResult> GetCount()
        {
            if (HttpContext.Current.User == null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Carts Null");
                return ResponseMessage(Request.CreateResponse((HttpStatusCode)403));
            }
            User requestUser = (User)HttpContext.Current.User;
            var cart = await db.Carts.FindAsync(requestUser.UserID);
            // Load data
            await db.Entry(cart).Collection(c => c.CartRecords).LoadAsync();
            
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new
            {
                EntityNumber = cart.CartRecords.Count(),
            }));
        }
            

        // GET: api/Carts
        [AuthenticationFilter]
        [HttpGet]
        public async Task<IHttpActionResult> GetCarts([FromUri] QueryPageURI queryPageURI)
        {
            
            if (HttpContext.Current.User == null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Carts Null");
                return ResponseMessage(Request.CreateResponse((HttpStatusCode)403));
            }
            User requestUser = (User)HttpContext.Current.User;
            System.Diagnostics.Debug.WriteLine("GetCarts has:" + requestUser.NickName);

            //var refer = db.Entry(requestUser).Reference(u => (u as User).Cart);
            //if (!refer.IsLoaded)
            //{
            //    System.Diagnostics.Debug.WriteLine("Not loaded");
            //    await refer.LoadAsync();
            //} else
            //{
            //    System.Diagnostics.Debug.WriteLine("Loaded");
            //}
            var cart = await db.Carts.FindAsync(requestUser.UserID);
            // Load data
            await db.Entry(cart).Collection(c => c.CartRecords).LoadAsync();
            foreach (var rec in cart.CartRecords)
            {
                await db.Entry(rec).Reference(record => record.SaleEntity).LoadAsync();
            }
            // await db.Entry(requestUser).Reference(u => u.Cart).LoadAsync();
            return Ok(cart);
        }

        [AuthenticationFilter]
        [Route("api/Carts/SaleEntities/{GoodID:int}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddToCart(int GoodID, [FromBody] CartRequest cartRequest)
        {
            var usr = (User)HttpContext.Current.User;
            System.Diagnostics.Debug.WriteLine("Enter the function");
            int addNumber = 1;
            if (cartRequest != null && cartRequest.Number != null)
            {
                addNumber = cartRequest.Number ?? 1;
            }
            SaleEntity saleEntity = await db.SaleEntities.FindAsync(GoodID);
            if (saleEntity == null)
            {
                System.Diagnostics.Debug.WriteLine("Not found saleEntity");
                return NotFound();
            }
            System.Diagnostics.Debug.WriteLine("GetCarts has:" + usr.NickName);
            CartRecord record;

            try
            {
                record = await db.CartRecords.FirstAsync(u => u.SaleEntityID == saleEntity.SaleEntityID);
                System.Diagnostics.Debug.WriteLine("Found Record.");
            } catch (InvalidOperationException _)
            {
                System.Diagnostics.Debug.WriteLine("Record not found, so create it");
                record = new CartRecord()
                {
                    RecordNum = 0,
                    UserID = usr.UserID,
                    SaleEntityID = GoodID
                };
                db.CartRecords.Add(record);
            }
            
            record.RecordNum += addNumber;
            await db.SaveChangesAsync();
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Carts.Count(e => e.UserID == id) > 0;
        }
    }
}