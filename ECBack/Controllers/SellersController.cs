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
    public class AddSellerSchema
    {
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class SellersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/Sellers/Register")]
        public async Task<IHttpActionResult> RegisterSeller([FromBody] AddSellerSchema seller)
        {
            System.Diagnostics.Debug.WriteLine("进来了");
            if (seller == null || seller.PhoneNumber == null || seller.Password == null)
            {
                
                System.Diagnostics.Debug.WriteLine("is null");
                return BadRequest(ModelState);
            }
            string phoneNumber = seller.PhoneNumber;
            try
            {
                System.Diagnostics.Debug.WriteLine("准备去重");
                //var duplicate = db.Users.First(x => x.PhoneNumber == registerData.PhoneNumber || x.NickName == registerData.NickName)
                //    ;
                var exist = await db.Sellers.AnyAsync(s => s.PhoneNumber == phoneNumber);
                if (exist)
                {
                    System.Diagnostics.Debug.WriteLine("电话查找");
                    return StatusCode(HttpStatusCode.Conflict);
                } else
                {
                    System.Diagnostics.Debug.WriteLine("meiyou");
                    var newSeller = new Seller()
                    {
                        PhoneNumber = seller.PhoneNumber,
                        PasswordHash = seller.Password
                    };
                    db.Sellers.Add(newSeller);
                    await db.SaveChangesAsync();
                    return CreatedAtRoute("DefaultApi", new { id = newSeller.SellerID }, seller);
                }
            } catch (InvalidOperationException)
            {
                System.Diagnostics.Debug.WriteLine("meiyou");
                var newSeller = new Seller()
                {
                    PhoneNumber = seller.PhoneNumber,
                    PasswordHash = seller.Password
                };
                db.Sellers.Add(newSeller);
                await db.SaveChangesAsync();
                return CreatedAtRoute("DefaultApi", new { id = newSeller.SellerID }, seller);
            }
        }

        [HttpGet]
        [Route("api/Sellers/{SellerID:int}/GoodEntities")]
        public async Task<IHttpActionResult> GetGoodEntities(int SellerID)
        {
            var seller = await db.Sellers.FindAsync(SellerID);
            if (seller == null)
            {
                return NotFound();
            }
            await db.Entry(seller).Collection(s => s.GoodEntities).LoadAsync();
            return Ok(seller);
        }

        // GET: api/Sellers/5
        [ResponseType(typeof(Seller))]
        public async Task<IHttpActionResult> GetSeller(int id)
        {
            Seller seller = await db.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        // PUT: api/Sellers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSeller(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seller.SellerID)
            {
                return BadRequest();
            }

            db.Entry(seller).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellerExists(id))
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


        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SellerExists(int id)
        {
            return db.Sellers.Count(e => e.SellerID == id) > 0;
        }
    }
}