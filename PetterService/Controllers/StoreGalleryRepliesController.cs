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
using PetterService.Models;

namespace PetterService.Controllers
{
    public class StoreGalleryRepliesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleryReplies
        public IQueryable<StoreGalleryReply> GetStoreGalleryReplies()
        {
            return db.StoreGalleryReplies;
        }

        // GET: api/StoreGalleryReplies/5
        [ResponseType(typeof(StoreGalleryReply))]
        public async Task<IHttpActionResult> GetStoreGalleryReply(int id)
        {
            StoreGalleryReply storeGalleryReply = await db.StoreGalleryReplies.FindAsync(id);
            if (storeGalleryReply == null)
            {
                return NotFound();
            }

            return Ok(storeGalleryReply);
        }

        // PUT: api/StoreGalleryReplies/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreGalleryReply(int id, StoreGalleryReply storeGalleryReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeGalleryReply.StoreGalleryReplyNo)
            {
                return BadRequest();
            }

            db.Entry(storeGalleryReply).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGalleryReplyExists(id))
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

        // POST: api/StoreGalleryReplies
        [ResponseType(typeof(StoreGalleryReply))]
        public async Task<IHttpActionResult> PostStoreGalleryReply(StoreGalleryReply storeGalleryReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreGalleryReplies.Add(storeGalleryReply);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeGalleryReply.StoreGalleryReplyNo }, storeGalleryReply);
        }

        // DELETE: api/StoreGalleryReplies/5
        [ResponseType(typeof(StoreGalleryReply))]
        public async Task<IHttpActionResult> DeleteStoreGalleryReply(int id)
        {
            StoreGalleryReply storeGalleryReply = await db.StoreGalleryReplies.FindAsync(id);
            if (storeGalleryReply == null)
            {
                return NotFound();
            }

            db.StoreGalleryReplies.Remove(storeGalleryReply);
            await db.SaveChangesAsync();

            return Ok(storeGalleryReply);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGalleryReplyExists(int id)
        {
            return db.StoreGalleryReplies.Count(e => e.StoreGalleryReplyNo == id) > 0;
        }
    }
}