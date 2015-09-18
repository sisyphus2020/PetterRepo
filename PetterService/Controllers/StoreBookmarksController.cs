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
    public class StoreBookmarksController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopBookmarks
        public IQueryable<StoreBookmark> GetStoreBookmarks()
        {
            return db.BeautyShopBookmarks;
        }

        // GET: api/BeautyShopBookmarks/5
        [ResponseType(typeof(StoreBookmark))]
        public async Task<IHttpActionResult> GetStoreBookmark(int id)
        {
            StoreBookmark beautyShopBookmark = await db.BeautyShopBookmarks.FindAsync(id);
            if (beautyShopBookmark == null)
            {
                return NotFound();
            }

            return Ok(beautyShopBookmark);
        }

        /// <summary>
        /// PUT: api/BeautyShopBookmarks/5
        /// 미용즐겨찾기 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shopBookmark"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreBookmark>))]
        public async Task<IHttpActionResult> PutStoreBookmark(int id, StoreBookmark shopBookmark)
        {
            PetterResultType<StoreBookmark> petterResultType = new PetterResultType<StoreBookmark>();
            List<StoreBookmark> beautyShopBookmarks = new List<StoreBookmark>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreBookmark beautyShopBookmark = await db.BeautyShopBookmarks.FindAsync(id);
            if (beautyShopBookmark == null)
            {
                return NotFound();
            }

            // 수정권한 체크
            if (beautyShopBookmark.MemberNo != shopBookmark.MemberNo)
            {
                return BadRequest(ModelState);
            }

            //beautyShopBookmark.Reply = boardReply.Reply;
            //beautyShopBookmark.StateFlag = StateFlags.Use;
            beautyShopBookmark.DateModified = DateTime.Now;
            db.Entry(beautyShopBookmark).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            beautyShopBookmarks.Add(beautyShopBookmark);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = beautyShopBookmarks;

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/BeautyShopBookmarks
        /// 미용즐겨찾기 등록
        /// </summary>
        /// <param name="beautyShopBookmark"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreBookmark>))]
        public async Task<IHttpActionResult> PostStoreBookmark(StoreBookmark beautyShopBookmark)
        {
            PetterResultType<StoreBookmark> petterResultType = new PetterResultType<StoreBookmark>();
            List<StoreBookmark> beautyShopBookmarks = new List<StoreBookmark>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            beautyShopBookmark.DateCreated = DateTime.Now;
            beautyShopBookmark.DateModified = DateTime.Now;

            db.BeautyShopBookmarks.Add(beautyShopBookmark);
            await db.SaveChangesAsync();

            beautyShopBookmarks.Add(beautyShopBookmark);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = beautyShopBookmarks;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/BeautyShopBookmarks/5
        /// 미용즐겨찾기 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreBookmark>))]
        public async Task<IHttpActionResult> DeleteStoreBookmark(int id)
        {
            PetterResultType<StoreBookmark> petterResultType = new PetterResultType<StoreBookmark>();
            List<StoreBookmark> beautyShopBookmarks = new List<StoreBookmark>();
            StoreBookmark beautyShopBookmark = await db.BeautyShopBookmarks.FindAsync(id);

            if (beautyShopBookmark == null)
            {
                return NotFound();
            }

            db.BeautyShopBookmarks.Remove(beautyShopBookmark);
            await db.SaveChangesAsync();

            beautyShopBookmarks.Add(beautyShopBookmark);

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = beautyShopBookmarks;

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

        private bool BeautyShopBookmarkExists(int id)
        {
            return db.BeautyShopBookmarks.Count(e => e.StoreBookmarkNo == id) > 0;
        }
    }
}