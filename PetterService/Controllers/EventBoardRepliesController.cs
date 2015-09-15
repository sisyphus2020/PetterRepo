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
    public class EventBoardRepliesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/EventBoardReplies
        public IQueryable<EventBoardReply> GetEventBoardReplies()
        {
            return db.EventBoardReplies;
        }

        // GET: api/EventBoardReplies/5
        [ResponseType(typeof(EventBoardReply))]
        public async Task<IHttpActionResult> GetEventBoardReply(int id)
        {
            EventBoardReply eventBoardReply = await db.EventBoardReplies.FindAsync(id);
            if (eventBoardReply == null)
            {
                return NotFound();
            }

            return Ok(eventBoardReply);
        }

        // PUT: api/EventBoardReplies/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEventBoardReply(int id, EventBoardReply eventBoardReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventBoardReply.EventBoardReplyNo)
            {
                return BadRequest();
            }

            db.Entry(eventBoardReply).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventBoardReplyExists(id))
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

        // POST: api/EventBoardReplies
        [ResponseType(typeof(EventBoardReply))]
        public async Task<IHttpActionResult> PostEventBoardReply(EventBoardReply eventBoardReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventBoardReplies.Add(eventBoardReply);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eventBoardReply.EventBoardReplyNo }, eventBoardReply);
        }

        // DELETE: api/EventBoardReplies/5
        [ResponseType(typeof(EventBoardReply))]
        public async Task<IHttpActionResult> DeleteEventBoardReply(int id)
        {
            EventBoardReply eventBoardReply = await db.EventBoardReplies.FindAsync(id);
            if (eventBoardReply == null)
            {
                return NotFound();
            }

            db.EventBoardReplies.Remove(eventBoardReply);
            await db.SaveChangesAsync();

            return Ok(eventBoardReply);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventBoardReplyExists(int id)
        {
            return db.EventBoardReplies.Count(e => e.EventBoardReplyNo == id) > 0;
        }
    }
}