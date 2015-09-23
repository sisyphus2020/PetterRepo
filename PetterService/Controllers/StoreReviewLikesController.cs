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
    public class StoreReviewLikesController : ApiController
    {
        // 1. 스토어 리뷰 좋아요 리스트 (X)
        // 2. 스토어 리뷰 좋아요 상세 (X)
        // 3. 스토어 리뷰 좋아요 등록 (O)
        // 4. 스토어 리뷰 좋아요 수정 (O)
        // 5. 스토어 리뷰 좋아요 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreReviewLikes
        public IQueryable<StoreReviewLike> GetStoreReviewLikes()
        {
            return db.StoreReviewLikes;
        }

        // GET: api/StoreReviewLikes/5
        [ResponseType(typeof(StoreReviewLike))]
        public async Task<IHttpActionResult> GetStoreReviewLike(int id)
        {
            StoreReviewLike storeReviewLike = await db.StoreReviewLikes.FindAsync(id);
            if (storeReviewLike == null)
            {
                return NotFound();
            }

            return Ok(storeReviewLike);
        }

        /// <summary>
        /// PUT: api/StoreReviewLikes/5
        /// 스토어 리뷰 좋아요 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reviewLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReviewLike>))]
        public async Task<IHttpActionResult> PutStoreReviewLike(int id, StoreReviewLike reviewLike)
        {
            PetterResultType<StoreReviewLike> petterResultType = new PetterResultType<StoreReviewLike>();
            List<StoreReviewLike> storeReviewLikes = new List<StoreReviewLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reviewLike.StoreReviewLikeNo)
            {
                return BadRequest();
            }

            StoreReviewLike storeReviewLike = await db.StoreReviewLikes.FindAsync(id);
            if (storeReviewLike == null)
            {
                return NotFound();
            }

            storeReviewLike.StateFlag = StateFlags.Use;
            storeReviewLike.DateModified = DateTime.Now;
            storeReviewLike.DateDeleted = null;
            db.Entry(storeReviewLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreReviewLikeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/StoreReviewLikes
        /// 스토어 리뷰 좋아요 등록
        /// </summary>
        /// <param name="storeReviewLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReviewLike>))]
        public async Task<IHttpActionResult> PostStoreReviewLike(StoreReviewLike storeReviewLike)
        {
            PetterResultType<StoreReviewLike> petterResultType = new PetterResultType<StoreReviewLike>();
            List<StoreReviewLike> storeReviewLikes = new List<StoreReviewLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            storeReviewLike.StateFlag = StateFlags.Use;
            storeReviewLike.DateCreated = DateTime.Now;
            storeReviewLike.DateModified = DateTime.Now;

            db.StoreReviewLikes.Add(storeReviewLike);
            await db.SaveChangesAsync();

            storeReviewLikes.Add(storeReviewLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeReviewLikes;

            return Ok(petterResultType);
        }

        // DELETE: api/StoreReviewLikes/5
        /// <summary>
        /// DELETE: api/StoreReviewLikes/5
        /// 스토어 리뷰 좋아요 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReviewLike>))]
        public async Task<IHttpActionResult> DeleteStoreReviewLike(int id)
        {
            PetterResultType<StoreReviewLike> petterResultType = new PetterResultType<StoreReviewLike>();
            List<StoreReviewLike> storeReviewLikes = new List<StoreReviewLike>();
            StoreReviewLike storeReviewLike = await db.StoreReviewLikes.FindAsync(id);

            if (storeReviewLike == null)
            {
                return NotFound();
            }

            storeReviewLike.StateFlag = StateFlags.Delete;
            storeReviewLike.DateDeleted = DateTime.Now;
            db.Entry(storeReviewLike).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeReviewLikes.Add(storeReviewLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeReviewLikes;

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

        private bool StoreReviewLikeExists(int id)
        {
            return db.StoreReviewLikes.Count(e => e.StoreReviewLikeNo == id) > 0;
        }
    }
}