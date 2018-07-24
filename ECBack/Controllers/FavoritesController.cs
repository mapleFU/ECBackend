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
using ECBack.Models;
using ECBack.Filters;
using Newtonsoft.Json.Linq;

namespace ECBack.Controllers
{
    public class FavoritesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Favorites
        public IQueryable<Favorite> GetFavorites()
        {
            return db.Favorites;
        }

        // GET: api/Favorites/5
        [ResponseType(typeof(Favorite))]
        public async Task<IHttpActionResult> GetFavorite(int id)
        {
            Favorite favorite = await db.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            return Ok(favorite);
        }

        // PUT: api/Favorites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFavorite(int id, Favorite favorite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != favorite.FavoriteID)
            {
                return BadRequest();
            }

            db.Entry(favorite).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
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

        /// <summary>
        /// Create a Favorite
        /// </summary>
        /// <param GoodID></param>
        /// <returns></returns>
        [AuthenticationFilter]
        [HttpPost]
        [Route("api/Favorites")]
        public HttpResponseMessage PostFavorite([FromBody] int GoodID)
        {
            if (HttpContext.Current.User == null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Carts Null");
                return Request.CreateResponse((HttpStatusCode)403);
            }
            User requestUser = (User)HttpContext.Current.User;
            int user_id = requestUser.UserID;
            int good_id = GoodID;

            HttpResponseMessage response;

            User user = db.Users.Find(user_id);
            GoodEntity good = db.GoodEntities.Find(good_id);
            if (user == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the user not exists");
            }
            else if (good == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the GoodEntity not exists");
            }
            else
            {
                Favorite favorite = new Favorite();
                favorite.GoodEntity = good;
                favorite.GoodEntityID = good_id;
                favorite.User = user;
                favorite.UserID = user_id;
                db.Favorites.Add(favorite);
                response = Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            db.SaveChanges();
            return response;
        }

        // DELETE: api/Favorites/5
        [ResponseType(typeof(Favorite))]
        public async Task<IHttpActionResult> DeleteFavorite(int id)
        {
            Favorite favorite = await db.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            db.Favorites.Remove(favorite);
            await db.SaveChangesAsync();

            return Ok(favorite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FavoriteExists(int id)
        {
            return db.Favorites.Count(e => e.FavoriteID == id) > 0;
        }
    }
}