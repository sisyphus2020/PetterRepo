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
    public class BeautyShopHolidaysController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopHolidays
        public IQueryable<BeautyShopHoliday> GetBeautyShopHolidays()
        {
            return db.BeautyShopHolidays;
        }

        // GET: api/BeautyShopHolidays/5
        [ResponseType(typeof(BeautyShopHoliday))]
        public async Task<IHttpActionResult> GetBeautyShopHoliday(int id)
        {
            BeautyShopHoliday beautyShopHoliday = await db.BeautyShopHolidays.FindAsync(id);
            if (beautyShopHoliday == null)
            {
                return NotFound();
            }

            return Ok(beautyShopHoliday);
        }

        // PUT: api/BeautyShopHolidays/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShopHoliday(int id, BeautyShopHoliday beautyShopHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShopHoliday.BeautyShopHolidayNo)
            {
                return BadRequest();
            }

            db.Entry(beautyShopHoliday).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeautyShopHolidayExists(id))
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

        // POST: api/BeautyShopHolidays
        [ResponseType(typeof(BeautyShopHoliday))]
        public async Task<IHttpActionResult> PostBeautyShopHoliday(BeautyShopHoliday beautyShopHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShopHolidays.Add(beautyShopHoliday);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShopHoliday.BeautyShopHolidayNo }, beautyShopHoliday);
        }

        // DELETE: api/BeautyShopHolidays/5
        [ResponseType(typeof(BeautyShopHoliday))]
        public async Task<IHttpActionResult> DeleteBeautyShopHoliday(int id)
        {
            BeautyShopHoliday beautyShopHoliday = await db.BeautyShopHolidays.FindAsync(id);
            if (beautyShopHoliday == null)
            {
                return NotFound();
            }

            db.BeautyShopHolidays.Remove(beautyShopHoliday);
            await db.SaveChangesAsync();

            return Ok(beautyShopHoliday);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeautyShopHolidayExists(int id)
        {
            return db.BeautyShopHolidays.Count(e => e.BeautyShopHolidayNo == id) > 0;
        }
    }
}