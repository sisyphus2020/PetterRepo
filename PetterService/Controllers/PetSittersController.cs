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
using System.IO;
using System.Web;
using System.Drawing.Imaging;

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
                            //.OrderByDescending(p => p.ReviewCount)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 점수
                case "grade":
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            //.OrderByDescending(p => p.Grade)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 즐겨찾기
                case "bookmark":
                    {
                        list = PetSitter
                            .Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            //.OrderByDescending(p => p.Bookmark)
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
            List<PetSitterDTO> petSitters = new List<PetSitterDTO>();

            var petSitter = await db.PetSitters.Where(p => p.PetSitterNo == id).Select(p => new PetSitterDTO
            {
                PetSitterNo = p.PetSitterNo,
                CompanyNo = p.CompanyNo,
                PetSitterName = p.PetSitterName,
                PetSitterAddr = p.PetSitterAddr,
                FileName = p.FileName,
                FilePath = p.FilePath,
                StartTime = p.StartTime,
                EndTime = p.EndTime,
                Introduction = p.Introduction,
                Coordinate = p.Coordinate,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                //Grade = p.Grade,
                //ReviewCount = p.ReviewCount,
                //Bookmark = p.Bookmark,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                PetSitterServices = p.PetSitterServices.ToList(),
                PetSitterHolidays = p.PetSitterHolidays.ToList()
            }).SingleOrDefaultAsync();

            if (petSitter == null)
            {
                return NotFound();
            }

            petSitters.Add(petSitter);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = petSitters;
            return Ok(petterResultType);
        }

        // PUT: api/PetSitters/5
        [ResponseType(typeof(PetterResultType<PetSitterDTO>))]
        public async Task<IHttpActionResult> PutPetSitter(int id)
        {
            PetterResultType<PetSitter> petterResultType = new PetterResultType<PetSitter>();
            List<PetSitter> petSitters = new List<PetSitter>();
            List<PetSitterService> petSitterServices = new List<PetSitterService>();
            List<PetSitterHoliday> petSitterHolidays = new List<PetSitterHoliday>();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            PetSitter petSitter = await db.PetSitters.FindAsync(id);
            if (petSitter == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.PetSitterPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.PetSitterExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.PetSitterWidth, FileSize.PetSitterHeight, ImageFormat.Png);
                        petSitter.FileName = fileName;
                        petSitter.FilePath = UploadPath.PetSitterPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "PetSitterNo":
                                petSitter.PetSitterNo = int.Parse(item);
                                break;
                            case "CompanyNo":
                                petSitter.CompanyNo = int.Parse(item);
                                break;
                            case "PetSitterName":
                                petSitter.PetSitterName = item;
                                break;
                            case "PetSitterAddr":
                                petSitter.PetSitterAddr = item;
                                break;
                            case "FileName":
                                petSitter.FileName = item;
                                break;
                            case "FilePath":
                                petSitter.FilePath = item;
                                break;
                            case "StartTime":
                                petSitter.StartTime = item;
                                break;
                            case "EndTime":
                                petSitter.EndTime = item;
                                break;
                            case "Introduction":
                                petSitter.Introduction = item;
                                break;
                            case "Latitude":
                                petSitter.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                petSitter.Longitude = Convert.ToDouble(item);
                                break;
                            //case "Grade":
                            //    petSitter.Grade = Convert.ToDouble(item);
                            //    break;
                            //case "ReviewCount":
                            //    petSitter.ReviewCount = int.Parse(item);
                            //    break;
                            //case "Bookmark":
                            //    petSitter.Bookmark = int.Parse(item);
                            //    break;
                            case "PetSitterServices":
                                pensionService = item;
                                break;
                            case "PetSitterHolidays":
                                pensionHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", petSitter.Latitude, petSitter.Longitude);
                petSitter.Coordinate = DbGeography.FromText(point);
                petSitter.DateModified = DateTime.Now;
                db.Entry(petSitter).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                await DeletePetSitterService(petSitter);
                if (!string.IsNullOrWhiteSpace(pensionService))
                {
                    List<PetSitterService> list = await AddPetSitterService(petSitter, pensionService);
                    petSitter.PetSitterServices = list;
                }

                await this.DeletePetSitterHoliday(petSitter);
                if (!string.IsNullOrWhiteSpace(pensionHoliday))
                {
                    List<PetSitterHoliday> list = await AddPetSitterHoliday(petSitter, pensionHoliday);
                    petSitter.PetSitterHolidays = list;
                }

                petSitters.Add(petSitter);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = petSitters;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        // POST: api/PetSitters
        [ResponseType(typeof(PetterResultType<PetSitter>))]
        public async Task<IHttpActionResult> PostPetSitter()
        {
            PetterResultType<PetSitter> petterResultType = new PetterResultType<PetSitter>();
            List<PetSitter> petSitters = new List<PetSitter>();
            List<PetSitterService> petSitterServices = new List<PetSitterService>();
            List<PetSitterHoliday> petSitterHolidays = new List<PetSitterHoliday>();
            PetSitter petSitter = new PetSitter();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.PetSitterPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.PetSitterExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.PensionWidth, FileSize.PensionHeight, ImageFormat.Png);
                        petSitter.FileName = fileName;
                        petSitter.FilePath = UploadPath.PetSitterPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "PetSitterNo":
                                petSitter.PetSitterNo = int.Parse(item);
                                break;
                            case "CompanyNo":
                                petSitter.CompanyNo = int.Parse(item);
                                break;
                            case "PetSitterName":
                                petSitter.PetSitterName = item;
                                break;
                            case "PetSitterAddr":
                                petSitter.PetSitterAddr = item;
                                break;
                            case "FileName":
                                petSitter.FileName = item;
                                break;
                            case "FilePath":
                                petSitter.FilePath = item;
                                break;
                            case "StartTime":
                                petSitter.StartTime = item;
                                break;
                            case "EndTime":
                                petSitter.EndTime = item;
                                break;
                            case "Introduction":
                                petSitter.Introduction = item;
                                break;
                            case "Latitude":
                                petSitter.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                petSitter.Longitude = Convert.ToDouble(item);
                                break;
                            //case "Grade":
                            //    petSitter.Grade = Convert.ToDouble(item);
                            //    break;
                            //case "ReviewCount":
                            //    petSitter.ReviewCount = int.Parse(item);
                            //    break;
                            //case "Bookmark":
                            //    petSitter.Bookmark = int.Parse(item);
                            //    break;
                            case "PetSitterServices":
                                pensionService = item;
                                break;
                            case "PetSitterHolidays":
                                pensionHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", petSitter.Latitude, petSitter.Longitude);
                petSitter.Coordinate = DbGeography.FromText(point);
                petSitter.DateCreated = DateTime.Now;
                petSitter.DateModified = DateTime.Now;
                db.PetSitters.Add(petSitter);
                int num = await this.db.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(pensionService))
                {
                    List<PetSitterService> list = await AddPetSitterService(petSitter, pensionService);
                    petSitter.PetSitterServices = list;
                }

                if (!string.IsNullOrWhiteSpace(pensionHoliday))
                {
                    List<PetSitterHoliday> list = await AddPetSitterHoliday(petSitter, pensionHoliday);
                    petSitter.PetSitterHolidays = list;
                }

                petSitters.Add(petSitter);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = petSitters;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
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

        private async Task<List<PetSitterService>> AddPetSitterService(PetSitter petSitter, string service)
        {
            List<PetSitterService> petSitterServices = new List<PetSitterService>();
            var arr = HttpUtility.UrlDecode(service.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                PetSitterService petSitterService = new PetSitterService();
                petSitterService.PetSitterNo = petSitter.PetSitterNo;
                petSitterService.PetSitterServiceCode = int.Parse(arr[i].ToString());
                petSitterServices.Add(petSitterService);
            }

            db.PetSitterServices.AddRange(petSitterServices);
            await db.SaveChangesAsync();

            return petSitterServices;
        }

        private async Task DeletePetSitterService(PetSitter petSitter)
        {
            var petSitterServices = await db.PetSitterServices.Where(p => p.PetSitterNo == petSitter.PetSitterNo).ToListAsync();
            db.PetSitterServices.RemoveRange(petSitterServices);
        }

        private async Task<List<PetSitterHoliday>> AddPetSitterHoliday(PetSitter petSitter, string holiday)
        {
            List<PetSitterHoliday> petSitterHolidays = new List<PetSitterHoliday>();

            var arr = HttpUtility.UrlDecode(holiday.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                PetSitterHoliday petSitterHoliday = new PetSitterHoliday();
                petSitterHoliday.PetSitterNo = petSitter.PetSitterNo;
                petSitterHoliday.PetSitterHolidayCode = int.Parse(arr[i].ToString());
                petSitterHolidays.Add(petSitterHoliday);
            }

            db.PetSitterHolidays.AddRange(petSitterHolidays);
            await db.SaveChangesAsync();

            return petSitterHolidays;
        }

        private async Task DeletePetSitterHoliday(PetSitter petSitter)
        {
            var petSitterHolidays = await db.PetSitterHolidays.Where(p => p.PetSitterNo == petSitter.PetSitterNo).ToListAsync();
            db.PetSitterHolidays.RemoveRange(petSitterHolidays);
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