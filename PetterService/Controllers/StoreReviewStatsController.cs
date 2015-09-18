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
    public class StoreReviewStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreReviewStats
        public IQueryable<StoreReviewStats> GetStoreReviewStats()
        {
            return db.StoreReviewStats;
        }

        // GET: api/StoreReviewStats/5
        [ResponseType(typeof(StoreReviewStats))]
        public async Task<IHttpActionResult> GetStoreReviewStats(int id)
        {
            StoreReviewStats storeReviewStats = await db.StoreReviewStats.FindAsync(id);
            if (storeReviewStats == null)
            {
                return NotFound();
            }

            return Ok(storeReviewStats);
        }

        // PUT: api/StoreReviewStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreReviewStats(int id, StoreReviewStats storeReviewStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeReviewStats.StoreReviewStatsNo)
            {
                return BadRequest();
            }

            db.Entry(storeReviewStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreReviewStatsExists(id))
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

        // POST: api/StoreReviewStats
        [ResponseType(typeof(StoreReviewStats))]
        public async Task<IHttpActionResult> PostStoreReviewStats(StoreReviewStats storeReviewStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreReviewStats.Add(storeReviewStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeReviewStats.StoreReviewStatsNo }, storeReviewStats);
        }

        // DELETE: api/StoreReviewStats/5
        [ResponseType(typeof(StoreReviewStats))]
        public async Task<IHttpActionResult> DeleteStoreReviewStats(int id)
        {
            StoreReviewStats storeReviewStats = await db.StoreReviewStats.FindAsync(id);
            if (storeReviewStats == null)
            {
                return NotFound();
            }

            db.StoreReviewStats.Remove(storeReviewStats);
            await db.SaveChangesAsync();

            return Ok(storeReviewStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreReviewStatsExists(int id)
        {
            return db.StoreReviewStats.Count(e => e.StoreReviewStatsNo == id) > 0;
        }
    }
}