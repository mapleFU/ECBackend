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
using ECBack.Filters;
using ECBack.Models;

namespace ECBack.Controllers
{
    /// <summary>
    /// response error
    /// https://stackoverflow.com/questions/10732644/best-practice-to-return-errors-in-asp-net-web-api
    /// 
    /// 
    /// </summary>
    
    public class GetUsersURI
    {
        public string Name { get; set; }
        public int? Pn { get; set; }
    }

    public class UsersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
        
        // GET: api/Users?name=
        [HttpGet]
        public IHttpActionResult GetAll([FromUri] string Name)
        {
            if (Name == null)
            {
                // https://stackoverflow.com/questions/9454811/which-http-status-code-to-use-for-required-parameters-not-provided
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var usr = db.Users.Include(u => u.Addresses).First(u => u.NickName == Name);
            if (usr == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            } 

            return Ok(usr);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user =  db.Users.Include(u => u.Addresses).First(u => u.UserID == id);
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // https://stackoverflow.com/questions/21758615/why-should-i-use-ihttpactionresult-instead-of-httpresponsemessage
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var usr = await db.Users.FirstAsync(u => u.UserID == user.UserID);
            if (usr != null)
            {
                // 303 redirect
                usr = user;
                await db.SaveChangesAsync();
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("/api/users/" + user.UserID);
                return ResponseMessage(response);
            } else
            {
                return Redirect("/api/register");
            }
            //db.Users.Add(user);
            //await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        //[ResponseType(typeof(User))]
        //public async Task<IHttpActionResult> DeleteUser(int id)
        //{
        //    User user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();

        //    return Ok(user);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}