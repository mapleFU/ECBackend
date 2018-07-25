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
using Qiniu.Util;

namespace ECBack.Controllers
{
    public class ImagesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        private static string AccessKey = "X4pHLlBJDRBSCEyWZ0Lfmbx_aftvCbRUVnsW-zDp";
        private static string SecretKey = "MAgFx6Q9PLLXRBuYu79rVkv7Lgi8LoFUZYwqPzjd";

        private static Mac mac = new Mac(AccessKey, SecretKey);
        
        

        // POST: api/Images

        public IHttpActionResult PostImage([FromBody] HttpPostedFileBase image)
        {
            System.Diagnostics.Debug.WriteLine("Allow");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            System.Diagnostics.Debug.WriteLine("Allow-Model, ready to update");
            string uri = ImageFileManager.Upload(image);



            return Ok(uri);
        }

        // DELETE: api/Images/5
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> DeleteImage(int id)
        {
            Image image = await db.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            db.Images.Remove(image);
            await db.SaveChangesAsync();

            return Ok(image);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int id)
        {
            return db.Images.Count(e => e.ImageID == id) > 0;
        }
    }
}