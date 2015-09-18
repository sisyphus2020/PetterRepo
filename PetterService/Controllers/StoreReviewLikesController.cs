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
    public class StoreReviewLikesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreReviewLikes
        public IQueryable<StoreReviewLike> GetStoreReviewLikes()
        {
            return db.StoreReviewLikes;
        }

        // GET: api/StoreReviewLikes/5
        [ResponseType(typeof(StoreReviewLike))]
        public async Task<IHttpActionResult> GetStoreReviewLike(int id)
        {
            StoreReviewLike storeReviewLike = await db.StoreReviewLikes.FindAsync(id);
            if (storeReviewLike == null)
            {
                return NotFound();
            }

            return Ok(storeReviewLike);
        }

        // PUT: api/StoreReviewLikes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreReviewLike(int id, StoreReviewLike storeReviewLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeReviewLike.StoreReviewLikeNo)
            {
                return BadRequest();
            }

            db.Entry(storeReviewLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreReviewLikeExists(id))
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

        // POST: api/StoreReviewLikes
        [ResponseType(typeof(StoreReviewLike))]
        public async Task<IHttpActionResult> PostStoreReviewLike(StoreReviewLike storeReviewLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreReviewLikes.Add(storeReviewLike);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeReviewLike.StoreReviewLikeNo }, storeReviewLike);
        }

        // DELETE: api/StoreReviewLikes/5
        [ResponseType(typeof(StoreReviewLike))]
        public async Task<IHttpActionResult> DeleteStoreReviewLike(int id)
        {
            StoreReviewLike storeReviewLike = await db.StoreReviewLikes.FindAsync(id);
            if (storeReviewLike == null)
            {
                return NotFound();
            }

            db.StoreReviewLikes.Remove(storeReviewLike);
            await db.SaveChangesAsync();

            return Ok(storeReviewLike);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreReviewLikeExists(int id)
        {
            return db.StoreReviewLikes.Count(e => e.StoreReviewLikeNo == id) > 0;
        }
    }
}