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
    public class NoticeFilesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/NoticeFiles
        public IQueryable<NoticeFile> GetNoticeFiles()
        {
            return db.NoticeFiles;
        }

        // GET: api/NoticeFiles/5
        [ResponseType(typeof(NoticeFile))]
        public async Task<IHttpActionResult> GetNoticeFile(int id)
        {
            NoticeFile noticeFile = await db.NoticeFiles.FindAsync(id);
            if (noticeFile == null)
            {
                return NotFound();
            }

            return Ok(noticeFile);
        }

        // PUT: api/NoticeFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNoticeFile(int id, NoticeFile noticeFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != noticeFile.NoticeFileNo)
            {
                return BadRequest();
            }

            db.Entry(noticeFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeFileExists(id))
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

        // POST: api/NoticeFiles
        [ResponseType(typeof(NoticeFile))]
        public async Task<IHttpActionResult> PostNoticeFile(NoticeFile noticeFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NoticeFiles.Add(noticeFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = noticeFile.NoticeFileNo }, noticeFile);
        }

        // DELETE: api/NoticeFiles/5
        [ResponseType(typeof(NoticeFile))]
        public async Task<IHttpActionResult> DeleteNoticeFile(int id)
        {
            NoticeFile noticeFile = await db.NoticeFiles.FindAsync(id);
            if (noticeFile == null)
            {
                return NotFound();
            }

            db.NoticeFiles.Remove(noticeFile);
            await db.SaveChangesAsync();

            return Ok(noticeFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NoticeFileExists(int id)
        {
            return db.NoticeFiles.Count(e => e.NoticeFileNo == id) > 0;
        }
    }
}