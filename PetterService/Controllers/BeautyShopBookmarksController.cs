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
        [ResponseType(typeof(BeautyShopBookmark))]
        public async Task<IHttpActionResult> PostBeautyShopBookmark(BeautyShopBookmark beautyShopBookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShopBookmarks.Add(beautyShopBookmark);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShopBookmark.BeautyShopBookmarkNo }, beautyShopBookmark);
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