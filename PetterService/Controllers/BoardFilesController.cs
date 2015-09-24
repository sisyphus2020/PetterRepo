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
    public class BoardFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BoardFiles
        public IQueryable<BoardFile> GetBoardFiles()
        {
            return db.BoardFiles;
        }

        // GET: api/BoardFiles/5
        [ResponseType(typeof(BoardFile))]
        public async Task<IHttpActionResult> GetBoardFile(int id)
        {
            BoardFile BoardFile = await db.BoardFiles.FindAsync(id);
            if (BoardFile == null)
            {
                return NotFound();
            }

            return Ok(BoardFile);
        }

        // PUT: api/BoardFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBoardFile(int id, BoardFile BoardFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != BoardFile.BoardFileNo)
            {
                return BadRequest();
            }

            db.Entry(BoardFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardFileExists(id))
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

        // POST: api/BoardFiles
        [ResponseType(typeof(BoardFile))]
        public async Task<IHttpActionResult> PostBoardFile(BoardFile BoardFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BoardFiles.Add(BoardFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = BoardFile.BoardFileNo }, BoardFile);
        }

        // DELETE: api/BoardFiles/5
        [ResponseType(typeof(BoardFile))]
        public async Task<IHttpActionResult> DeleteBoardFile(int id)
        {
            BoardFile BoardFile = await db.BoardFiles.FindAsync(id);
            if (BoardFile == null)
            {
                return NotFound();
            }

            db.BoardFiles.Remove(BoardFile);
            await db.SaveChangesAsync();

            return Ok(BoardFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BoardFileExists(int id)
        {
            return db.BoardFiles.Count(e => e.BoardFileNo == id) > 0;
        }
    }
}