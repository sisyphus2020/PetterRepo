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
using System.Data.Entity.Spatial;
using PetterService.Common;

namespace PetterService.Controllers
{
    public class PetSittersController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/PetSitters
        public IEnumerable<PetSitter> GetPensions([FromUri] PetterRequestType petterRequestType)
        {
            List<PetSitter> list = new List<PetSitter>();
            DbGeography currentLocation = DbGeography.FromText(string.Format("POINT({0} {1})", petterRequestType.Latitude, petterRequestType.Longitude));
            int distance = petterRequestType.Distance;

            var PetSitter = db.PetSitters.AsEnumerable();

            // 검색 조건 
            if (!String.IsNullOrWhiteSpace(petterRequestType.Search))
            {
                PetSitter = PetSitter.Where(p => p.PetSitterName != null && p.PetSitterName.Contains(petterRequestType.Search));
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 거리
                case "distance":
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderBy(p => p.Coordinate.Distance(currentLocation))
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 리뷰수
                case "reviewcount":
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.ReviewCount)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 점수
                case "grade":
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.Grade)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 즐겨찾기
                case "bookmark":
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.Bookmark)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 기본
                default:
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.CompanyNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
            }
            #endregion 정렬방식

            return list;
        }

        // GET: api/PetSitters/5
        [ResponseType(typeof(PetterResultType<PetSitterDTO>))]
        public async Task<IHttpActionResult> GetPetSitter(int id)
        {
            PetterResultType<PetSitterDTO> petterResultType = new PetterResultType<PetSitterDTO>();

            var petSitterDatail = await db.PetSitters.Where(p => p.PetSitterNo == id).Select(p => new PetSitterDTO
            {
                PetSitterNo = p.PetSitterNo,
                CompanyNo = p.CompanyNo,
                PetSitterName = p.PetSitterName,
                PetSitterAddr = p.PetSitterAddr,
                PictureName = p.PictureName,
                PicturePath = p.PicturePath,
                StartHours = p.StartHours,
                EndHours = p.EndHours,
                Introduction = p.Introduction,
                Coordinate = p.Coordinate,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Grade = p.Grade,
                ReviewCount = p.ReviewCount,
                Bookmark = p.Bookmark,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                PetSitterServices = p.PetSitterServices.ToList(),
                PetSitterHolidays = p.PetSitterHolidays.ToList()
            }).SingleOrDefaultAsync();

            if (petSitterDatail == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = petSitterDatail;
            return Ok(petterResultType);
        }

        // PUT: api/PetSitters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPetSitter(int id, PetSitter petSitter)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != petSitter.PetSitterNo)
            {
                return BadRequest();
            }

            db.Entry(petSitter).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetSitterExists(id))
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

        // POST: api/PetSitters
        [ResponseType(typeof(PetSitter))]
        public async Task<IHttpActionResult> PostPetSitter(PetSitter petSitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetSitters.Add(petSitter);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = petSitter.PetSitterNo }, petSitter);
        }

        // DELETE: api/PetSitters/5
        [ResponseType(typeof(PetSitter))]
        public async Task<IHttpActionResult> DeletePetSitter(int id)
        {
            PetSitter petSitter = await db.PetSitters.FindAsync(id);
            if (petSitter == null)
            {
                return NotFound();
            }

            db.PetSitters.Remove(petSitter);
            await db.SaveChangesAsync();

            return Ok(petSitter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PetSitterExists(int id)
        {
            return db.PetSitters.Count(e => e.PetSitterNo == id) > 0;
        }
    }
}