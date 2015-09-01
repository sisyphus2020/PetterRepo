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
        [ResponseType(typeof(PetterResultType<Pension>))]
        public async Task<IHttpActionResult> PutPension(int id, Pension pension)
        {
            PetterResultType<Pension> PetterResultType = new PetterResultType<Pension>();
            List<PensionService> pensionServices = new List<PensionService>();
            List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            BeautyShop beautyShop = await db.BeautyShops.FindAsync(id);
            if (beautyShop == null)
            {
                return NotFound();
            }

            var folder = HostingEnvironment.MapPath(UploadPath.BeautyShopPath);
            Utilities.CreateDirectory(folder);

            //var provider = new CustomMultipartFormDataStreamProvider(folder);

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
        [ResponseType(typeof(PetterResultType<Pension>))]
        public async Task<IHttpActionResult> PostPension()
        {
            PetterResultType<Pension> PetterResultType = new PetterResultType<Pension>();
            List<PensionService> pensionServices = new List<PensionService>();
            List<PensionHoliday> pensionHolidays = new List<PensionHoliday>();
            Pension pension = new Pension();
            string pensionService = string.Empty;
            string pensionHoliday = string.Empty;

            IHttpActionResult ihttpActionResult;

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.PensionPath);
                Utilities.CreateDirectory(folder);
                var  provider = new MultipartMemoryStreamProvider();
                MultipartMemoryStreamProvider memoryStreamProvider = await (Task<MultipartMemoryStreamProvider>)HttpContentMultipartExtensions.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(this.get_Request().Content, (M0)provider);
                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        byte[] numArray = await content.ReadAsByteArrayAsync();
                        byte[] file = numArray;
                        numArray = (byte[])null;
                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));
                        if (!Enumerable.Any<string>((IEnumerable<string>)FileExtension.PensionExtensions, (Func<string, bool>)(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase))))
                        {
                            PetterResultType.IsSuccessful = false;
                            PetterResultType.JsonDataSet = null;
                            PetterResultType.ErrorMessage = ErrorMessage.FileTypeError;
                            
                        }
                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);
                        //Utilities.ResizeImage(fullPath, thumbnamil, 980.0, 360.0, ImageFormat.Png);
                        pension.PictureName = fileName;
                        pension.PicturePath = folder;
                        file = (byte[])null;
                        fullPath = (string)null;
                        thumbnamil = (string)null;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);
                        str = (string)null;
                        string s = fieldName.ToString();
                        // ISSUE: reference to a compiler-generated method
                        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
                        string[] geography;
                        string point;
                        if (stringHash <= 2709835700U)
                        {
                            if (stringHash <= 1485049058U)
                            {
                                if (stringHash <= 663770264U)
                                {
                                    if ((int)stringHash != 499144555)
                                    {
                                        if ((int)stringHash == 663770264 && s == "ReviewCount")
                                            pension.ReviewCount = int.Parse(item);
                                    }
                                    else if (s == "Coordinate")
                                    {
                                        geography = item.Split(',');
                                        if (geography.Length != 2)
                                        {
                                            ihttpActionResult = (IHttpActionResult)this.BadRequest();
                                            goto label_74;
                                        }
                                        else
                                        {
                                            point = string.Format("POINT({0} {1})", (object)geography[0], (object)geography[1]);
                                            pension.Coordinate = DbGeography.FromText(point);
                                        }
                                    }
                                }
                                else if ((int)stringHash != 833590459)
                                {
                                    if ((int)stringHash == 1485049058 && s == "PictureName")
                                        pension.PictureName = item;
                                }
                                else if (s == "CompanyNo")
                                    pension.CompanyNo = int.Parse(item);
                            }
                            else if (stringHash <= 1702902297U)
                            {
                                if ((int)stringHash != 1644290004)
                                {
                                    if ((int)stringHash == 1702902297 && s == "Introduction")
                                        pension.Introduction = item;
                                }
                                else if (s == "Grade")
                                    pension.Grade = (Decimal)int.Parse(item);
                            }
                            else if ((int)stringHash != 1985364884)
                            {
                                if ((int)stringHash == -1585131596 && s == "PensionHolidays")
                                    pensionHoliday = item;
                            }
                            else if (s == "EndPension")
                                pension.EndPension = item;
                        }
                        else if (stringHash <= 3247220935U)
                        {
                            if (stringHash <= 2782850510U)
                            {
                                if ((int)stringHash != -1549514183)
                                {
                                    if ((int)stringHash == -1512116786 && s == "PensionNo")
                                        pension.PensionNo = int.Parse(item);
                                }
                                else if (s == "Bookmark")
                                    pension.Bookmark = int.Parse(item);
                            }
                            else if ((int)stringHash != -1509863178)
                            {
                                if ((int)stringHash == -1047746361 && s == "StartPension")
                                    pension.StartPension = item;
                            }
                            else if (s == "PensionAddr")
                                pension.PensionAddr = item;
                        }
                        else if (stringHash <= 3852676210U)
                        {
                            if ((int)stringHash != -929768346)
                            {
                                if ((int)stringHash == -442291086 && s == "PicturePath")
                                    pension.PicturePath = item;
                            }
                            else if (s == "DateModified")
                                pension.DateModified = DateTime.Now;
                        }
                        else if ((int)stringHash != -378585865)
                        {
                            if ((int)stringHash != -224587967)
                            {
                                if ((int)stringHash == -57857742 && s == "PensionName")
                                    pension.PensionName = item;
                            }
                            else if (s == "PensionServices")
                                pensionService = item;
                        }
                        else if (s == "DateCreated")
                            pension.DateCreated = DateTime.Now;
                        geography = (string[])null;
                        point = (string)null;
                        item = (string)null;
                    }
                    fieldName = (string)null;
                    content = (HttpContent)null;
                }
                pension.DateCreated = DateTime.Now;
                pension.DateModified = DateTime.Now;
                this.db.Pensions.Add(pension);
                int num = await this.db.SaveChangesAsync();
                folder = (string)null;
                provider = (MultipartMemoryStreamProvider)null;
                if (!string.IsNullOrWhiteSpace(pensionService))
                {
                    List<PensionService> list = await this.AddPensionService(pension, pensionService);
                    pensionServices = list;
                    list = (List<PensionService>)null;
                    pension.PensionServices = (ICollection<PensionService>)Enumerable.ToList<PensionService>((IEnumerable<PensionService>)pensionServices);
                }
                if (!string.IsNullOrWhiteSpace(pensionHoliday))
                {
                    List<PensionHoliday> list = await this.AddPensionHoliday(pension, pensionHoliday);
                    pensionHolidays = list;
                    list = (List<PensionHoliday>)null;
                    pension.PensionHolidays = (ICollection<PensionHoliday>)Enumerable.ToList<PensionHoliday>((IEnumerable<PensionHoliday>)pensionHolidays);
                }
                PetterResultType.IsSuccessful = true;
                PetterResultType.JsonDataSet = pension;

                //return PetterResultType;
            }
            else
            {
                PetterResultType.IsSuccessful = false;
                PetterResultType.JsonDataSet = (Pension)null;
               
            }

            //return PetterResultType;
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

        private async Task<List<PensionService>> AddPensionService(Pension Pension, string pensionService)
        {
            List<PensionService> PensionServices = new List<PensionService>();
            string[] arr = HttpUtility.UrlDecode(pensionService.ToString()).Split(',');
            string[] strArray = arr;
            for (int index = 0; index < strArray.Length; ++index)
            {
                string item = strArray[index];
                PensionService PensionService = new PensionService();
                PensionService.PensionNo = Pension.PensionNo;
                PensionService.PensionServiceCode = int.Parse(item);
                this.db.PensionServices.Add(PensionService);
                int num = await this.db.SaveChangesAsync();
                PensionServices.Add(PensionService);
                PensionService = (PensionService)null;
                item = (string)null;
            }
            strArray = (string[])null;
            return PensionServices;
        }

        private async Task DeletePensionService(Pension Pension)
        {
            List<PensionService> PensionService = new List<PensionService>();
            List<PensionService> list = await (Task<List<PensionService>>)QueryableExtensions.ToListAsync<PensionService>((IQueryable<M0>)Queryable.Where<PensionService>((IQueryable<PensionService>)this.db.PensionServices, (Expression<Func<PensionService, bool>>)(p => p.PensionNo == Pension.PensionNo)));
            PensionService = list;
            list = (List<PensionService>)null;
            foreach (PensionService pensionService in PensionService)
            {
                PensionService item = pensionService;
                this.db.PensionServices.Remove(item);
                int num = await this.db.SaveChangesAsync();
                item = (PensionService)null;
            }
            List<PensionService>.Enumerator enumerator = new List<PensionService>.Enumerator();
        }

        private async Task<List<PensionHoliday>> AddPensionHoliday(Pension Pension, string pensionHoliday)
        {
            List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
            string[] arr = HttpUtility.UrlDecode(pensionHoliday.ToString()).Split(',');
            string[] strArray = arr;
            for (int index = 0; index < strArray.Length; ++index)
            {
                string item = strArray[index];
                PensionHoliday PensionHoliday = new PensionHoliday();
                PensionHoliday.PensionNo = Pension.PensionNo;
                PensionHoliday.PensionHolidayCode = int.Parse(item);
                this.db.PensionHolidays.Add(PensionHoliday);
                int num = await this.db.SaveChangesAsync();
                PensionHolidays.Add(PensionHoliday);
                PensionHoliday = (PensionHoliday)null;
                item = (string)null;
            }
            strArray = (string[])null;
            return PensionHolidays;
        }

        private async Task DeletePensionHoliday(Pension Pension)
        {
            List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
            List<PensionHoliday> list = await (Task<List<PensionHoliday>>)QueryableExtensions.ToListAsync<PensionHoliday>((IQueryable<M0>)Queryable.Where<PensionHoliday>((IQueryable<PensionHoliday>)this.db.PensionHolidays, (Expression<Func<PensionHoliday, bool>>)(p => p.PensionNo == Pension.PensionNo)));
            PensionHolidays = list;
            list = (List<PensionHoliday>)null;
            foreach (PensionHoliday pensionHoliday in PensionHolidays)
            {
                PensionHoliday item = pensionHoliday;
                this.db.PensionHolidays.Remove(item);
                int num = await this.db.SaveChangesAsync();
                item = (PensionHoliday)null;
            }
            List<PensionHoliday>.Enumerator enumerator = new List<PensionHoliday>.Enumerator();
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