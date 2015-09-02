﻿using System;
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
                StartPensionHours = p.StartPensionHours,
                EndPensionHours = p.EndPensionHours,
                Introduction = p.Introduction,
                Coordinate = p.Coordinate,
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
        //[ResponseType(typeof(PetterResultType<Pension>))]
        //public async Task<IHttpActionResult> PutPension(int id, Pension pension)
        //{
        //    PetterResultType<Pension> PetterResultType = new PetterResultType<Pension>();
        //    List<PensionService> pensionServices = new List<PensionService>();
        //    List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
        //    string pensionService = string.Empty;
        //    string pensionHoliday = string.Empty;

        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    BeautyShop beautyShop = await db.BeautyShops.FindAsync(id);
        //    if (beautyShop == null)
        //    {
        //        return NotFound();
        //    }

        //    var folder = HostingEnvironment.MapPath(UploadPath.BeautyShopPath);
        //    Utilities.CreateDirectory(folder);

        //    var provider = await Request.Content.ReadAsMultipartAsync();

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // Show all the key-value pairs.
        //        foreach (var key in provider.FormData)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key.ToString()))
        //            {
        //                var item = HttpUtility.UrlDecode(val);
        //                #region case
        //                switch (key.ToString())
        //                {
        //                    case "BeautyShopNo":
        //                        beautyShop.BeautyShopNo = int.Parse(item);
        //                        break;
        //                    case "CompanyNo":
        //                        beautyShop.CompanyNo = int.Parse(item);
        //                        break;
        //                    case "BeautyShopName":
        //                        beautyShop.BeautyShopName = item;
        //                        break;
        //                    case "BeautyShopAddr":
        //                        beautyShop.BeautyShopAddr = item;
        //                        break;
        //                    case "PictureName":
        //                        beautyShop.PictureName = item;
        //                        break;
        //                    case "PicturePath":
        //                        beautyShop.PicturePath = item;
        //                        break;
        //                    case "StartBeautyShop":
        //                        beautyShop.StartBeautyShop = item;
        //                        break;
        //                    case "EndBeautyShop":
        //                        beautyShop.EndBeautyShop = item;
        //                        break;
        //                    case "Introduction":
        //                        beautyShop.Introduction = item;
        //                        break;
        //                    case "Coordinate":
        //                        //beautyShop.Coordinate = item;
        //                        break;
        //                    case "Grade":
        //                        beautyShop.Grade = int.Parse(item);
        //                        break;
        //                    case "ReviewCount":
        //                        beautyShop.ReviewCount = int.Parse(item);
        //                        break;
        //                    case "Bookmark":
        //                        beautyShop.Bookmark = int.Parse(item);
        //                        break;
        //                    case "BeautyShopServices":
        //                        shopService = item;
        //                        break;
        //                    case "BeautyShopHolidays":
        //                        shopHoliday = item;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                #endregion case
        //            }
        //        }

        //        foreach (MultipartFileData file in provider.FileData)
        //        {
        //            Trace.WriteLine("Server file name: " + file.Headers.ContentDisposition.FileName);
        //            Trace.WriteLine("Server file path: " + file.LocalFileName);
        //            beautyShop.PictureName = file.Headers.ContentDisposition.FileName;
        //            beautyShop.PicturePath = file.LocalFileName;
        //            //beautyShop.DateCreated = DateTime.Now;
        //            beautyShop.DateModified = DateTime.Now;

        //            string thumbnamil = Path.GetFileNameWithoutExtension(file.LocalFileName) + "_thumbnail" + Path.GetExtension(file.LocalFileName);

        //            Utilities.ResizeImage(file.LocalFileName, thumbnamil, FileSize.BeautyShopWidth, FileSize.BeautyShopHeight, System.Drawing.Imaging.ImageFormat.Png);
        //        }

        //        db.Entry(beautyShop).State = EntityState.Modified;

        //        try
        //        {
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BeautyShopExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        // BeautyShopServic 
        //        #region Insert BeautyShopServic
        //        var sc = new BeautyShopServicesController();
        //        await sc.DeleteBeautyShopServicByBeautyShopNo(beautyShop.BeautyShopNo);

        //        if (!string.IsNullOrEmpty(shopService))
        //        {
        //            var arr = HttpUtility.UrlDecode(shopService.ToString()).Split(',');

        //            if (arr.Count() > 0)
        //            {
        //                foreach (var item in arr)
        //                {
        //                    beautyShopService.BeautyShopNo = beautyShop.BeautyShopNo;
        //                    beautyShopService.BeautyShopServiceCode = int.Parse(item);

        //                    db.BeautyShopServices.Add(beautyShopService);
        //                    await db.SaveChangesAsync();
        //                }
        //            }
        //        }
        //        #endregion Insert BeautyShopServic

        //        // BeautyShopServic 
        //        #region Insert BeautyShopHoliday
        //        var hc = new BeautyShopHolidaysController();
        //        await hc.DeleteBeautyShopHolidayByBeautyShopNo(beautyShop.BeautyShopNo);

        //        if (!string.IsNullOrEmpty(shopHoliday))
        //        {
        //            var arr = HttpUtility.UrlDecode(shopHoliday.ToString()).Split(',');

        //            if (arr.Count() > 0)
        //            {
        //                foreach (var item in arr)
        //                {
        //                    beautyShopHoliday.BeautyShopNo = beautyShop.BeautyShopNo;
        //                    beautyShopHoliday.BeautyShopHolidayCode = int.Parse(item);

        //                    db.BeautyShopHolidays.Add(beautyShopHoliday);
        //                    await db.SaveChangesAsync();
        //                }
        //            }
        //        }
        //        #endregion Insert BeautyShopHoliday

        //        PetterResultType.IsSuccessful = true;
        //        PetterResultType.JsonDataSet = beautyShop;

        //        return Ok(PetterResultType);
        //    }
        //    catch (System.Exception e)
        //    {
        //        return InternalServerError(e);
        //    }

        //}


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

                        if (!Enumerable.Any<string>((IEnumerable<string>)FileExtension.PensionExtensions, (Func<string, bool>)(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase))))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ErrorMessage.FileTypeError;
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);
                        //Utilities.ResizeImage(fullPath, thumbnamil, 980.0, 360.0, ImageFormat.Png);
                        pension.PictureName = fileName;
                        pension.PicturePath = folder;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);
                        string[] geography;
                        string point = string.Empty;

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
                                pension.StartPensionHours = item;
                                break;
                            case "EndPensionHours":
                                pension.EndPensionHours = item;
                                break;
                            case "Introduction":
                                pension.Introduction = item;
                                break;
                            case "Coordinate":
                                geography = item.Split(',');
                                if (geography.Length != 2)
                                {
                                    return BadRequest();
                                }
                                point = string.Format("POINT({0} {1})", geography[0], geography[1]);
                                pension.Coordinate = DbGeography.FromText(point);
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
                            case "DateCreated":
                                pension.DateCreated = DateTime.Now;
                                break;
                            case "DateModified":
                                pension.DateModified = DateTime.Now;
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

                pension.DateCreated = DateTime.Now;
                pension.DateModified = DateTime.Now;
                db.Pensions.Add(pension);
                int num = await this.db.SaveChangesAsync();

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

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = pension;

                //return PetterResultType;
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

                db.PensionServices.Add(pensionService);
                await db.SaveChangesAsync();

                pensionServices.Add(pensionService);
            }

            return pensionServices;
        }

        //private async Task DeletePensionService(Pension Pension)
        //{
        //    List<PensionService> PensionService = new List<PensionService>();
        //    List<PensionService> list = await (Task<List<PensionService>>)QueryableExtensions.ToListAsync<PensionService>((IQueryable<M0>)Queryable.Where<PensionService>((IQueryable<PensionService>)this.db.PensionServices, (Expression<Func<PensionService, bool>>)(p => p.PensionNo == Pension.PensionNo)));
        //    PensionService = list;
        //    list = (List<PensionService>)null;
        //    foreach (PensionService pensionService in PensionService)
        //    {
        //        PensionService item = pensionService;
        //        this.db.PensionServices.Remove(item);
        //        int num = await this.db.SaveChangesAsync();
        //        item = (PensionService)null;
        //    }
        //    List<PensionService>.Enumerator enumerator = new List<PensionService>.Enumerator();
        //}

        private async Task<List<PensionHoliday>> AddPensionHoliday(Pension pension, string holiday)
        {
            List<PensionHoliday> pensionHolidays = new List<PensionHoliday>();
            var arr = HttpUtility.UrlDecode(holiday.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                PensionHoliday pensionHoliday = new PensionHoliday();
                pensionHoliday.PensionNo = pension.PensionNo;
                pensionHoliday.PensionHolidayCode = int.Parse(arr[i].ToString());

                db.PensionHolidays.Add(pensionHoliday);
                await db.SaveChangesAsync();

                pensionHolidays.Add(pensionHoliday);
            }

            return pensionHolidays;
        }

        //private async Task DeletePensionHoliday(Pension Pension)
        //{
        //    List<PensionHoliday> PensionHolidays = new List<PensionHoliday>();
        //    List<PensionHoliday> list = await (Task<List<PensionHoliday>>)QueryableExtensions.ToListAsync<PensionHoliday>((IQueryable<M0>)Queryable.Where<PensionHoliday>((IQueryable<PensionHoliday>)this.db.PensionHolidays, (Expression<Func<PensionHoliday, bool>>)(p => p.PensionNo == Pension.PensionNo)));
        //    PensionHolidays = list;
        //    list = (List<PensionHoliday>)null;
        //    foreach (PensionHoliday pensionHoliday in PensionHolidays)
        //    {
        //        PensionHoliday item = pensionHoliday;
        //        this.db.PensionHolidays.Remove(item);
        //        int num = await this.db.SaveChangesAsync();
        //        item = (PensionHoliday)null;
        //    }
        //    List<PensionHoliday>.Enumerator enumerator = new List<PensionHoliday>.Enumerator();
        //}

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