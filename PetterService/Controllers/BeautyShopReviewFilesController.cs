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
    public class BeautyShopReviewFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopReviewFiles
        public IQueryable<BeautyShopReviewFile> GetBeautyShopReviewFiles()
        {
            return db.BeautyShopReviewFiles;
        }

        // GET: api/BeautyShopReviewFiles/5
        [ResponseType(typeof(BeautyShopReviewFile))]
        public async Task<IHttpActionResult> GetBeautyShopReviewFile(int id)
        {
            BeautyShopReviewFile beautyShopReviewFile = await db.BeautyShopReviewFiles.FindAsync(id);
            if (beautyShopReviewFile == null)
            {
                return NotFound();
            }

            return Ok(beautyShopReviewFile);
        }

        // PUT: api/BeautyShopReviewFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShopReviewFile(int id, BeautyShopReviewFile beautyShopReviewFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShopReviewFile.BeautyShopReviewFileNo)
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
        [ResponseType(typeof(BeautyShopReviewFile))]
        public async Task<IHttpActionResult> PostBeautyShopReviewFile(BeautyShopReviewFile beautyShopReviewFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShopReviewFiles.Add(beautyShopReviewFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShopReviewFile.BeautyShopReviewFileNo }, beautyShopReviewFile);
        }

        // DELETE: api/BeautyShopReviewFiles/5
        [ResponseType(typeof(BeautyShopReviewFile))]
        public async Task<IHttpActionResult> DeleteBeautyShopReviewFile(int id)
        {
            BeautyShopReviewFile beautyShopReviewFile = await db.BeautyShopReviewFiles.FindAsync(id);
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
            return db.BeautyShopReviewFiles.Count(e => e.BeautyShopReviewFileNo == id) > 0;
        }
    }
}