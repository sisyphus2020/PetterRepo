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
    public class StoreNewsStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreNewsStats
        public IQueryable<StoreNewsStats> GetStoreNewsStats()
        {
            return db.StoreNewsStats;
        }

        // GET: api/StoreNewsStats/5
        [ResponseType(typeof(StoreNewsStats))]
        public async Task<IHttpActionResult> GetStoreNewsStats(int id)
        {
            StoreNewsStats StoreNewsStats = await db.StoreNewsStats.FindAsync(id);
            if (StoreNewsStats == null)
            {
                return NotFound();
            }

            return Ok(StoreNewsStats);
        }

        // PUT: api/StoreNewsStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreNewsStats(int id, StoreNewsStats StoreNewsStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != StoreNewsStats.StoreNewsStatsNo)
            {
                return BadRequest();
            }

            db.Entry(StoreNewsStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreNewsStatsExists(id))
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

        // POST: api/StoreNewsStats
        [ResponseType(typeof(StoreNewsStats))]
        public async Task<IHttpActionResult> PostStoreNewsStats(StoreNewsStats StoreNewsStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreNewsStats.Add(StoreNewsStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = StoreNewsStats.StoreNewsStatsNo }, StoreNewsStats);
        }

        // DELETE: api/StoreNewsStats/5
        [ResponseType(typeof(StoreNewsStats))]
        public async Task<IHttpActionResult> DeleteStoreNewsStats(int id)
        {
            StoreNewsStats StoreNewsStats = await db.StoreNewsStats.FindAsync(id);
            if (StoreNewsStats == null)
            {
                return NotFound();
            }

            db.StoreNewsStats.Remove(StoreNewsStats);
            await db.SaveChangesAsync();

            return Ok(StoreNewsStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreNewsStatsExists(int id)
        {
            return db.StoreNewsStats.Count(e => e.StoreNewsStatsNo == id) > 0;
        }
    }
}