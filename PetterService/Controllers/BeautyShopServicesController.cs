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
    public class BeautyShopServicesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopServices
        public IQueryable<BeautyShopService> GetBeautyShopServices()
        {
            return db.BeautyShopServices;
        }

        // GET: api/BeautyShopServices/5
        [ResponseType(typeof(BeautyShopService))]
        public async Task<IHttpActionResult> GetBeautyShopService(int id)
        {
            BeautyShopService beautyShopService = await db.BeautyShopServices.FindAsync(id);
            if (beautyShopService == null)
            {
                return NotFound();
            }

            return Ok(beautyShopService);
        }

        // PUT: api/BeautyShopServices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShopService(int id, BeautyShopService beautyShopService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShopService.BeautyShopServiceNo)
            {
                return BadRequest();
            }

            db.Entry(beautyShopService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeautyShopServiceExists(id))
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

        // POST: api/BeautyShopServices
        [ResponseType(typeof(BeautyShopService))]
        public async Task<IHttpActionResult> PostBeautyShopService(BeautyShopService beautyShopService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShopServices.Add(beautyShopService);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShopService.BeautyShopServiceNo }, beautyShopService);
        }

        // DELETE: api/BeautyShopServices/5
        [ResponseType(typeof(BeautyShopService))]
        public async Task<IHttpActionResult> DeleteBeautyShopService(int id)
        {
            BeautyShopService beautyShopService = await db.BeautyShopServices.FindAsync(id);
            if (beautyShopService == null)
            {
                return NotFound();
            }

            db.BeautyShopServices.Remove(beautyShopService);
            await db.SaveChangesAsync();

            return Ok(beautyShopService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeautyShopServiceExists(int id)
        {
            return db.BeautyShopServices.Count(e => e.BeautyShopServiceNo == id) > 0;
        }
    }
}