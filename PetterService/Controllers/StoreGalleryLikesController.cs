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
    public class StoreGalleryLikesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleryLikes
        public IQueryable<StoreGalleryLike> GetStoreGalleryLikes()
        {
            return db.StoreGalleryLikes;
        }

        // GET: api/StoreGalleryLikes/5
        [ResponseType(typeof(StoreGalleryLike))]
        public async Task<IHttpActionResult> GetStoreGalleryLike(int id)
        {
            StoreGalleryLike storeGalleryLike = await db.StoreGalleryLikes.FindAsync(id);
            if (storeGalleryLike == null)
            {
                return NotFound();
            }

            return Ok(storeGalleryLike);
        }

        // PUT: api/StoreGalleryLikes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreGalleryLike(int id, StoreGalleryLike storeGalleryLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeGalleryLike.StoreGalleryLikeNo)
            {
                return BadRequest();
            }

            db.Entry(storeGalleryLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGalleryLikeExists(id))
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

        // POST: api/StoreGalleryLikes
        [ResponseType(typeof(StoreGalleryLike))]
        public async Task<IHttpActionResult> PostStoreGalleryLike(StoreGalleryLike storeGalleryLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreGalleryLikes.Add(storeGalleryLike);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeGalleryLike.StoreGalleryLikeNo }, storeGalleryLike);
        }

        // DELETE: api/StoreGalleryLikes/5
        [ResponseType(typeof(StoreGalleryLike))]
        public async Task<IHttpActionResult> DeleteStoreGalleryLike(int id)
        {
            StoreGalleryLike storeGalleryLike = await db.StoreGalleryLikes.FindAsync(id);
            if (storeGalleryLike == null)
            {
                return NotFound();
            }

            db.StoreGalleryLikes.Remove(storeGalleryLike);
            await db.SaveChangesAsync();

            return Ok(storeGalleryLike);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGalleryLikeExists(int id)
        {
            return db.StoreGalleryLikes.Count(e => e.StoreGalleryLikeNo == id) > 0;
        }
    }
}