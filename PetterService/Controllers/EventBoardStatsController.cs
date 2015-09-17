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
    public class EventBoardStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/EventBoardStats
        public IQueryable<EventBoardStats> GetEventBoardStats()
        {
            return db.EventBoardStats;
        }

        // GET: api/EventBoardStats/5
        [ResponseType(typeof(EventBoardStats))]
        public async Task<IHttpActionResult> GetEventBoardStats(int id)
        {
            EventBoardStats eventBoardStats = await db.EventBoardStats.FindAsync(id);
            if (eventBoardStats == null)
            {
                return NotFound();
            }

            return Ok(eventBoardStats);
        }

        // PUT: api/EventBoardStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEventBoardStats(int id, EventBoardStats eventBoardStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventBoardStats.EventBoardStatsNo)
            {
                return BadRequest();
            }

            db.Entry(eventBoardStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventBoardStatsExists(id))
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

        // POST: api/EventBoardStats
        [ResponseType(typeof(EventBoardStats))]
        public async Task<IHttpActionResult> PostEventBoardStats(EventBoardStats eventBoardStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventBoardStats.Add(eventBoardStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eventBoardStats.EventBoardStatsNo }, eventBoardStats);
        }

        // DELETE: api/EventBoardStats/5
        [ResponseType(typeof(EventBoardStats))]
        public async Task<IHttpActionResult> DeleteEventBoardStats(int id)
        {
            EventBoardStats eventBoardStats = await db.EventBoardStats.FindAsync(id);
            if (eventBoardStats == null)
            {
                return NotFound();
            }

            db.EventBoardStats.Remove(eventBoardStats);
            await db.SaveChangesAsync();

            return Ok(eventBoardStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventBoardStatsExists(int id)
        {
            return db.EventBoardStats.Count(e => e.EventBoardStatsNo == id) > 0;
        }
    }
}