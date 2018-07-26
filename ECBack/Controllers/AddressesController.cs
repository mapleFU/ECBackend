using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CreateAddressRequestData
    {
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Block { get; set; }
        
        public string DetailAddress { get; set; }

        public bool? IsDefault { get; set; }

    }
    /// <summary>
    /// 表示地址的CONTROLLER
    /// </summary>
    public class AddressesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        /// <summary>
        ///  验证自身是否对 User 有操作权
        /// </summary>
        /// <param name="UserID"></param>
        [NonAction]
        private void VerifyUser(int UserID)
        {
            User requestUser = (User)HttpContext.Current.User;
            if (requestUser.UserID != UserID)
            {
                throw new HttpException(403, "You can't view other's information");
            }
        }

        [NonAction]
        private async Task<User> GetUser(int UserID)
        {
            var usr = await db.Users.FindAsync(UserID);
            if (usr == null)
            {
                throw new HttpException(404, "User not found");
            }
            return usr;
        }

        

        [Route("api/Addresses")]
        [HttpGet]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> ReceiveAddresses()
        {
            // load user
            var usr = (User)HttpContext.Current.User;
            if (usr == null)
            {
                return BadRequest();
            }
            System.Diagnostics.Debug.WriteLine("User name: " + usr.NickName);
            // usr is not null 
            var addresses = await db.Addresses.Where(add => add.UserID == usr.UserID).ToListAsync();
            
            return Ok(addresses);
        }

        /// <summary>
        ///  add address
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("api/Addresses")]
        [HttpPost]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> PostAddresses([FromBody] CreateAddressRequestData data)
        {
            // DEBUG 
            if (data == null)
            {
                System.Diagnostics.Debug.WriteLine("Sorry, Your fucking data is null ");
            } else
            {
                System.Diagnostics.Debug.WriteLine("data is not null");
            }

            // load user
            var usr = (User)HttpContext.Current.User;
            if (usr == null)
            {
                System.Diagnostics.Debug.WriteLine("Usr here is null");
            } else
            {
                System.Diagnostics.Debug.WriteLine("Hey " + usr.NickName);
            }
            Address newAddress = new Address()
            {
                ReceiverName = data.ReceiverName,
                Phone = data.Phone,
                Province = data.Province,
                City = data.City,
                Block = data.Block,
                DetailAddress = data.DetailAddress,
                IsDefault = data.IsDefault ?? false,
                UserID = usr.UserID
            };
            
            db.Addresses.Add(newAddress);
            await db.SaveChangesAsync();
            string newUrl = "api/Addresses/" + newAddress.AddressID;
            var responseMessage = Request.CreateResponse(HttpStatusCode.OK, new
            {
                AddressID = newAddress.AddressID
            });
            responseMessage.Headers.Add("Location", newUrl);
            return ResponseMessage(responseMessage);
        }

        [Route("api/Addresses/{AddressID:int}")]
        [HttpPost]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> PartialUpdateAddresses(int AddressID, [FromBody] Address data)
        {
            // load user

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var usr = (User)HttpContext.Current.User;
            Address address = await db.Addresses.FindAsync(AddressID);
            if (address == null)
            {
                return NotFound();
            }
            if (address.UserID != usr.UserID)
            {
                return StatusCode((HttpStatusCode)403);
            }
            address = data;
            address.AddressID = AddressID;
            
            await db.SaveChangesAsync();
            return Ok();
        }

        [Route("api/Addresses/{AddressID:int}")]
        [HttpDelete]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> RemoveAddress( int AddressID)
        {
            var usr = (User)HttpContext.Current.User;
            var add = await db.Addresses.FindAsync(AddressID);
            if (add == null)
            {
                return NotFound();
            } 
            if (add.UserID != usr.UserID)
            {
                return StatusCode((HttpStatusCode)403);
            }
            db.Addresses.Remove(add);
            await db.SaveChangesAsync();
            return Ok();
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int id)
        {
            return db.Addresses.Count(e => e.AddressID == id) > 0;
        }
    }
}