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
    public class MemberAccessesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/MemberAccesses
        public IQueryable<MemberAccess> GetMemberAccesses()
        {
            return db.MemberAccesses;
        }

        // GET: api/MemberAccesses/5
        [ResponseType(typeof(MemberAccess))]
        public async Task<IHttpActionResult> GetMemberAccess(int id)
        {
            MemberAccess memberAccess = await db.MemberAccesses.FindAsync(id);
            if (memberAccess == null)
            {
                return NotFound();
            }

            return Ok(memberAccess);
        }

        // PUT: api/MemberAccesses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMemberAccess(int id, MemberAccess memberAccess)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memberAccess.MemberAccessNo)
            {
                return BadRequest();
            }

            db.Entry(memberAccess).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberAccessExists(id))
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

        // POST: api/MemberAccesses
        [ResponseType(typeof(MemberAccess))]
        public async Task<IHttpActionResult> PostMemberAccess(MemberAccess memberAccess)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MemberAccesses.Add(memberAccess);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = memberAccess.MemberAccessNo }, memberAccess);
        }

        // DELETE: api/MemberAccesses/5
        [ResponseType(typeof(MemberAccess))]
        public async Task<IHttpActionResult> DeleteMemberAccess(int id)
        {
            MemberAccess memberAccess = await db.MemberAccesses.FindAsync(id);
            if (memberAccess == null)
            {
                return NotFound();
            }

            db.MemberAccesses.Remove(memberAccess);
            await db.SaveChangesAsync();

            return Ok(memberAccess);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberAccessExists(int id)
        {
            return db.MemberAccesses.Count(e => e.MemberAccessNo == id) > 0;
        }
    }
}