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
    public class BoardStatsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BoardStats
        public IQueryable<BoardStats> GetBoardStats()
        {
            return db.BoardStats;
        }

        // GET: api/BoardStats/5
        [ResponseType(typeof(BoardStats))]
        public async Task<IHttpActionResult> GetBoardStats(int id)
        {
            BoardStats BoardStats = await db.BoardStats.FindAsync(id);
            if (BoardStats == null)
            {
                return NotFound();
            }

            return Ok(BoardStats);
        }

        // PUT: api/BoardStats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBoardStats(int id, BoardStats BoardStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != BoardStats.BoardStatsNo)
            {
                return BadRequest();
            }

            db.Entry(BoardStats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardStatsExists(id))
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

        // POST: api/BoardStats
        [ResponseType(typeof(BoardStats))]
        public async Task<IHttpActionResult> PostBoardStats(BoardStats BoardStats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BoardStats.Add(BoardStats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = BoardStats.BoardStatsNo }, BoardStats);
        }

        // DELETE: api/BoardStats/5
        [ResponseType(typeof(BoardStats))]
        public async Task<IHttpActionResult> DeleteBoardStats(int id)
        {
            BoardStats BoardStats = await db.BoardStats.FindAsync(id);
            if (BoardStats == null)
            {
                return NotFound();
            }

            db.BoardStats.Remove(BoardStats);
            await db.SaveChangesAsync();

            return Ok(BoardStats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BoardStatsExists(int id)
        {
            return db.BoardStats.Count(e => e.BoardStatsNo == id) > 0;
        }
    }
}