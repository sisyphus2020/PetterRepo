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
    public class StoreStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreStats
        public IQueryable<StoreStats> GetStoreStats()
        {
            return db.StoreStats;
        }

        // GET: api/StoreStats/5
        [ResponseType(typeof(StoreStats))]
        public async Task<IHttpActionResult> GetStoreStats(int id)
        {
            StoreStats StoreStats = await db.StoreStats.FindAsync(id);
            if (StoreStats == null)
            {
                return NotFound();
            }

            return Ok(StoreStats);
        }

        // PUT: api/StoreStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreStats(int id, StoreStats StoreStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != StoreStats.StoreStatsNo)
            {
                return BadRequest();
            }

            db.Entry(StoreStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreStatsExists(id))
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

        // POST: api/StoreStats
        [ResponseType(typeof(StoreStats))]
        public async Task<IHttpActionResult> PostStoreStats(StoreStats StoreStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreStats.Add(StoreStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = StoreStats.StoreStatsNo }, StoreStats);
        }

        // DELETE: api/StoreStats/5
        [ResponseType(typeof(StoreStats))]
        public async Task<IHttpActionResult> DeleteStoreStats(int id)
        {
            StoreStats StoreStats = await db.StoreStats.FindAsync(id);
            if (StoreStats == null)
            {
                return NotFound();
            }

            db.StoreStats.Remove(StoreStats);
            await db.SaveChangesAsync();

            return Ok(StoreStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreStatsExists(int id)
        {
            return db.StoreStats.Count(e => e.StoreStatsNo == id) > 0;
        }
    }
}