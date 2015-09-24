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
    public class BoardRepliesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BoardReplies
        public IQueryable<BoardReply> GetBoardReplies()
        {
            return db.BoardReplies;
        }

        // GET: api/BoardReplies/5
        [ResponseType(typeof(BoardReply))]
        public async Task<IHttpActionResult> GetBoardReply(int id)
        {
            BoardReply BoardReply = await db.BoardReplies.FindAsync(id);
            if (BoardReply == null)
            {
                return NotFound();
            }

            return Ok(BoardReply);
        }

        /// <summary>
        /// PUT: api/BoardReplies/5
        /// 스토어 소식 댓글 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="galleryReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardReply>))]
        public async Task<IHttpActionResult> PutBoardReply(int id, BoardReply galleryReply)
        {
            PetterResultType<BoardReply> petterResultType = new PetterResultType<BoardReply>();
            List<BoardReply> BoardReplies = new List<BoardReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BoardReply BoardReply = await db.BoardReplies.FindAsync(id);
            if (BoardReply == null)
            {
                return NotFound();
            }

            // 수정권한 체크
            if (BoardReply.MemberNo != galleryReply.MemberNo)
            {
                return BadRequest(ModelState);
            }

            BoardReply.Reply = galleryReply.Reply;
            BoardReply.StateFlag = StateFlags.Use;
            BoardReply.DateModified = DateTime.Now;
            db.Entry(BoardReply).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            BoardReplies.Add(BoardReply);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = BoardReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/BoardReplies
        /// 스토어 소식 댓글 등록
        /// </summary>
        /// <param name="BoardReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardReply>))]
        public async Task<IHttpActionResult> PostBoardReply(BoardReply BoardReply)
        {
            PetterResultType<BoardReply> petterResultType = new PetterResultType<BoardReply>();
            List<BoardReply> BoardReplies = new List<BoardReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BoardReply.StateFlag = StateFlags.Use;
            BoardReply.DateCreated = DateTime.Now;
            BoardReply.DateModified = DateTime.Now;

            db.BoardReplies.Add(BoardReply);
            await db.SaveChangesAsync();

            BoardReplies.Add(BoardReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = BoardReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/BoardReplies/5
        /// 스토어 소식 댓글 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardReply>))]
        public async Task<IHttpActionResult> DeleteBoardReply(int id)
        {
            PetterResultType<BoardReply> petterResultType = new PetterResultType<BoardReply>();
            List<BoardReply> BoardReplies = new List<BoardReply>();
            BoardReply BoardReply = await db.BoardReplies.FindAsync(id);

            if (BoardReply == null)
            {
                return NotFound();
            }

            BoardReply.StateFlag = StateFlags.Delete;
            BoardReply.DateDeleted = DateTime.Now;
            db.Entry(BoardReply).State = EntityState.Modified;

            await db.SaveChangesAsync();

            BoardReplies.Add(BoardReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = BoardReplies;

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

        private bool BoardReplyExists(int id)
        {
            return db.BoardReplies.Count(e => e.BoardReplyNo == id) > 0;
        }
    }
}