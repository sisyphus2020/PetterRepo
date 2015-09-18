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
    public class StoreServicesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreServices
        public IQueryable<StoreService> GetStoreServices()
        {
            return db.StoreServices;
        }

        // GET: api/StoreServices/5
        [ResponseType(typeof(StoreService))]
        public async Task<IHttpActionResult> GetStoreservice(int id)
        {
            StoreService Storeservice = await db.StoreServices.FindAsync(id);
            if (Storeservice == null)
            {
                return NotFound();
            }

            return Ok(Storeservice);
        }

        // PUT: api/StoreServices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreservice(int id, StoreService Storeservice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Storeservice.StoreServiceNo)
            {
                return BadRequest();
            }

            db.Entry(Storeservice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreserviceExists(id))
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

        // POST: api/StoreServices
        [ResponseType(typeof(StoreService))]
        public async Task<IHttpActionResult> PostStoreservice(StoreService Storeservice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreServices.Add(Storeservice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Storeservice.StoreServiceNo }, Storeservice);
        }

        // DELETE: api/StoreServices/5
        [ResponseType(typeof(StoreService))]
        public async Task<IHttpActionResult> DeleteStoreservice(int id)
        {
            StoreService Storeservice = await db.StoreServices.FindAsync(id);
            if (Storeservice == null)
            {
                return NotFound();
            }

            db.StoreServices.Remove(Storeservice);
            await db.SaveChangesAsync();

            return Ok(Storeservice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreserviceExists(int id)
        {
            return db.StoreServices.Count(e => e.StoreServiceNo == id) > 0;
        }
    }
}