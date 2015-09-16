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
using PetterService.Common;

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

        /// <summary>
        /// PUT: api/EventBoardReplies/5
        /// 이벤트게시판 리뷰 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<EventBoardReply>))]
        public async Task<IHttpActionResult> PutEventBoardReply(int id, EventBoardReply boardReply)
        {
            PetterResultType<EventBoardReply> petterResultType = new PetterResultType<EventBoardReply>();
            List<EventBoardReply> eventBoardReplies = new List<EventBoardReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventBoardReply eventBoardReply = await db.EventBoardReplies.FindAsync(id);
            if (eventBoardReply == null)
            {
                return NotFound();
            }

            // 수정권한 체크
            if (eventBoardReply.MemberNo != boardReply.MemberNo)
            {
                return BadRequest(ModelState);
            }

            eventBoardReply.Reply = boardReply.Reply;
            eventBoardReply.StateFlag = StateFlags.Use;
            eventBoardReply.DateModified = DateTime.Now;
            db.Entry(eventBoardReply).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            eventBoardReplies.Add(eventBoardReply);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = eventBoardReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/EventBoardReplies
        /// 이벤트게시판 리뷰 등록
        /// </summary>
        /// <param name="eventBoardReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<EventBoardReply>))]
        public async Task<IHttpActionResult> PostEventBoardReply(EventBoardReply eventBoardReply)
        {
            PetterResultType<EventBoardReply> petterResultType = new PetterResultType<EventBoardReply>();
            List<EventBoardReply> eventBoardReplies = new List<EventBoardReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            eventBoardReply.StateFlag = StateFlags.Use;
            eventBoardReply.DateCreated = DateTime.Now;
            eventBoardReply.DateModified = DateTime.Now;

            db.EventBoardReplies.Add(eventBoardReply);
            await db.SaveChangesAsync();

            eventBoardReplies.Add(eventBoardReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = eventBoardReplies;
            return Ok(petterResultType);
        }

        // DELETE: api/EventBoardReplies/5
        [ResponseType(typeof(EventBoardReply))]
        public async Task<IHttpActionResult> DeleteEventBoardReply(int id)
        {
            PetterResultType<EventBoardReply> petterResultType = new PetterResultType<EventBoardReply>();
            List<EventBoardReply> companionAnimals = new List<EventBoardReply>();
            EventBoardReply eventBoardReply = await db.EventBoardReplies.FindAsync(id);

            if (eventBoardReply == null)
            {
                return NotFound();
            }

            eventBoardReply.StateFlag = StateFlags.Delete;
            eventBoardReply.DateDeleted = DateTime.Now;
            db.Entry(eventBoardReply).State = EntityState.Modified;

            await db.SaveChangesAsync();

            companionAnimals.Add(eventBoardReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = companionAnimals;

            return Ok(petterResultType);
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