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
    public class StoreNewsLikesController : ApiController
    {
        // 1. 스토어 소식 좋아요 리스트 (X)
        // 2. 스토어 소식 좋아요 상세 (X)
        // 3. 스토어 소식 좋아요 등록 (O)
        // 4. 스토어 소식 좋아요 수정 (O)
        // 5. 스토어 소식 좋아요 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreNewsLikes
        public IQueryable<StoreNewsLike> GetStoreNewsLikes()
        {
            return db.StoreNewsLikes;
        }

        // GET: api/StoreNewsLikes/5
        [ResponseType(typeof(StoreNewsLike))]
        public async Task<IHttpActionResult> GetStoreNewsLike(int id)
        {
            StoreNewsLike StoreNewsLike = await db.StoreNewsLikes.FindAsync(id);
            if (StoreNewsLike == null)
            {
                return NotFound();
            }

            return Ok(StoreNewsLike);
        }

        /// <summary>
        /// PUT: api/StoreNewsLikes/5
        /// 스토어 소식 좋아요 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="galleryLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNewsLike>))]
        public async Task<IHttpActionResult> PutStoreNewsLike(int id, StoreNewsLike galleryLike)
        {
            PetterResultType<StoreNewsLike> petterResultType = new PetterResultType<StoreNewsLike>();
            List<StoreNewsLike> StoreNewsLikes = new List<StoreNewsLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != galleryLike.StoreNewsLikeNo)
            {
                return BadRequest();
            }

            StoreNewsLike StoreNewsLike = await db.StoreNewsLikes.FindAsync(id);
            if (StoreNewsLike == null)
            {
                return NotFound();
            }

            StoreNewsLike.StateFlag = StateFlags.Use;
            StoreNewsLike.DateModified = DateTime.Now;
            StoreNewsLike.DateDeleted = null;
            db.Entry(StoreNewsLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreNewsLikeExists(id))
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
        /// POST: api/StoreNewsLikes
        /// 스토어 소식 좋아요 등록
        /// </summary>
        /// <param name="StoreNewsLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNewsLike>))]
        public async Task<IHttpActionResult> PostStoreNewsLike(StoreNewsLike StoreNewsLike)
        {
            PetterResultType<StoreNewsLike> petterResultType = new PetterResultType<StoreNewsLike>();
            List<StoreNewsLike> StoreNewsLikes = new List<StoreNewsLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreNewsLike.StateFlag = StateFlags.Use;
            StoreNewsLike.DateCreated = DateTime.Now;
            StoreNewsLike.DateModified = DateTime.Now;

            db.StoreNewsLikes.Add(StoreNewsLike);
            await db.SaveChangesAsync();

            StoreNewsLikes.Add(StoreNewsLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = StoreNewsLikes;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/StoreNewsLikes/5
        /// 스토어 소식 좋아요 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(StoreNewsLike))]
        public async Task<IHttpActionResult> DeleteStoreNewsLike(int id)
        {
            PetterResultType<StoreNewsLike> petterResultType = new PetterResultType<StoreNewsLike>();
            List<StoreNewsLike> StoreNewsLikes = new List<StoreNewsLike>();
            StoreNewsLike StoreNewsLike = await db.StoreNewsLikes.FindAsync(id);

            if (StoreNewsLike == null)
            {
                return NotFound();
            }

            StoreNewsLike.StateFlag = StateFlags.Delete;
            StoreNewsLike.DateDeleted = DateTime.Now;
            db.Entry(StoreNewsLike).State = EntityState.Modified;

            await db.SaveChangesAsync();

            StoreNewsLikes.Add(StoreNewsLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = StoreNewsLikes;

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

        private bool StoreNewsLikeExists(int id)
        {
            return db.StoreNewsLikes.Count(e => e.StoreNewsLikeNo == id) > 0;
        }
    }
}