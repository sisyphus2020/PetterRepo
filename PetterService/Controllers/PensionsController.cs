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
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace PetterService.Controllers
{
    public class PensionsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/Pensions
        public IEnumerable<Pension> GetPensions([FromUri] PetterRequestType petterRequestType)
        {
            List<Pension> list = new List<Pension>();
            DbGeography currentLocation = DbGeography.FromText(string.Format("POINT({0} {1})", petterRequestType.Latitude, petterRequestType.Longitude));
            int distance = petterRequestType.Distance;

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
        [ResponseType(typeof(PetterResultType<PensionDTO>))]
        public async Task<IHttpActionResult> GetPension(int id)
        {
            PetterResultType<PensionDTO> petterResultType = new PetterResultType<PensionDTO>();
          
            var pensionDatail = await db.Pensions.Where(p => p.PensionNo == id).Select(p => new PensionDTO
            {
                PensionNo = p.PensionNo,
                CompanyNo = p.CompanyNo,
                PensionName = p.PensionName,
                PensionAddr = p.PensionAddr,
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
                PensionServices = p.PensionServices.ToList(),
                PensionHolidays = p.PensionHolidays.ToList()
            }).SingleOrDefaultAsync();

            if (pensionDatail == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = pensionDatail;
            return Ok(petterResultType);
        }

        // PUT: api/Pensions/5
        [ResponseType(typeof(PetterResultType<Pension>))]
        public async Task<IHttpActionResult> PutPension(int id)
        {
            PetterResultType<Pension> petterResultType = new PetterResultType<Pension>();
            List<PensionService> pensionServices = new List<PensionService>();
            List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Pension pension = await db.Pensions.FindAsync(id);
            if (pension == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.PensionPath);
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

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.PensionWidth, FileSize.PensionHeight, ImageFormat.Png);
                        pension.PictureName = fileName;
                        pension.PicturePath = UploadPath.PensionPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "PensionNo":
                                pension.PensionNo = int.Parse(item);
                                break;
                            case "CompanyNo":
                                pension.CompanyNo = int.Parse(item);
                                break;
                            case "PensionName":
                                pension.PensionName = item;
                                break;
                            case "PensionAddr":
                                pension.PensionAddr = item;
                                break;
                            case "StartPensionHours":
                                pension.StartHours = item;
                                break;
                            case "EndPensionHours":
                                pension.EndHours = item;
                                break;
                            case "Introduction":
                                pension.Introduction = item;
                                break;
                            case "Latitude":
                                pension.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                pension.Longitude = Convert.ToDouble(item);
                                break;
                            case "Grade":
                                pension.Grade = int.Parse(item);
                                break;
                            case "ReviewCount":
                                pension.ReviewCount = int.Parse(item);
                                break;
                            case "Bookmark":
                                pension.Bookmark = int.Parse(item);
                                break;
                            case "PensionServices":
                                pensionService = item;
                                break;
                            case "PensionHolidays":
                                pensionHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", pension.Latitude, pension.Longitude);
                pension.Coordinate = DbGeography.FromText(point);
                pension.DateModified = DateTime.Now;
                db.Entry(pension).State = EntityState.Modified;
                
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                await DeletePensionService(pension);
                if (!string.IsNullOrWhiteSpace(pensionService))
                {
                    List<PensionService> list = await AddPensionService(pension, pensionService);
                    pension.PensionServices = list;
                }

                await this.DeletePensionHoliday(pension);
                if (!string.IsNullOrWhiteSpace(pensionHoliday))
                {
                    List<PensionHoliday> list = await AddPensionHoliday(pension, pensionHoliday);
                    pension.PensionHolidays = list;
                }

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = pension;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }


        // POST: api/Pensions
        [ResponseType(typeof(PetterResultType<Pension>))]
        public async Task<IHttpActionResult> PostPension()
        {
            PetterResultType<Pension> petterResultType = new PetterResultType<Pension>();
            List<PensionService> pensionServices = new List<PensionService>();
            List<PensionHoliday> pensionHolidays = new List<PensionHoliday>();
            Pension pension = new Pension();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.PensionPath);
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

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.PensionWidth, FileSize.PensionHeight, ImageFormat.Png);
                        pension.PictureName = fileName;
                        pension.PicturePath = UploadPath.PensionPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "PensionNo":
                                pension.PensionNo = int.Parse(item);
                                break;
                            case "CompanyNo":
                                pension.CompanyNo = int.Parse(item);
                                break;
                            case "PensionName":
                                pension.PensionName = item;
                                break;
                            case "PensionAddr":
                                pension.PensionAddr = item;
                                break;
                            case "PictureName":
                                pension.PictureName = item;
                                break;
                            case "PicturePath":
                                pension.PicturePath = item;
                                break;
                            case "StartPensionHours":
                                pension.StartHours = item;
                                break;
                            case "EndPensionHours":
                                pension.EndHours = item;
                                break;
                            case "Introduction":
                                pension.Introduction = item;
                                break;
                            case "Latitude":
                                pension.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                pension.Longitude = Convert.ToDouble(item);
                                break;
                            case "Grade":
                                pension.Grade = int.Parse(item);
                                break;
                            case "ReviewCount":
                                pension.ReviewCount = int.Parse(item);
                                break;
                            case "Bookmark":
                                pension.Bookmark = int.Parse(item);
                                break;
                            case "PensionServices":
                                pensionService = item;
                                break;
                            case "PensionHolidays":
                                pensionHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", pension.Latitude, pension.Longitude);
                pension.Coordinate = DbGeography.FromText(point);
                pension.DateCreated = DateTime.Now;
                pension.DateModified = DateTime.Now;
                db.Pensions.Add(pension);
                int num = await this.db.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(pensionService))
                {
                    List<PensionService> list = await AddPensionService(pension, pensionService);
                    pension.PensionServices = list;
                }

                if (!string.IsNullOrWhiteSpace(pensionHoliday))
                {
                    List<PensionHoliday> list = await AddPensionHoliday(pension, pensionHoliday);
                    pension.PensionHolidays = list;
                }

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = pension;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
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

        private async Task<List<PensionService>> AddPensionService(Pension pension, string service)
        {
            List<PensionService> pensionServices = new List<PensionService>();
            var arr = HttpUtility.UrlDecode(service.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                PensionService pensionService = new PensionService();
                pensionService.PensionNo = pension.PensionNo;
                pensionService.PensionServiceCode = int.Parse(arr[i].ToString());
                pensionServices.Add(pensionService);
            }

            db.PensionServices.AddRange(pensionServices);
            await db.SaveChangesAsync();

            return pensionServices;
        }

        private async Task DeletePensionService(Pension pension)
        {
            var pensionService = await db.PensionServices.Where(p => p.PensionNo == pension.PensionNo).ToListAsync();
            db.PensionServices.RemoveRange(pensionService);
        }

        private async Task<List<PensionHoliday>> AddPensionHoliday(Pension pension, string holiday)
        {
            List<PensionHoliday> pensionHolidays = new List<PensionHoliday>();
            
            var arr = HttpUtility.UrlDecode(holiday.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                PensionHoliday pensionHoliday = new PensionHoliday();
                pensionHoliday.PensionNo = pension.PensionNo;
                pensionHoliday.PensionHolidayCode = int.Parse(arr[i].ToString());
                pensionHolidays.Add(pensionHoliday);
            }

            db.PensionHolidays.AddRange(pensionHolidays);
            await db.SaveChangesAsync();

            return pensionHolidays;
        }

        private async Task DeletePensionHoliday(Pension pension)
        {
            var pensionHolidays = await db.PensionHolidays.Where(p => p.PensionNo == pension.PensionNo).ToListAsync();
            db.PensionHolidays.RemoveRange(pensionHolidays);
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