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
    public class PensionHolidaysController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PensionHolidays
        public IQueryable<PensionHoliday> GetPensionHolidays()
        {
            return db.PensionHolidays;
        }

        // GET: api/PensionHolidays/5
        [ResponseType(typeof(PensionHoliday))]
        public async Task<IHttpActionResult> GetPensionHoliday(int id)
        {
            PensionHoliday pensionHoliday = await db.PensionHolidays.FindAsync(id);
            if (pensionHoliday == null)
            {
                return NotFound();
            }

            return Ok(pensionHoliday);
        }

        // PUT: api/PensionHolidays/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPensionHoliday(int id, PensionHoliday pensionHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pensionHoliday.PensionHolidayNo)
            {
                return BadRequest();
            }

            db.Entry(pensionHoliday).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PensionHolidayExists(id))
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

        // POST: api/PensionHolidays
        [ResponseType(typeof(PensionHoliday))]
        public async Task<IHttpActionResult> PostPensionHoliday(PensionHoliday pensionHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PensionHolidays.Add(pensionHoliday);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pensionHoliday.PensionHolidayNo }, pensionHoliday);
        }

        // DELETE: api/PensionHolidays/5
        [ResponseType(typeof(PensionHoliday))]
        public async Task<IHttpActionResult> DeletePensionHoliday(int id)
        {
            PensionHoliday pensionHoliday = await db.PensionHolidays.FindAsync(id);
            if (pensionHoliday == null)
            {
                return NotFound();
            }

            db.PensionHolidays.Remove(pensionHoliday);
            await db.SaveChangesAsync();

            return Ok(pensionHoliday);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PensionHolidayExists(int id)
        {
            return db.PensionHolidays.Count(e => e.PensionHolidayNo == id) > 0;
        }
    }
}