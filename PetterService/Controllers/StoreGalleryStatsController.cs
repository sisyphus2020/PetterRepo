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
    public class StoreGalleryStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleryStats
        public IQueryable<StoreGalleryStats> GetStoreGalleryStats()
        {
            return db.StoreGalleryStats;
        }

        // GET: api/StoreGalleryStats/5
        [ResponseType(typeof(StoreGalleryStats))]
        public async Task<IHttpActionResult> GetStoreGalleryStats(int id)
        {
            StoreGalleryStats storeGalleryStats = await db.StoreGalleryStats.FindAsync(id);
            if (storeGalleryStats == null)
            {
                return NotFound();
            }

            return Ok(storeGalleryStats);
        }

        // PUT: api/StoreGalleryStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreGalleryStats(int id, StoreGalleryStats storeGalleryStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeGalleryStats.StoreGalleryStatsNo)
            {
                return BadRequest();
            }

            db.Entry(storeGalleryStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGalleryStatsExists(id))
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

        // POST: api/StoreGalleryStats
        [ResponseType(typeof(StoreGalleryStats))]
        public async Task<IHttpActionResult> PostStoreGalleryStats(StoreGalleryStats storeGalleryStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreGalleryStats.Add(storeGalleryStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeGalleryStats.StoreGalleryStatsNo }, storeGalleryStats);
        }

        // DELETE: api/StoreGalleryStats/5
        [ResponseType(typeof(StoreGalleryStats))]
        public async Task<IHttpActionResult> DeleteStoreGalleryStats(int id)
        {
            StoreGalleryStats storeGalleryStats = await db.StoreGalleryStats.FindAsync(id);
            if (storeGalleryStats == null)
            {
                return NotFound();
            }

            db.StoreGalleryStats.Remove(storeGalleryStats);
            await db.SaveChangesAsync();

            return Ok(storeGalleryStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGalleryStatsExists(int id)
        {
            return db.StoreGalleryStats.Count(e => e.StoreGalleryStatsNo == id) > 0;
        }
    }
}