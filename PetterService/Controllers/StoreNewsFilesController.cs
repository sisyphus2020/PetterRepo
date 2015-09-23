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
    public class StoreNewsFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreNewsFiles
        public IQueryable<StoreNewsFile> GetStoreNewsFiles()
        {
            return db.StoreNewsFiles;
        }

        // GET: api/StoreNewsFiles/5
        [ResponseType(typeof(StoreNewsFile))]
        public async Task<IHttpActionResult> GetStoreNewsFile(int id)
        {
            StoreNewsFile StoreNewsFile = await db.StoreNewsFiles.FindAsync(id);
            if (StoreNewsFile == null)
            {
                return NotFound();
            }

            return Ok(StoreNewsFile);
        }

        // PUT: api/StoreNewsFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreNewsFile(int id, StoreNewsFile StoreNewsFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != StoreNewsFile.StoreNewsFileNo)
            {
                return BadRequest();
            }

            db.Entry(StoreNewsFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreNewsFileExists(id))
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

        // POST: api/StoreNewsFiles
        [ResponseType(typeof(StoreNewsFile))]
        public async Task<IHttpActionResult> PostStoreNewsFile(StoreNewsFile StoreNewsFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreNewsFiles.Add(StoreNewsFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = StoreNewsFile.StoreNewsFileNo }, StoreNewsFile);
        }

        // DELETE: api/StoreNewsFiles/5
        [ResponseType(typeof(StoreNewsFile))]
        public async Task<IHttpActionResult> DeleteStoreNewsFile(int id)
        {
            StoreNewsFile StoreNewsFile = await db.StoreNewsFiles.FindAsync(id);
            if (StoreNewsFile == null)
            {
                return NotFound();
            }

            db.StoreNewsFiles.Remove(StoreNewsFile);
            await db.SaveChangesAsync();

            return Ok(StoreNewsFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreNewsFileExists(int id)
        {
            return db.StoreNewsFiles.Count(e => e.StoreNewsFileNo == id) > 0;
        }
    }
}