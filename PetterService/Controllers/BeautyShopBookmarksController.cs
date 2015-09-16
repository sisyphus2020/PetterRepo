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
    public class BeautyShopBookmarksController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopBookmarks
        public IQueryable<BeautyShopBookmark> GetBeautyShopBookmarks()
        {
            return db.BeautyShopBookmarks;
        }

        // GET: api/BeautyShopBookmarks/5
        [ResponseType(typeof(BeautyShopBookmark))]
        public async Task<IHttpActionResult> GetBeautyShopBookmark(int id)
        {
            BeautyShopBookmark beautyShopBookmark = await db.BeautyShopBookmarks.FindAsync(id);
            if (beautyShopBookmark == null)
            {
                return NotFound();
            }

            return Ok(beautyShopBookmark);
        }

        // PUT: api/BeautyShopBookmarks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShopBookmark(int id, BeautyShopBookmark beautyShopBookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShopBookmark.BeautyShopBookmarkNo)
            {
                return BadRequest();
            }

            db.Entry(beautyShopBookmark).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeautyShopBookmarkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BeautyShopBookmarks
        [ResponseType(typeof(PetterResultType<BeautyShopBookmark>))]
        public async Task<IHttpActionResult> PostBeautyShopBookmark(BeautyShopBookmark beautyShopBookmark)
        {
            PetterResultType<BeautyShopBookmark> petterResultType = new PetterResultType<BeautyShopBookmark>();
            List<BeautyShopBookmark> beautyShopBookmarks = new List<BeautyShopBookmark>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //beautyShopBookmark.StateFlag = StateFlags.Use;
            beautyShopBookmark.DateCreated = DateTime.Now;
            beautyShopBookmark.DateModified = DateTime.Now;

            db.BeautyShopBookmarks.Add(beautyShopBookmark);
            await db.SaveChangesAsync();

            beautyShopBookmarks.Add(beautyShopBookmark);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = beautyShopBookmarks;

            return Ok(petterResultType);
        }

        // DELETE: api/BeautyShopBookmarks/5
        [ResponseType(typeof(BeautyShopBookmark))]
        public async Task<IHttpActionResult> DeleteBeautyShopBookmark(int id)
        {
            BeautyShopBookmark beautyShopBookmark = await db.BeautyShopBookmarks.FindAsync(id);
            if (beautyShopBookmark == null)
            {
                return NotFound();
            }

            db.BeautyShopBookmarks.Remove(beautyShopBookmark);
            await db.SaveChangesAsync();

            return Ok(beautyShopBookmark);
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
            return db.BeautyShopBookmarks.Count(e => e.BeautyShopBookmarkNo == id) > 0;
        }
    }
}