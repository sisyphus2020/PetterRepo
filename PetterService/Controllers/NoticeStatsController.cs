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
    public class NoticeStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/NoticeStats
        public IQueryable<NoticeStats> GetNoticeStats()
        {
            return db.NoticeStats;
        }

        // GET: api/NoticeStats/5
        [ResponseType(typeof(NoticeStats))]
        public async Task<IHttpActionResult> GetNoticeStats(int id)
        {
            NoticeStats noticeStats = await db.NoticeStats.FindAsync(id);
            if (noticeStats == null)
            {
                return NotFound();
            }

            return Ok(noticeStats);
        }

        // PUT: api/NoticeStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNoticeStats(int id, NoticeStats noticeStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != noticeStats.NoticeStatsNo)
            {
                return BadRequest();
            }

            db.Entry(noticeStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeStatsExists(id))
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

        // POST: api/NoticeStats
        [ResponseType(typeof(NoticeStats))]
        public async Task<IHttpActionResult> PostNoticeStats(NoticeStats noticeStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NoticeStats.Add(noticeStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = noticeStats.NoticeStatsNo }, noticeStats);
        }

        // DELETE: api/NoticeStats/5
        [ResponseType(typeof(NoticeStats))]
        public async Task<IHttpActionResult> DeleteNoticeStats(int id)
        {
            NoticeStats noticeStats = await db.NoticeStats.FindAsync(id);
            if (noticeStats == null)
            {
                return NotFound();
            }

            db.NoticeStats.Remove(noticeStats);
            await db.SaveChangesAsync();

            return Ok(noticeStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NoticeStatsExists(int id)
        {
            return db.NoticeStats.Count(e => e.NoticeStatsNo == id) > 0;
        }
    }
}