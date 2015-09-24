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
    public class StoreQuestionsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreQuestions
        public IQueryable<StoreQuestion> GetStoreQuestions()
        {
            return db.StoreQuestions;
        }

        // GET: api/StoreQuestions/5
        [ResponseType(typeof(StoreQuestion))]
        public async Task<IHttpActionResult> GetStoreQuestion(int id)
        {
            StoreQuestion storeQuestion = await db.StoreQuestions.FindAsync(id);
            if (storeQuestion == null)
            {
                return NotFound();
            }

            return Ok(storeQuestion);
        }

        // PUT: api/StoreQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStoreQuestion(int id, StoreQuestion storeQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storeQuestion.StoreNo)
            {
                return BadRequest();
            }

            db.Entry(storeQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreQuestionExists(id))
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

        // POST: api/StoreQuestions
        [ResponseType(typeof(StoreQuestion))]
        public async Task<IHttpActionResult> PostStoreQuestion(StoreQuestion storeQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreQuestions.Add(storeQuestion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreQuestionExists(storeQuestion.StoreNo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = storeQuestion.StoreNo }, storeQuestion);
        }

        // DELETE: api/StoreQuestions/5
        [ResponseType(typeof(StoreQuestion))]
        public async Task<IHttpActionResult> DeleteStoreQuestion(int id)
        {
            StoreQuestion storeQuestion = await db.StoreQuestions.FindAsync(id);
            if (storeQuestion == null)
            {
                return NotFound();
            }

            db.StoreQuestions.Remove(storeQuestion);
            await db.SaveChangesAsync();

            return Ok(storeQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreQuestionExists(int id)
        {
            return db.StoreQuestions.Count(e => e.StoreNo == id) > 0;
        }
    }
}