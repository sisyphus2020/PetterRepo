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
    public class StoreNewsRepliesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreNewsReplies
        public IQueryable<StoreNewsReply> GetStoreNewsReplies()
        {
            return db.StoreNewsReplies;
        }

        // GET: api/StoreNewsReplies/5
        [ResponseType(typeof(StoreNewsReply))]
        public async Task<IHttpActionResult> GetStoreNewsReply(int id)
        {
            StoreNewsReply StoreNewsReply = await db.StoreNewsReplies.FindAsync(id);
            if (StoreNewsReply == null)
            {
                return NotFound();
            }

            return Ok(StoreNewsReply);
        }

        /// <summary>
        /// PUT: api/StoreNewsReplies/5
        /// 스토어 소식 댓글 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="galleryReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNewsReply>))]
        public async Task<IHttpActionResult> PutStoreNewsReply(int id, StoreNewsReply galleryReply)
        {
            PetterResultType<StoreNewsReply> petterResultType = new PetterResultType<StoreNewsReply>();
            List<StoreNewsReply> StoreNewsReplies = new List<StoreNewsReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreNewsReply StoreNewsReply = await db.StoreNewsReplies.FindAsync(id);
            if (StoreNewsReply == null)
            {
                return NotFound();
            }

            // 수정권한 체크
            if (StoreNewsReply.MemberNo != galleryReply.MemberNo)
            {
                return BadRequest(ModelState);
            }

            StoreNewsReply.Reply = galleryReply.Reply;
            StoreNewsReply.StateFlag = StateFlags.Use;
            StoreNewsReply.DateModified = DateTime.Now;
            db.Entry(StoreNewsReply).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            StoreNewsReplies.Add(StoreNewsReply);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = StoreNewsReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/StoreNewsReplies
        /// 스토어 소식 댓글 등록
        /// </summary>
        /// <param name="StoreNewsReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNewsReply>))]
        public async Task<IHttpActionResult> PostStoreNewsReply(StoreNewsReply StoreNewsReply)
        {
            PetterResultType<StoreNewsReply> petterResultType = new PetterResultType<StoreNewsReply>();
            List<StoreNewsReply> StoreNewsReplies = new List<StoreNewsReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreNewsReply.StateFlag = StateFlags.Use;
            StoreNewsReply.DateCreated = DateTime.Now;
            StoreNewsReply.DateModified = DateTime.Now;

            db.StoreNewsReplies.Add(StoreNewsReply);
            await db.SaveChangesAsync();

            StoreNewsReplies.Add(StoreNewsReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = StoreNewsReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/StoreNewsReplies/5
        /// 스토어 소식 댓글 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNewsReply>))]
        public async Task<IHttpActionResult> DeleteStoreNewsReply(int id)
        {
            PetterResultType<StoreNewsReply> petterResultType = new PetterResultType<StoreNewsReply>();
            List<StoreNewsReply> StoreNewsReplies = new List<StoreNewsReply>();
            StoreNewsReply StoreNewsReply = await db.StoreNewsReplies.FindAsync(id);

            if (StoreNewsReply == null)
            {
                return NotFound();
            }

            StoreNewsReply.StateFlag = StateFlags.Delete;
            StoreNewsReply.DateDeleted = DateTime.Now;
            db.Entry(StoreNewsReply).State = EntityState.Modified;

            await db.SaveChangesAsync();

            StoreNewsReplies.Add(StoreNewsReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = StoreNewsReplies;

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

        private bool StoreNewsReplyExists(int id)
        {
            return db.StoreNewsReplies.Count(e => e.StoreNewsReplyNo == id) > 0;
        }
    }
}