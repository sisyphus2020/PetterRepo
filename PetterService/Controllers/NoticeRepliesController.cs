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
    public class NoticeRepliesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();
             
        // GET: api/NoticeReplies
        public IQueryable<NoticeReply> GetNoticeReplies()
        {
            return db.NoticeReplies;
        }

        // GET: api/NoticeReplies/5
        [ResponseType(typeof(NoticeReply))]
        public async Task<IHttpActionResult> GetNoticeReply(int id)
        {
            NoticeReply noticeReply = await db.NoticeReplies.FindAsync(id);

            if (noticeReply == null)
            {
                return NotFound();
            }

            return Ok(noticeReply);
        }

        /// <summary>
        /// PUT: api/NoticeReplies/5
        /// 공지사항 댓글 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<NoticeReply>))]
        public async Task<IHttpActionResult> PutNoticeReply(int id, NoticeReply reply)
        {
            PetterResultType<NoticeReply> petterResultType = new PetterResultType<NoticeReply>();
            List<NoticeReply> noticeReplies = new List<NoticeReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            NoticeReply noticeReply = await db.NoticeReplies.FindAsync(id);
            if (noticeReply == null)
            {
                return NotFound();
            }

            // 수정권한 체크
            if (noticeReply.MemberNo != noticeReply.MemberNo)
            {
                return BadRequest(ModelState);
            }

            noticeReply.Reply = reply.Reply;
            noticeReply.StateFlag = StateFlags.Use;
            noticeReply.DateModified = DateTime.Now;
            db.Entry(noticeReply).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            noticeReplies.Add(noticeReply);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = noticeReplies;

            return Ok(petterResultType);
        }

        // POST: api/NoticeReplies
        /// <summary>
        /// POST: api/NoticeReplies
        /// 공지사항 댓글 등록
        /// </summary>
        /// <param name="noticeReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<NoticeReply>))]
        public async Task<IHttpActionResult> PostNoticeReply(NoticeReply noticeReply)
        {
            PetterResultType<NoticeReply> petterResultType = new PetterResultType<NoticeReply>();
            List<NoticeReply> noticeReplies = new List<NoticeReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            noticeReply.StateFlag = StateFlags.Use;
            noticeReply.DateCreated = DateTime.Now;
            noticeReply.DateModified = DateTime.Now;

            db.NoticeReplies.Add(noticeReply);
            await db.SaveChangesAsync();

            noticeReplies.Add(noticeReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = noticeReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/NoticeReplies/5
        /// 공지사항 댓글 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<NoticeReply>))]
        public async Task<IHttpActionResult> DeleteNoticeReply(int id)
        {
            PetterResultType<NoticeReply> petterResultType = new PetterResultType<NoticeReply>();
            List<NoticeReply> noticeReplies = new List<NoticeReply>();
            NoticeReply noticeReply = await db.NoticeReplies.FindAsync(id);

            if (noticeReply == null)
            {
                return NotFound();
            }

            noticeReply.StateFlag = StateFlags.Delete;
            noticeReply.DateDeleted = DateTime.Now;
            db.Entry(noticeReply).State = EntityState.Modified;

            await db.SaveChangesAsync();

            noticeReplies.Add(noticeReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = noticeReplies;

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

        private bool NoticeReplyExists(int id)
        {
            return db.NoticeReplies.Count(e => e.NoticeReplyNo == id) > 0;
        }
    }
}