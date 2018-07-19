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
    public class CommentInfoesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/CommentInfoes
        public IQueryable<CommentInfo> GetCommentInfoes()
        {
            return db.CommentInfoes;
        }

        // GET: api/CommentInfoes/5
        [ResponseType(typeof(CommentInfo))]
        public async Task<IHttpActionResult> GetCommentInfo(int id)
        {
            CommentInfo commentInfo = await db.CommentInfoes.FindAsync(id);
            if (commentInfo == null)
            {
                return NotFound();
            }

            return Ok(commentInfo);
        }

        // PUT: api/CommentInfoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommentInfo(int id, CommentInfo commentInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentInfo.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentInfo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentInfoExists(id))
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

        // POST: api/CommentInfoes
        [ResponseType(typeof(CommentInfo))]
        public async Task<IHttpActionResult> PostCommentInfo(CommentInfo commentInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CommentInfoes.Add(commentInfo);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CommentInfoExists(commentInfo.CommentID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = commentInfo.CommentID }, commentInfo);
        }

        // DELETE: api/CommentInfoes/5
        [ResponseType(typeof(CommentInfo))]
        public async Task<IHttpActionResult> DeleteCommentInfo(int id)
        {
            CommentInfo commentInfo = await db.CommentInfoes.FindAsync(id);
            if (commentInfo == null)
            {
                return NotFound();
            }

            db.CommentInfoes.Remove(commentInfo);
            await db.SaveChangesAsync();

            return Ok(commentInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentInfoExists(int id)
        {
            return db.CommentInfoes.Count(e => e.CommentID == id) > 0;
        }
    }
}