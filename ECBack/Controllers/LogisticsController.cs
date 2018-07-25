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
    public class LogisticsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Logistics/5
        [HttpGet]
        [ResponseType(typeof(Logistic))]
        [Route("api/Logistics/{id:int}")]
        public async Task<IHttpActionResult> GetLogistic(int id)
        {
            Logistic logistic = await db.Logistics.FindAsync(id);
            if (logistic == null)
            {
                return NotFound();
            }
            await db.Entry(logistic).Collection(log => log.LogisticInfos).LoadAsync();
           
            return Ok(logistic);
        }

        // POST: api/Logistics
        [HttpPost]
        [ResponseType(typeof(Logistic))]
        [Route("api/Logistics/{LogisticID:int}")]
        public async Task<IHttpActionResult> AddLogisticInfo(int LogisticID, [FromBody]LogisticInfo logisticInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Logistic logistic = await db.Logistics.FindAsync(LogisticID);
            if (logistic == null)
            {
                return NotFound();
            }
            logisticInfo.LogisticID = logistic.LogisticID;
            db.LogisticInfoes.Add(logisticInfo);
            await db.SaveChangesAsync();

            var httpResp = Request.CreateResponse(HttpStatusCode.NoContent);

            return ResponseMessage(httpResp);
        }

        //// DELETE: api/Logistics/5
        //[ResponseType(typeof(Logistic))]
        //public async Task<IHttpActionResult> DeleteLogistic(int id)
        //{
        //    Logistic logistic = await db.Logistics.FindAsync(id);
        //    if (logistic == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Logistics.Remove(logistic);
        //    await db.SaveChangesAsync();

        //    return Ok(logistic);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogisticExists(int id)
        {
            return db.Logistics.Count(e => e.LogisticID == id) > 0;
        }
    }
}