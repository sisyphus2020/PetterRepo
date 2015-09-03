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
    public class PetSitterHolidaysController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PetSitterHolidays
        public IQueryable<PetSitterHoliday> GetPetSitterHolidays()
        {
            return db.PetSitterHolidays;
        }

        // GET: api/PetSitterHolidays/5
        [ResponseType(typeof(PetSitterHoliday))]
        public async Task<IHttpActionResult> GetPetSitterHoliday(int id)
        {
            PetSitterHoliday petSitterHoliday = await db.PetSitterHolidays.FindAsync(id);
            if (petSitterHoliday == null)
            {
                return NotFound();
            }

            return Ok(petSitterHoliday);
        }

        // PUT: api/PetSitterHolidays/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPetSitterHoliday(int id, PetSitterHoliday petSitterHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != petSitterHoliday.PetSitterHolidayNo)
            {
                return BadRequest();
            }

            db.Entry(petSitterHoliday).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetSitterHolidayExists(id))
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

        // POST: api/PetSitterHolidays
        [ResponseType(typeof(PetSitterHoliday))]
        public async Task<IHttpActionResult> PostPetSitterHoliday(PetSitterHoliday petSitterHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetSitterHolidays.Add(petSitterHoliday);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = petSitterHoliday.PetSitterHolidayNo }, petSitterHoliday);
        }

        // DELETE: api/PetSitterHolidays/5
        [ResponseType(typeof(PetSitterHoliday))]
        public async Task<IHttpActionResult> DeletePetSitterHoliday(int id)
        {
            PetSitterHoliday petSitterHoliday = await db.PetSitterHolidays.FindAsync(id);
            if (petSitterHoliday == null)
            {
                return NotFound();
            }

            db.PetSitterHolidays.Remove(petSitterHoliday);
            await db.SaveChangesAsync();

            return Ok(petSitterHoliday);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PetSitterHolidayExists(int id)
        {
            return db.PetSitterHolidays.Count(e => e.PetSitterHolidayNo == id) > 0;
        }
    }
}