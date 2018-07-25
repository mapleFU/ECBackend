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

        public HttpResponseMessage PostImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                System.Diagnostics.Debug.WriteLine("Image enter");
                foreach (string file in httpRequest.Files)
                {
                    System.Diagnostics.Debug.WriteLine("Image is" + file);
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            string fileUrl = ImageFileManager.Upload(postedFile);
                            System.Diagnostics.Debug.WriteLine("Image is" + fileUrl);
                            //YourModelProperty.imageurl = userInfo.email_id + extension;
                            ////  where you want to attach your imageurl

                            ////if needed write the code to update the table

                            //var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + userInfo.email_id + extension);
                            ////Userimage myfolder name where i want to save my image
                            //postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
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