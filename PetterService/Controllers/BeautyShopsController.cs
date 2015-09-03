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
using PetterService.Common;
using System.Data.Entity.Spatial;
using System.Web.Hosting;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace PetterService.Controllers
{
    public class BeautyShopsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShops
        public IEnumerable<BeautyShop> GetBeautyShop([FromUri] PetterRequestType petterRequestType)
        {
            List<BeautyShop> list = new List<BeautyShop>();
            DbGeography currentLocation = DbGeography.FromText(string.Format("POINT({0} {1})", petterRequestType.Latitude, petterRequestType.Longitude));
            int distance = petterRequestType.Distance;

            var beautyShop = db.BeautyShops.AsEnumerable();

            // 검색 조건 
            if (!String.IsNullOrEmpty(petterRequestType.Search))
            {
                beautyShop = beautyShop.Where(p => p.BeautyShopName != null && p.BeautyShopName.Contains(petterRequestType.Search));
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 거리
                case "distance":
                    {
                        list = beautyShop
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.BeautyShopNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 리뷰수
                case "reviewcount":
                    {
                        list = beautyShop
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.ReviewCount)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 점수
                case "grade":
                    {
                        list = beautyShop
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.Grade)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 즐겨찾기
                case "bookmark":
                    {
                        list = beautyShop
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.Bookmark)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 기본
                default:
                    {
                        list = beautyShop
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

        // GET: api/BeautyShops/5
        [ResponseType(typeof(PetterResultType<BeautyShopDTO>))]
        public async Task<IHttpActionResult> GetBeautyShop(int id)
        {
            PetterResultType<BeautyShopDTO> petterResultType = new PetterResultType<BeautyShopDTO>();

            var beautyShopDatail = await db.BeautyShops.Where(p => p.BeautyShopNo == id).Select(p => new BeautyShopDTO
            {
                BeautyShopNo = p.BeautyShopNo,
                CompanyNo = p.CompanyNo,
                BeautyShopName = p.BeautyShopName,
                BeautyShopAddr = p.BeautyShopAddr,
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
                BeautyShopServices = p.BeautyShopServices.ToList(),
                BeautyShopHolidays = p.BeautyShopHolidays.ToList()
            }).SingleOrDefaultAsync();

            if (beautyShopDatail == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = beautyShopDatail;
            return Ok(petterResultType);
        }

        // PUT: api/BeautyShops/5
        [ResponseType(typeof(PetterResultType<BeautyShop>))]
        public async Task<IHttpActionResult> PutBeautyShop(int id)
        {
            PetterResultType<BeautyShop> petterResultType = new PetterResultType<BeautyShop>();
            List<BeautyShopService> beautyShopServices = new List<BeautyShopService>();
            List<BeautyShopHoliday> beautyShopHolidays = new List<BeautyShopHoliday>();
            string beautyShopService = string.Empty;
            string beautyShopHoliday = string.Empty;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            BeautyShop beautyShop = await db.BeautyShops.FindAsync(id);
            if (beautyShop == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.BeautyShopPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.BeautyShopExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BeautyShopWidth, FileSize.BeautyShopHeight, ImageFormat.Png);
                        beautyShop.PictureName = fileName;
                        beautyShop.PicturePath = UploadPath.BeautyShopPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
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
                            case "StartPensionHours":
                                beautyShop.StartHours = item;
                                break;
                            case "EndPensionHours":
                                beautyShop.EndHours = item;
                                break;
                            case "Introduction":
                                beautyShop.Introduction = item;
                                break;
                            case "Latitude":
                                beautyShop.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                beautyShop.Longitude = Convert.ToDouble(item);
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
                                beautyShopService = item;
                                break;
                            case "BeautyShopHolidays":
                                beautyShopHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", beautyShop.Latitude, beautyShop.Longitude);
                beautyShop.Coordinate = DbGeography.FromText(point);
                beautyShop.DateModified = DateTime.Now;
                db.Entry(beautyShop).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   throw;
                }

                await DeleteBeautyShopService(beautyShop);
                if (!string.IsNullOrWhiteSpace(beautyShopService))
                {
                    List<BeautyShopService> list = await AddBeautyShopService(beautyShop, beautyShopService);
                    beautyShop.BeautyShopServices = list;
                }

                await this.DeleteBeautyShopHoliday(beautyShop);
                if (!string.IsNullOrWhiteSpace(beautyShopHoliday))
                {
                    List<BeautyShopHoliday> list = await AddBeautyShopHoliday(beautyShop, beautyShopHoliday);
                    beautyShop.BeautyShopHolidays = list;
                }

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = beautyShop;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        // POST: api/BeautyShops
        [ResponseType(typeof(PetterResultType<BeautyShop>))]
        public async Task<IHttpActionResult> PostBeautyShop()
        {
            PetterResultType<BeautyShop> petterResultType = new PetterResultType<BeautyShop>();
            List<BeautyShopService> beautyShopServices = new List<BeautyShopService>();
            List<BeautyShopHoliday> beautyShopHolidays = new List<BeautyShopHoliday>();
            BeautyShop beautyShop = new BeautyShop();
            string beautyShopService = string.Empty;
            string beautyShopHoliday = string.Empty;

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.BeautyShopPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.PensionExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BeautyShopWidth, FileSize.BeautyShopHeight, ImageFormat.Png);
                        beautyShop.PictureName = fileName;
                        beautyShop.PicturePath = UploadPath.BeautyShopPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "PensionNo":
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
                            case "StartPensionHours":
                                beautyShop.StartHours = item;
                                break;
                            case "EndPensionHours":
                                beautyShop.EndHours = item;
                                break;
                            case "Introduction":
                                beautyShop.Introduction = item;
                                break;
                            case "Latitude":
                                beautyShop.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                beautyShop.Longitude = Convert.ToDouble(item);
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
                                beautyShopService = item;
                                break;
                            case "BeautyShopHolidays":
                                beautyShopHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", beautyShop.Latitude, beautyShop.Longitude);
                beautyShop.Coordinate = DbGeography.FromText(point);
                beautyShop.DateCreated = DateTime.Now;
                beautyShop.DateModified = DateTime.Now;
                db.BeautyShops.Add(beautyShop);
                int num = await this.db.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(beautyShopService))
                {
                    List<BeautyShopService> list = await AddBeautyShopService(beautyShop, beautyShopService);
                    beautyShop.BeautyShopServices = list;
                }

                if (!string.IsNullOrWhiteSpace(beautyShopHoliday))
                {
                    List<BeautyShopHoliday> list = await AddBeautyShopHoliday(beautyShop, beautyShopHoliday);
                    beautyShop.BeautyShopHolidays = list;
                }

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = beautyShop;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
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

        private async Task<List<BeautyShopService>> AddBeautyShopService(BeautyShop beautyShop, string service)
        {
            List<BeautyShopService> beautyShopServices = new List<BeautyShopService>();
            var arr = HttpUtility.UrlDecode(service.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                BeautyShopService beautyShopService = new BeautyShopService();
                beautyShopService.BeautyShopNo = beautyShop.BeautyShopNo;
                beautyShopService.BeautyShopServiceCode = int.Parse(arr[i].ToString());
                beautyShopServices.Add(beautyShopService);
            }

            db.BeautyShopServices.AddRange(beautyShopServices);
            await db.SaveChangesAsync();

            return beautyShopServices;
        }

        private async Task DeleteBeautyShopService(BeautyShop beautyShop)
        {
            var beuatyShopService = await db.BeautyShopServices.Where(p => p.BeautyShopNo == beautyShop.BeautyShopNo).ToListAsync();
            db.BeautyShopServices.RemoveRange(beuatyShopService);
        }

        private async Task<List<BeautyShopHoliday>> AddBeautyShopHoliday(BeautyShop beautyShop, string holiday)
        {
            List<BeautyShopHoliday> beautyShopHolidays = new List<BeautyShopHoliday>();
            var arr = HttpUtility.UrlDecode(holiday.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                BeautyShopHoliday beautyShopHoliday = new BeautyShopHoliday();
                beautyShopHoliday.BeautyShopNo = beautyShop.BeautyShopNo;
                beautyShopHoliday.BeautyShopHolidayCode = int.Parse(arr[i].ToString());
                beautyShopHolidays.Add(beautyShopHoliday);
            }

            db.BeautyShopHolidays.AddRange(beautyShopHolidays);
            await db.SaveChangesAsync();

            return beautyShopHolidays;
        }

        private async Task DeleteBeautyShopHoliday(BeautyShop beautyShop)
        {
            var beautyShpHolidays = await db.BeautyShopHolidays.Where(p => p.BeautyShopNo == beautyShop.BeautyShopNo).ToListAsync();
            db.BeautyShopHolidays.RemoveRange(beautyShpHolidays);
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