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
    public class StoreHolidaysController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreHolidays
        public IQueryable<StoreHoliday> GetStoreHolidays()
        {
            return db.StoreHolidays;
        }

        // GET: api/StoreHolidays/5
        [ResponseType(typeof(StoreHoliday))]
        public async Task<IHttpActionResult> GetStoreHoliday(int id)
        {
            StoreHoliday storeHoliday = await db.StoreHolidays.FindAsync(id);
            if (storeHoliday == null)
            {
                return NotFound();
            }

            return Ok(storeHoliday);
        }

        // PUT: api/StoreHolidays/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutstoreHoliday(int id, StoreHoliday storeHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeHoliday.StoreHolidayNo)
            {
                return BadRequest();
            }

            db.Entry(storeHoliday).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!storeHolidayExists(id))
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

        // POST: api/StoreHolidays
        [ResponseType(typeof(StoreHoliday))]
        public async Task<IHttpActionResult> PoststoreHoliday(StoreHoliday storeHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreHolidays.Add(storeHoliday);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = storeHoliday.StoreHolidayNo }, storeHoliday);
        }

        // DELETE: api/StoreHolidays/5
        [ResponseType(typeof(StoreHoliday))]
        public async Task<IHttpActionResult> DeletestoreHoliday(int id)
        {
            StoreHoliday storeHoliday = await db.StoreHolidays.FindAsync(id);
            if (storeHoliday == null)
            {
                return NotFound();
            }

            db.StoreHolidays.Remove(storeHoliday);
            await db.SaveChangesAsync();

            return Ok(storeHoliday);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool storeHolidayExists(int id)
        {
            return db.StoreHolidays.Count(e => e.StoreHolidayNo == id) > 0;
        }
    }
}