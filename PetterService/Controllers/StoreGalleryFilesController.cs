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
    public class StoreGalleryFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleryFiles
        public IQueryable<StoreGalleryFile> GetStoreGalleryFiles()
        {
            return db.StoreGalleryFiles;
        }

        // GET: api/StoreGalleryFiles/5
        [ResponseType(typeof(StoreGalleryFile))]
        public async Task<IHttpActionResult> GetStoreGalleryFile(int id)
        {
            StoreGalleryFile storeGalleryFile = await db.StoreGalleryFiles.FindAsync(id);
            if (storeGalleryFile == null)
            {
                return NotFound();
            }

            return Ok(storeGalleryFile);
        }

        // PUT: api/StoreGalleryFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreGalleryFile(int id, StoreGalleryFile storeGalleryFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeGalleryFile.StoreGalleryFileNo)
            {
                return BadRequest();
            }

            db.Entry(storeGalleryFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGalleryFileExists(id))
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

        // POST: api/StoreGalleryFiles
        [ResponseType(typeof(StoreGalleryFile))]
        public async Task<IHttpActionResult> PostStoreGalleryFile(StoreGalleryFile storeGalleryFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreGalleryFiles.Add(storeGalleryFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeGalleryFile.StoreGalleryFileNo }, storeGalleryFile);
        }

        // DELETE: api/StoreGalleryFiles/5
        [ResponseType(typeof(StoreGalleryFile))]
        public async Task<IHttpActionResult> DeleteStoreGalleryFile(int id)
        {
            StoreGalleryFile storeGalleryFile = await db.StoreGalleryFiles.FindAsync(id);
            if (storeGalleryFile == null)
            {
                return NotFound();
            }

            db.StoreGalleryFiles.Remove(storeGalleryFile);
            await db.SaveChangesAsync();

            return Ok(storeGalleryFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGalleryFileExists(int id)
        {
            return db.StoreGalleryFiles.Count(e => e.StoreGalleryFileNo == id) > 0;
        }
    }
}