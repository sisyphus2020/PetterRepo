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
    public class BeautyShopStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopStats
        public IQueryable<BeautyShopStats> GetBeautyShopStats()
        {
            return db.BeautyShopStats;
        }

        // GET: api/BeautyShopStats/5
        [ResponseType(typeof(BeautyShopStats))]
        public async Task<IHttpActionResult> GetBeautyShopStats(int id)
        {
            BeautyShopStats beautyShopStats = await db.BeautyShopStats.FindAsync(id);
            if (beautyShopStats == null)
            {
                return NotFound();
            }

            return Ok(beautyShopStats);
        }

        // PUT: api/BeautyShopStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShopStats(int id, BeautyShopStats beautyShopStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShopStats.BeautyShopStatsNo)
            {
                return BadRequest();
            }

            db.Entry(beautyShopStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeautyShopStatsExists(id))
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

        // POST: api/BeautyShopStats
        [ResponseType(typeof(BeautyShopStats))]
        public async Task<IHttpActionResult> PostBeautyShopStats(BeautyShopStats beautyShopStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShopStats.Add(beautyShopStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShopStats.BeautyShopStatsNo }, beautyShopStats);
        }

        // DELETE: api/BeautyShopStats/5
        [ResponseType(typeof(BeautyShopStats))]
        public async Task<IHttpActionResult> DeleteBeautyShopStats(int id)
        {
            BeautyShopStats beautyShopStats = await db.BeautyShopStats.FindAsync(id);
            if (beautyShopStats == null)
            {
                return NotFound();
            }

            db.BeautyShopStats.Remove(beautyShopStats);
            await db.SaveChangesAsync();

            return Ok(beautyShopStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeautyShopStatsExists(int id)
        {
            return db.BeautyShopStats.Count(e => e.BeautyShopStatsNo == id) > 0;
        }
    }
}