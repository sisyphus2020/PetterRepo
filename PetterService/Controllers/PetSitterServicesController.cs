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
    public class PetSitterServicesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PetSitterServices
        public IQueryable<PetSitterService> GetPetSitterServices()
        {
            return db.PetSitterServices;
        }

        // GET: api/PetSitterServices/5
        [ResponseType(typeof(PetSitterService))]
        public async Task<IHttpActionResult> GetPetSitterService(int id)
        {
            PetSitterService petSitterService = await db.PetSitterServices.FindAsync(id);
            if (petSitterService == null)
            {
                return NotFound();
            }

            return Ok(petSitterService);
        }

        // PUT: api/PetSitterServices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPetSitterService(int id, PetSitterService petSitterService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != petSitterService.PetSitterServiceNo)
            {
                return BadRequest();
            }

            db.Entry(petSitterService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetSitterServiceExists(id))
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

        // POST: api/PetSitterServices
        [ResponseType(typeof(PetSitterService))]
        public async Task<IHttpActionResult> PostPetSitterService(PetSitterService petSitterService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetSitterServices.Add(petSitterService);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = petSitterService.PetSitterServiceNo }, petSitterService);
        }

        // DELETE: api/PetSitterServices/5
        [ResponseType(typeof(PetSitterService))]
        public async Task<IHttpActionResult> DeletePetSitterService(int id)
        {
            PetSitterService petSitterService = await db.PetSitterServices.FindAsync(id);
            if (petSitterService == null)
            {
                return NotFound();
            }

            db.PetSitterServices.Remove(petSitterService);
            await db.SaveChangesAsync();

            return Ok(petSitterService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PetSitterServiceExists(int id)
        {
            return db.PetSitterServices.Count(e => e.PetSitterServiceNo == id) > 0;
        }
    }
}