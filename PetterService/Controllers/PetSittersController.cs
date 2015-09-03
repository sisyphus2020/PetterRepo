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
    public class PetSittersController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PetSitters
        public IQueryable<PetSitter> GetPetSitters()
        {
            return db.PetSitters;
        }

        // GET: api/PetSitters/5
        [ResponseType(typeof(PetSitter))]
        public async Task<IHttpActionResult> GetPetSitter(int id)
        {
            PetSitter petSitter = await db.PetSitters.FindAsync(id);
            if (petSitter == null)
            {
                return NotFound();
            }

            return Ok(petSitter);
        }

        // PUT: api/PetSitters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPetSitter(int id, PetSitter petSitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != petSitter.PetSitterNo)
            {
                return BadRequest();
            }

            db.Entry(petSitter).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetSitterExists(id))
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

        // POST: api/PetSitters
        [ResponseType(typeof(PetSitter))]
        public async Task<IHttpActionResult> PostPetSitter(PetSitter petSitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetSitters.Add(petSitter);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = petSitter.PetSitterNo }, petSitter);
        }

        // DELETE: api/PetSitters/5
        [ResponseType(typeof(PetSitter))]
        public async Task<IHttpActionResult> DeletePetSitter(int id)
        {
            PetSitter petSitter = await db.PetSitters.FindAsync(id);
            if (petSitter == null)
            {
                return NotFound();
            }

            db.PetSitters.Remove(petSitter);
            await db.SaveChangesAsync();

            return Ok(petSitter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PetSitterExists(int id)
        {
            return db.PetSitters.Count(e => e.PetSitterNo == id) > 0;
        }
    }
}