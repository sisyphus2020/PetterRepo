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
    public class EventBoardFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/EventBoardFiles
        public IQueryable<EventBoardFile> GetEventBoardFiles()
        {
            return db.EventBoardFiles;
        }

        // GET: api/EventBoardFiles/5
        [ResponseType(typeof(EventBoardFile))]
        public async Task<IHttpActionResult> GetEventBoardFile(int id)
        {
            EventBoardFile eventBoardFile = await db.EventBoardFiles.FindAsync(id);
            if (eventBoardFile == null)
            {
                return NotFound();
            }

            return Ok(eventBoardFile);
        }

        // PUT: api/EventBoardFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEventBoardFile(int id, EventBoardFile eventBoardFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventBoardFile.EvnetBoardFileNo)
            {
                return BadRequest();
            }

            db.Entry(eventBoardFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventBoardFileExists(id))
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

        // POST: api/EventBoardFiles
        [ResponseType(typeof(EventBoardFile))]
        public async Task<IHttpActionResult> PostEventBoardFile(EventBoardFile eventBoardFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventBoardFiles.Add(eventBoardFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eventBoardFile.EvnetBoardFileNo }, eventBoardFile);
        }

        // DELETE: api/EventBoardFiles/5
        [ResponseType(typeof(EventBoardFile))]
        public async Task<IHttpActionResult> DeleteEventBoardFile(int id)
        {
            EventBoardFile eventBoardFile = await db.EventBoardFiles.FindAsync(id);
            if (eventBoardFile == null)
            {
                return NotFound();
            }

            db.EventBoardFiles.Remove(eventBoardFile);
            await db.SaveChangesAsync();

            return Ok(eventBoardFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventBoardFileExists(int id)
        {
            return db.EventBoardFiles.Count(e => e.EvnetBoardFileNo == id) > 0;
        }
    }
}