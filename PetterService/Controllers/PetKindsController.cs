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
    public class PetKindsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PetKinds
        public IQueryable<PetKind> GetPetKinds()
        {
            return db.PetKinds;
        }

        // GET: api/PetKinds/5
        [ResponseType(typeof(PetKind))]
        public async Task<IHttpActionResult> GetPetKind(int id)
        {
            PetKind petKind = await db.PetKinds.FindAsync(id);
            if (petKind == null)
            {
                return NotFound();
            }

            return Ok(petKind);
        }

        // PUT: api/PetKinds/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPetKind(int id, PetKind petKind)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != petKind.PetKindNo)
            {
                return BadRequest();
            }

            db.Entry(petKind).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetKindExists(id))
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

        // POST: api/PetKinds
        [ResponseType(typeof(PetKind))]
        public async Task<IHttpActionResult> PostPetKind(PetKind petKind)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetKinds.Add(petKind);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = petKind.PetKindNo }, petKind);
        }

        // DELETE: api/PetKinds/5
        [ResponseType(typeof(PetKind))]
        public async Task<IHttpActionResult> DeletePetKind(int id)
        {
            PetKind petKind = await db.PetKinds.FindAsync(id);
            if (petKind == null)
            {
                return NotFound();
            }

            db.PetKinds.Remove(petKind);
            await db.SaveChangesAsync();

            return Ok(petKind);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PetKindExists(int id)
        {
            return db.PetKinds.Count(e => e.PetKindNo == id) > 0;
        }
    }
}