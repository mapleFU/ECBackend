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


        [Route("api/Users/{UserID}/Addresses/{AddressId}")]
        [HttpGet]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> GetAddresses(int UserID, int AddressID)
        {
            // load user
            //var usr = await db.Users.FindAsync(UserID);
            //if (usr == null)
            //{
            //    return NotFound();
            //}
            VerifyUser(UserID);
            var usr = await GetUser(UserID);
            
            // usr is not null 
            
            await db.Entry(usr).Collection(u => u.Addresses).LoadAsync();
            return Ok(usr.Addresses);
        }

        [Route("api/Users/{UserID}/Addresses")]
        [HttpPost]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> PostAddresses(int UserID, [FromBody] Address data)
        {
            // load user
            var usr = await GetUser(UserID);
            VerifyUser(UserID);

            data.UserID = usr.UserID;
            db.Addresses.Add(data);
            await db.SaveChangesAsync();
            string newUrl = "api/Users/" + usr.UserID + "/Addresses/" + data.AddressID;
            var responseMessage = Request.CreateResponse(HttpStatusCode.NoContent);
            responseMessage.Headers.Add("Location", newUrl);
            return ResponseMessage(responseMessage);
        }

        [Route("api/Users/{UserID:int}/Addresses/{AddressID:int}")]
        [HttpPost]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> PartialUpdateAddresses(int UserID, int AddressID, [FromBody] Address data)
        {
            // load user

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VerifyUser(UserID);
            var usr = await GetUser(UserID);
            Address address = await db.Addresses.FindAsync(AddressID);
            if (address == null)
            {
                return NotFound();
            }
            address = data;
            address.AddressID = AddressID;
            
            await db.SaveChangesAsync();
            return Ok();
        }

        [Route("api/Users/{UserID:int}/Addresses/{AddressID:int}")]
        [HttpPost]
        [AuthenticationFilter]
        public async Task<IHttpActionResult> DeleteAddress(int UserID, int AddressID)
        {
            VerifyUser(UserID);
            var add = await db.Addresses.FindAsync(AddressID);
            if (add == null)
            {
                return NotFound();
            } else
            {
                db.Addresses.Remove(add);
                return Ok();
            }
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