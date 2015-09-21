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
    public class StoreGalleryLikesController : ApiController
    {
        // 1. 스토어 갤러리 좋아요 리스트 (X)
        // 2. 스토어 갤러리 좋아요 상세 (X)
        // 3. 스토어 갤러리 좋아요 등록 (O)
        // 4. 스토어 갤러리 좋아요 수정 (O)
        // 5. 스토어 갤러리 좋아요 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleryLikes
        public IQueryable<StoreGalleryLike> GetStoreGalleryLikes()
        {
            return db.StoreGalleryLikes;
        }

        // GET: api/StoreGalleryLikes/5
        [ResponseType(typeof(StoreGalleryLike))]
        public async Task<IHttpActionResult> GetStoreGalleryLike(int id)
        {
            StoreGalleryLike storeGalleryLike = await db.StoreGalleryLikes.FindAsync(id);
            if (storeGalleryLike == null)
            {
                return NotFound();
            }

            return Ok(storeGalleryLike);
        }

        /// <summary>
        /// PUT: api/StoreGalleryLikes/5
        /// 스토어 갤러리 좋아요 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="galleryLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGalleryLike>))]
        public async Task<IHttpActionResult> PutStoreGalleryLike(int id, StoreGalleryLike galleryLike)
        {
            PetterResultType<StoreGalleryLike> petterResultType = new PetterResultType<StoreGalleryLike>();
            List<StoreGalleryLike> storeGalleryLikes = new List<StoreGalleryLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != galleryLike.StoreGalleryLikeNo)
            {
                return BadRequest();
            }

            StoreGalleryLike storeGalleryLike = await db.StoreGalleryLikes.FindAsync(id);
            if (storeGalleryLike == null)
            {
                return NotFound();
            }

            storeGalleryLike.StateFlag = StateFlags.Use;
            storeGalleryLike.DateModified = DateTime.Now;
            storeGalleryLike.DateDeleted = null;
            db.Entry(storeGalleryLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGalleryLikeExists(id))
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
        /// POST: api/StoreGalleryLikes
        /// 스토어 갤러리 좋아요 등록
        /// </summary>
        /// <param name="storeGalleryLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGalleryLike>))]
        public async Task<IHttpActionResult> PostStoreGalleryLike(StoreGalleryLike storeGalleryLike)
        {
            PetterResultType<StoreGalleryLike> petterResultType = new PetterResultType<StoreGalleryLike>();
            List<StoreGalleryLike> storeGalleryLikes = new List<StoreGalleryLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            storeGalleryLike.StateFlag = StateFlags.Use;
            storeGalleryLike.DateCreated = DateTime.Now;
            storeGalleryLike.DateModified = DateTime.Now;

            db.StoreGalleryLikes.Add(storeGalleryLike);
            await db.SaveChangesAsync();

            storeGalleryLikes.Add(storeGalleryLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleryLikes;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/StoreGalleryLikes/5
        /// 스토어 갤러리 좋아요 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(StoreGalleryLike))]
        public async Task<IHttpActionResult> DeleteStoreGalleryLike(int id)
        {
            PetterResultType<StoreGalleryLike> petterResultType = new PetterResultType<StoreGalleryLike>();
            List<StoreGalleryLike> storeGalleryLikes = new List<StoreGalleryLike>();
            StoreGalleryLike storeGalleryLike = await db.StoreGalleryLikes.FindAsync(id);

            if (storeGalleryLike == null)
            {
                return NotFound();
            }

            storeGalleryLike.StateFlag = StateFlags.Delete;
            storeGalleryLike.DateDeleted = DateTime.Now;
            db.Entry(storeGalleryLike).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeGalleryLikes.Add(storeGalleryLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleryLikes;

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

        private bool StoreGalleryLikeExists(int id)
        {
            return db.StoreGalleryLikes.Count(e => e.StoreGalleryLikeNo == id) > 0;
        }
    }
}