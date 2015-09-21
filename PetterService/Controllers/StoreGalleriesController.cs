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
    public class StoreGalleriesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleries
        public IQueryable<StoreGallery> GetStoreGalleries()
        {
            return db.StoreGalleries;
        }

        // GET: api/StoreGalleries/5
        [ResponseType(typeof(StoreGallery))]
        public async Task<IHttpActionResult> GetStoreGallery(int id)
        {
            StoreGallery storeGallery = await db.StoreGalleries.FindAsync(id);
            if (storeGallery == null)
            {
                return NotFound();
            }

            return Ok(storeGallery);
        }

        // PUT: api/StoreGalleries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreGallery(int id, StoreGallery storeGallery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeGallery.StoreGalleryNo)
            {
                return BadRequest();
            }

            db.Entry(storeGallery).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGalleryExists(id))
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

        // POST: api/StoreGalleries
        [ResponseType(typeof(StoreGallery))]
        public async Task<IHttpActionResult> PostStoreGallery(StoreGallery storeGallery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreGalleries.Add(storeGallery);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeGallery.StoreGalleryNo }, storeGallery);
        }

        // DELETE: api/StoreGalleries/5
        [ResponseType(typeof(StoreGallery))]
        public async Task<IHttpActionResult> DeleteStoreGallery(int id)
        {
            StoreGallery storeGallery = await db.StoreGalleries.FindAsync(id);
            if (storeGallery == null)
            {
                return NotFound();
            }

            db.StoreGalleries.Remove(storeGallery);
            await db.SaveChangesAsync();

            return Ok(storeGallery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGalleryExists(int id)
        {
            return db.StoreGalleries.Count(e => e.StoreGalleryNo == id) > 0;
        }
    }
}