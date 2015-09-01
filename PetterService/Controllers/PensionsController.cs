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
using System.Web.Hosting;

namespace PetterService.Controllers
{
    public class PensionsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/Pensions
        public IEnumerable<Pension> GetPensions([FromUri] PetterRequestType petterRequestType)
        {
            List<Pension> list = new List<Pension>();
            DbGeography currentLocation = DbGeography.FromText(string.Format("POINT({0} {1})", (object)petterRequestType.Latitude, (object)petterRequestType.Longitude));
            int distance = petterRequestType.Distance;
            IEnumerable<Pension> source = Enumerable.AsEnumerable<Pension>((IEnumerable<Pension>)this.db.Pensions);

            var Pension = db.Pensions.AsEnumerable();

            // 검색 조건 
            if (!String.IsNullOrWhiteSpace(petterRequestType.Search))
            {
                Pension = Pension.Where(p => p.PensionName != null && p.PensionName.Contains(petterRequestType.Search));
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 거리
                case "distance":
                    {
                        list = Pension
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderBy(p => p.Coordinate.Distance(currentLocation))
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 리뷰수
                case "reviewcount":
                    {
                        list = Pension
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.ReviewCount)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 점수
                case "grade":
                    {
                        list = Pension
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.Grade)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 즐겨찾기
                case "bookmark":
                    {
                        list = Pension
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.Bookmark)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 기본
                default:
                    {
                        list = Pension
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

        // GET: api/Pensions/5
        [ResponseType(typeof(Pension))]
        public async Task<IHttpActionResult> GetPension(int id)
        {
            Pension pension = await db.Pensions.FindAsync(id);
            if (pension == null)
            {
                return NotFound();
            }

            return Ok(pension);
        }

        // PUT: api/Pensions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPension(int id, Pension pension)
        {
            PetterResultType<Pension> PetterResultType = new PetterResultType<Pension>();
            List<PensionService> pensionServices = new List<PensionService>();
            List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            BeautyShop beautyShop = await db.BeautyShops.FindAsync(id);
            if (beautyShop == null)
            {
                return NotFound();
            }

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var folder = HostingEnvironment.MapPath(UploadPath.BeautyShopPath);
            Utilities.CreateDirectory(folder);

            var provider = new CustomMultipartFormDataStreamProvider(folder);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                // Show all the key-value pairs.
                foreach (var key in provider.FormData)
                {
                    foreach (var val in provider.FormData.GetValues(key.ToString()))
                    {
                        var item = HttpUtility.UrlDecode(val);
                        #region case
                        switch (key.ToString())
                        {
                            case "BeautyShopNo":
                                beautyShop.BeautyShopNo = int.Parse(item);
                                break;
                            case "CompanyNo":
                                beautyShop.CompanyNo = int.Parse(item);
                                break;
                            case "BeautyShopName":
                                beautyShop.BeautyShopName = item;
                                break;
                            case "BeautyShopAddr":
                                beautyShop.BeautyShopAddr = item;
                                break;
                            case "PictureName":
                                beautyShop.PictureName = item;
                                break;
                            case "PicturePath":
                                beautyShop.PicturePath = item;
                                break;
                            case "StartBeautyShop":
                                beautyShop.StartBeautyShop = item;
                                break;
                            case "EndBeautyShop":
                                beautyShop.EndBeautyShop = item;
                                break;
                            case "Introduction":
                                beautyShop.Introduction = item;
                                break;
                            case "Coordinate":
                                //beautyShop.Coordinate = item;
                                break;
                            case "Grade":
                                beautyShop.Grade = int.Parse(item);
                                break;
                            case "ReviewCount":
                                beautyShop.ReviewCount = int.Parse(item);
                                break;
                            case "Bookmark":
                                beautyShop.Bookmark = int.Parse(item);
                                break;
                            case "BeautyShopServices":
                                shopService = item;
                                break;
                            case "BeautyShopHolidays":
                                shopHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion case
                    }
                }

                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine("Server file name: " + file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    beautyShop.PictureName = file.Headers.ContentDisposition.FileName;
                    beautyShop.PicturePath = file.LocalFileName;
                    //beautyShop.DateCreated = DateTime.Now;
                    beautyShop.DateModified = DateTime.Now;

                    string thumbnamil = Path.GetFileNameWithoutExtension(file.LocalFileName) + "_thumbnail" + Path.GetExtension(file.LocalFileName);

                    Utilities.ResizeImage(file.LocalFileName, thumbnamil, FileSize.BeautyShopWidth, FileSize.BeautyShopHeight, System.Drawing.Imaging.ImageFormat.Png);
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

                // BeautyShopServic 
                #region Insert BeautyShopServic
                var sc = new BeautyShopServicesController();
                await sc.DeleteBeautyShopServicByBeautyShopNo(beautyShop.BeautyShopNo);

                if (!string.IsNullOrEmpty(shopService))
                {
                    var arr = HttpUtility.UrlDecode(shopService.ToString()).Split(',');

                    if (arr.Count() > 0)
                    {
                        foreach (var item in arr)
                        {
                            beautyShopService.BeautyShopNo = beautyShop.BeautyShopNo;
                            beautyShopService.BeautyShopServiceCode = int.Parse(item);

                            db.BeautyShopServices.Add(beautyShopService);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                #endregion Insert BeautyShopServic

                // BeautyShopServic 
                #region Insert BeautyShopHoliday
                var hc = new BeautyShopHolidaysController();
                await hc.DeleteBeautyShopHolidayByBeautyShopNo(beautyShop.BeautyShopNo);

                if (!string.IsNullOrEmpty(shopHoliday))
                {
                    var arr = HttpUtility.UrlDecode(shopHoliday.ToString()).Split(',');

                    if (arr.Count() > 0)
                    {
                        foreach (var item in arr)
                        {
                            beautyShopHoliday.BeautyShopNo = beautyShop.BeautyShopNo;
                            beautyShopHoliday.BeautyShopHolidayCode = int.Parse(item);

                            db.BeautyShopHolidays.Add(beautyShopHoliday);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                #endregion Insert BeautyShopHoliday

                PetterResultType.IsSuccessful = true;
                PetterResultType.JsonDataSet = beautyShop;

                return Ok(PetterResultType);
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }

        }

        // POST: api/Pensions
        [ResponseType(typeof(Pension))]
        public async Task<IHttpActionResult> PostPension(Pension pension)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pensions.Add(pension);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pension.PensionNo }, pension);
        }

        // DELETE: api/Pensions/5
        [ResponseType(typeof(Pension))]
        public async Task<IHttpActionResult> DeletePension(int id)
        {
            Pension pension = await db.Pensions.FindAsync(id);
            if (pension == null)
            {
                return NotFound();
            }

            db.Pensions.Remove(pension);
            await db.SaveChangesAsync();

            return Ok(pension);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PensionExists(int id)
        {
            return db.Pensions.Count(e => e.PensionNo == id) > 0;
        }
    }
}