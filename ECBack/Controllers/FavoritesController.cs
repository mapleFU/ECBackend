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
    public class Schema
    {
        public int GoodEntityID { get; set; }
        public string GoodName { get; set; }
        public string ImageURL { get; set; }
        public decimal GoodPrice { get; set; }
    }
    public class FavoritesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [AuthenticationFilter]
        [HttpGet]
        [Route("api/Favorites")]
        public IHttpActionResult GetRelatedFavorites()
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (HttpContext.Current.User == null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Carts Null");
                return ResponseMessage(Request.CreateResponse((HttpStatusCode)403));
            }
            
            List<Schema> Schemas = new List<Schema>();
            IQueryable<Favorite> Favs = db.Favorites;
            List<GoodEntity> goods = new List<GoodEntity>();
            User requestUser = (User)HttpContext.Current.User;
            try
            {
                //找到对应UserID的Favorite
                IEnumerable<Favorite> query = Favs.Where(w => w.UserID == requestUser.UserID);
               
                //找到对应的GoodEntity
                foreach (var VARIABLE in query)
                {
                    goods.Add(VARIABLE.GoodEntity);
                }
                //沈sb需要的类List
                foreach (var VARIABLE in goods)
                {
                    Schema tmp = new Schema();
                    tmp.GoodEntityID = VARIABLE.GoodEntityID;
                    tmp.GoodName = VARIABLE.GoodName;
                    tmp.GoodPrice = VARIABLE.SaleEntities.First().Price;
                    tmp.ImageURL = VARIABLE.Images.First().ImageURL;
                    Schemas.Add(tmp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,Schemas));
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
                System.Diagnostics.Debug.WriteLine("Get Favorites Null");
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
        /// <summary>
        /// 根据UserID和GoodID删除收藏
        /// </summary>
        /// <param name="GoodID"></param>
        /// <returns></returns>
        [AuthenticationFilter]
        [HttpDelete]
        [Route("api/Favorites")]
        public HttpResponseMessage DeleteFavorite([FromBody] int GoodID)
        {
            if (HttpContext.Current.User == null)
            {
                // 无权
                System.Diagnostics.Debug.WriteLine("Get Favorites Null");
                return Request.CreateResponse((HttpStatusCode)403);
            }
            User requestUser = (User)HttpContext.Current.User;
            int user_id = requestUser.UserID;
            int good_id = GoodID;
           
            Favorite favorite=new Favorite();
            favorite.FavoriteID = -1;
            List <Favorite>favs= db.Favorites.ToList();
            for(int i=0;i<favs.Count();i++)
            {
                if (favs[i].UserID == user_id && favs[i].GoodEntityID == good_id)
                {
                    favorite = favs[i];
                    break;
                }
            }
            if (!FavoriteExists(favorite.FavoriteID))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "favorite doesn't exist.");
            }
            db.Favorites.Remove(favorite);
            db.SaveChanges();

            return Request.CreateErrorResponse(HttpStatusCode.OK,"deleted");
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