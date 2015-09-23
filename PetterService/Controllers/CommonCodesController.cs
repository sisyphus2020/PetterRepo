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
    public class CommonCodesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/CommonCodes
        public IQueryable<CommonCode> GetCommonCodes()
        {
            return db.CommonCodes;
        }

        // GET: api/CommonCodes/5
        [ResponseType(typeof(CommonCode))]
        public async Task<IHttpActionResult> GetCommonCode(int id)
        {
            CommonCode commonCode = await db.CommonCodes.FindAsync(id);
            if (commonCode == null)
            {
                return NotFound();
            }

            return Ok(commonCode);
        }

        // PUT: api/CommonCodes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommonCode(int id, CommonCode commonCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commonCode.CodeNo)
            {
                return BadRequest();
            }

            db.Entry(commonCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommonCodeExists(id))
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

        // POST: api/CommonCodes
        [ResponseType(typeof(CommonCode))]
        public async Task<IHttpActionResult> PostCommonCode(CommonCode commonCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CommonCodes.Add(commonCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = commonCode.CodeNo }, commonCode);
        }

        // DELETE: api/CommonCodes/5
        [ResponseType(typeof(CommonCode))]
        public async Task<IHttpActionResult> DeleteCommonCode(int id)
        {
            CommonCode commonCode = await db.CommonCodes.FindAsync(id);
            if (commonCode == null)
            {
                return NotFound();
            }

            db.CommonCodes.Remove(commonCode);
            await db.SaveChangesAsync();

            return Ok(commonCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommonCodeExists(int id)
        {
            return db.CommonCodes.Count(e => e.CodeNo == id) > 0;
        }
    }
}