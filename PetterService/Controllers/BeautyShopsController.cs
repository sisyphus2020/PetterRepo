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
    public class BeautyShopsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShops
        public IEnumerable<BeautyShop> GetBeautyShop(int page = 1, int itemsPerPage = 10, string sortBy = "basic", bool reverse = false, string search = null)
        {
            List<BeautyShop> list = new List<BeautyShop>();

            var beautyShop = db.BeautyShops.AsEnumerable();

            // 검색 조건 
            if (!String.IsNullOrEmpty(search))
            {
                beautyShop = beautyShop.Where(p => p.BeautyShopName != null && p.BeautyShopName.Contains(search));
            }

            #region 정렬 방식
            switch (sortBy)
            {
                // 거리
                case "distance":
                    {
                        list = beautyShop
                            .OrderByDescending(p => p.BeautyShopNo)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage).ToList();
                        break;
                    }
                // 리뷰수
                case "reviewcount":
                    {
                        list = beautyShop
                            .OrderByDescending(p => p.ReviewCount)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage).ToList();
                        break;
                    }
                // 점수
                case "grade":
                    {
                        list = beautyShop
                            .OrderByDescending(p => p.Grade)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage).ToList();
                        break;
                    }
                // 즐겨찾기
                case "bookmark":
                    {
                        list = beautyShop
                            .OrderByDescending(p => p.Bookmark)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage).ToList();
                        break;
                    }
                // 기본
                default:
                    {
                        list = beautyShop
                             .OrderByDescending(p => p.CompanyNo)
                             .Skip((page - 1) * itemsPerPage)
                             .Take(itemsPerPage).ToList();
                        break;
                    }
            }
            #endregion 정렬방식

            return list;
        }

        // GET: api/BeautyShops/5
        [ResponseType(typeof(BeautyShop))]
        public async Task<IHttpActionResult> GetBeautyShop(int id)
        {
            var BeautyShopDatails = await db.BeautyShops.Where(p => p.BeautyShopNo == id).Select(p => new BeautyShopDTO
            {
                BeautyShopNo = p.BeautyShopNo,
                CompanyNo = p.CompanyNo,
                BeautyShopName = p.BeautyShopName,
                BeautyShopAddr = p.BeautyShopAddr,
                PictureName = p.PictureName,
                PicturePath = p.PicturePath,
                StartBeautyShopHours = p.StartBeautyShopHours,
                EndBeautyShopHours = p.EndBeautyShopHours,
                Introduction = p.Introduction,
                Coordinate = p.Coordinate,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Grade = p.Grade,
                ReviewCount = p.ReviewCount,
                Bookmark = p.Bookmark,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                BeautyShopServices = p.BeautyShopServices.ToList(),
                BeautyShopHolidays = p.BeautyShopHolidays.ToList()
            }).SingleOrDefaultAsync();

            if (BeautyShopDatails == null)
            {
                return NotFound();
            }

            return Ok(BeautyShopDatails);
        }

        // PUT: api/BeautyShops/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBeautyShop(int id, BeautyShop beautyShop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beautyShop.BeautyShopNo)
            {
                return BadRequest();
            }

            db.Entry(beautyShop).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeautyShopExists(id))
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

        // POST: api/BeautyShops
        [ResponseType(typeof(BeautyShop))]
        public async Task<IHttpActionResult> PostBeautyShop(BeautyShop beautyShop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeautyShops.Add(beautyShop);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beautyShop.BeautyShopNo }, beautyShop);
        }

        // DELETE: api/BeautyShops/5
        [ResponseType(typeof(BeautyShop))]
        public async Task<IHttpActionResult> DeleteBeautyShop(int id)
        {
            BeautyShop beautyShop = await db.BeautyShops.FindAsync(id);
            if (beautyShop == null)
            {
                return NotFound();
            }

            db.BeautyShops.Remove(beautyShop);
            await db.SaveChangesAsync();

            return Ok(beautyShop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeautyShopExists(int id)
        {
            return db.BeautyShops.Count(e => e.BeautyShopNo == id) > 0;
        }
    }
}