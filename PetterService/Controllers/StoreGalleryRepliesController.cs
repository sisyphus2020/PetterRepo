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
    public class StoreGalleryRepliesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleryReplies
        public IQueryable<StoreGalleryReply> GetStoreGalleryReplies()
        {
            return db.StoreGalleryReplies;
        }

        // GET: api/StoreGalleryReplies/5
        [ResponseType(typeof(StoreGalleryReply))]
        public async Task<IHttpActionResult> GetStoreGalleryReply(int id)
        {
            StoreGalleryReply storeGalleryReply = await db.StoreGalleryReplies.FindAsync(id);
            if (storeGalleryReply == null)
            {
                return NotFound();
            }

            return Ok(storeGalleryReply);
        }

        /// <summary>
        /// PUT: api/StoreGalleryReplies/5
        /// 스토어 갤러리 댓글 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="galleryReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGalleryReply>))]
        public async Task<IHttpActionResult> PutStoreGalleryReply(int id, StoreGalleryReply galleryReply)
        {
            PetterResultType<StoreGalleryReply> petterResultType = new PetterResultType<StoreGalleryReply>();
            List<StoreGalleryReply> storeGalleryReplies = new List<StoreGalleryReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreGalleryReply storeGalleryReply = await db.StoreGalleryReplies.FindAsync(id);
            if (storeGalleryReply == null)
            {
                return NotFound();
            }

            // 수정권한 체크
            if (storeGalleryReply.MemberNo != galleryReply.MemberNo)
            {
                return BadRequest(ModelState);
            }

            storeGalleryReply.Reply = galleryReply.Reply;
            storeGalleryReply.StateFlag = StateFlags.Use;
            storeGalleryReply.DateModified = DateTime.Now;
            db.Entry(storeGalleryReply).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            storeGalleryReplies.Add(storeGalleryReply);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleryReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/StoreGalleryReplies
        /// 스토어 갤러리 댓글 등록
        /// </summary>
        /// <param name="storeGalleryReply"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGalleryReply>))]
        public async Task<IHttpActionResult> PostStoreGalleryReply(StoreGalleryReply storeGalleryReply)
        {
            PetterResultType<StoreGalleryReply> petterResultType = new PetterResultType<StoreGalleryReply>();
            List<StoreGalleryReply> storeGalleryReplies = new List<StoreGalleryReply>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            storeGalleryReply.StateFlag = StateFlags.Use;
            storeGalleryReply.DateCreated = DateTime.Now;
            storeGalleryReply.DateModified = DateTime.Now;

            db.StoreGalleryReplies.Add(storeGalleryReply);
            await db.SaveChangesAsync();

            storeGalleryReplies.Add(storeGalleryReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleryReplies;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/StoreGalleryReplies/5
        /// 스토어 갤러리 댓글 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGalleryReply>))]
        public async Task<IHttpActionResult> DeleteStoreGalleryReply(int id)
        {
            PetterResultType<StoreGalleryReply> petterResultType = new PetterResultType<StoreGalleryReply>();
            List<StoreGalleryReply> storeGalleryReplies = new List<StoreGalleryReply>();
            StoreGalleryReply storeGalleryReply = await db.StoreGalleryReplies.FindAsync(id);

            if (storeGalleryReply == null)
            {
                return NotFound();
            }

            storeGalleryReply.StateFlag = StateFlags.Delete;
            storeGalleryReply.DateDeleted = DateTime.Now;
            db.Entry(storeGalleryReply).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeGalleryReplies.Add(storeGalleryReply);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleryReplies;

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

        private bool StoreGalleryReplyExists(int id)
        {
            return db.StoreGalleryReplies.Count(e => e.StoreGalleryReplyNo == id) > 0;
        }
    }
}