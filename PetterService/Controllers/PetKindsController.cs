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
using PetterService.Common;

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

        // GET: api/PetKinds
        [Route("api/PetKinds/{petCategory}")]
        [ResponseType(typeof(PetterResultType<PetKind>))]
        public async Task<IHttpActionResult> GetPetKinds(string petCategory)
        {
            PetterResultType<PetKind> petterResultType = new PetterResultType<PetKind>();
            //List<PetKind> petKinds = new List<PetKind>();

            var petKinds = await db.PetKinds.Where(p => p.PetCategory == petCategory).ToListAsync();

            if (petKinds == null)
            {
                return NotFound();
            }

            //petKinds.Add(petKind);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = petKinds;

            return Ok(petterResultType);
        }

        [Route("api/PetKinds/{petCategory}/{petCode}/")]
        [ResponseType(typeof(PetKind))]
        public async Task<IHttpActionResult> GetPetKind(string petCategory, string petCode)
        {
            PetterResultType<PetKind> petterResultType = new PetterResultType<PetKind>();
            List<PetKind> petKinds = new List<PetKind>();

            PetKind petKind = await db.PetKinds.Where(p => p.PetCategory == petCategory & p.PetCode == petCode).SingleOrDefaultAsync();

            if (petKind == null)
            {
                return NotFound();
            }

            petKinds.Add(petKind);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = petKinds;

            return Ok(petterResultType);
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