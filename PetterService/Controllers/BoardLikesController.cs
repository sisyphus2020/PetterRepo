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
    public class BoardLikesController : ApiController
    {
        // 1. 스토어 소식 좋아요 리스트 (X)
        // 2. 스토어 소식 좋아요 상세 (X)
        // 3. 스토어 소식 좋아요 등록 (O)
        // 4. 스토어 소식 좋아요 수정 (O)
        // 5. 스토어 소식 좋아요 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BoardLikes
        public IQueryable<BoardLike> GetBoardLikes()
        {
            return db.BoardLikes;
        }

        // GET: api/BoardLikes/5
        [ResponseType(typeof(BoardLike))]
        public async Task<IHttpActionResult> GetBoardLike(int id)
        {
            BoardLike BoardLike = await db.BoardLikes.FindAsync(id);
            if (BoardLike == null)
            {
                return NotFound();
            }

            return Ok(BoardLike);
        }

        /// <summary>
        /// PUT: api/BoardLikes/5
        /// 스토어 소식 좋아요 수정
        /// </summary>
        /// <param name="id"></param>
        /// <param name="galleryLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardLike>))]
        public async Task<IHttpActionResult> PutBoardLike(int id, BoardLike galleryLike)
        {
            PetterResultType<BoardLike> petterResultType = new PetterResultType<BoardLike>();
            List<BoardLike> BoardLikes = new List<BoardLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != galleryLike.BoardLikeNo)
            {
                return BadRequest();
            }

            BoardLike BoardLike = await db.BoardLikes.FindAsync(id);
            if (BoardLike == null)
            {
                return NotFound();
            }

            BoardLike.StateFlag = StateFlags.Use;
            BoardLike.DateModified = DateTime.Now;
            BoardLike.DateDeleted = null;
            db.Entry(BoardLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardLikeExists(id))
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
        /// POST: api/BoardLikes
        /// 스토어 소식 좋아요 등록
        /// </summary>
        /// <param name="BoardLike"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardLike>))]
        public async Task<IHttpActionResult> PostBoardLike(BoardLike BoardLike)
        {
            PetterResultType<BoardLike> petterResultType = new PetterResultType<BoardLike>();
            List<BoardLike> BoardLikes = new List<BoardLike>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BoardLike.StateFlag = StateFlags.Use;
            BoardLike.DateCreated = DateTime.Now;
            BoardLike.DateModified = DateTime.Now;

            db.BoardLikes.Add(BoardLike);
            await db.SaveChangesAsync();

            BoardLikes.Add(BoardLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = BoardLikes;

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/BoardLikes/5
        /// 스토어 소식 좋아요 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(BoardLike))]
        public async Task<IHttpActionResult> DeleteBoardLike(int id)
        {
            PetterResultType<BoardLike> petterResultType = new PetterResultType<BoardLike>();
            List<BoardLike> BoardLikes = new List<BoardLike>();
            BoardLike BoardLike = await db.BoardLikes.FindAsync(id);

            if (BoardLike == null)
            {
                return NotFound();
            }

            BoardLike.StateFlag = StateFlags.Delete;
            BoardLike.DateDeleted = DateTime.Now;
            db.Entry(BoardLike).State = EntityState.Modified;

            await db.SaveChangesAsync();

            BoardLikes.Add(BoardLike);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = BoardLikes;

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

        private bool BoardLikeExists(int id)
        {
            return db.BoardLikes.Count(e => e.BoardLikeNo == id) > 0;
        }
    }
}