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
    public class PensionServicesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PensionServices
        public IQueryable<PensionService> GetPensionServices()
        {
            return db.PensionServices;
        }

        // GET: api/PensionServices/5
        [ResponseType(typeof(PensionService))]
        public async Task<IHttpActionResult> GetPensionService(int id)
        {
            PensionService pensionService = await db.PensionServices.FindAsync(id);
            if (pensionService == null)
            {
                return NotFound();
            }

            return Ok(pensionService);
        }

        // PUT: api/PensionServices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPensionService(int id, PensionService pensionService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pensionService.PensionServiceNo)
            {
                return BadRequest();
            }

            db.Entry(pensionService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PensionServiceExists(id))
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

        // POST: api/PensionServices
        [ResponseType(typeof(PensionService))]
        public async Task<IHttpActionResult> PostPensionService(PensionService pensionService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PensionServices.Add(pensionService);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pensionService.PensionServiceNo }, pensionService);
        }

        // DELETE: api/PensionServices/5
        [ResponseType(typeof(PensionService))]
        public async Task<IHttpActionResult> DeletePensionService(int id)
        {
            PensionService pensionService = await db.PensionServices.FindAsync(id);
            if (pensionService == null)
            {
                return NotFound();
            }

            db.PensionServices.Remove(pensionService);
            await db.SaveChangesAsync();

            return Ok(pensionService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PensionServiceExists(int id)
        {
            return db.PensionServices.Count(e => e.PensionServiceNo == id) > 0;
        }
    }
}