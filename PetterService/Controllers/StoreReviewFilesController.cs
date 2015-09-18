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
    public class StoreReviewFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopReviewFiles
        public IQueryable<StoreReviewFile> GetStoreReviewFiles()
        {
            return db.BeautyShopReviewFiles;
        }

        // GET: api/BeautyShopReviewFiles/5
        [ResponseType(typeof(StoreReviewFile))]
        public async Task<IHttpActionResult> GetStoreReviewFile(int id)
        {
            StoreReviewFile beautyShopReviewFile = await db.BeautyShopReviewFiles.FindAsync(id);
            if (beautyShopReviewFile == null)
            {
                return NotFound();
            }

            return Ok(beautyShopReviewFile);
        }

        // PUT: api/BeautyShopReviewFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShopReviewFile(int id, StoreReviewFile beautyShopReviewFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShopReviewFile.StoreReviewFileNo)
            {
                return BadRequest();
            }

            db.Entry(beautyShopReviewFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeautyShopReviewFileExists(id))
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

        // POST: api/BeautyShopReviewFiles
        [ResponseType(typeof(StoreReviewFile))]
        public async Task<IHttpActionResult> PostBeautyShopReviewFile(StoreReviewFile beautyShopReviewFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShopReviewFiles.Add(beautyShopReviewFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShopReviewFile.StoreReviewFileNo }, beautyShopReviewFile);
        }

        // DELETE: api/BeautyShopReviewFiles/5
        [ResponseType(typeof(StoreReviewFile))]
        public async Task<IHttpActionResult> DeleteBeautyShopReviewFile(int id)
        {
            StoreReviewFile beautyShopReviewFile = await db.BeautyShopReviewFiles.FindAsync(id);
            if (beautyShopReviewFile == null)
            {
                return NotFound();
            }

            db.BeautyShopReviewFiles.Remove(beautyShopReviewFile);
            await db.SaveChangesAsync();

            return Ok(beautyShopReviewFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeautyShopReviewFileExists(int id)
        {
            return db.BeautyShopReviewFiles.Count(e => e.StoreReviewFileNo == id) > 0;
        }
    }
}