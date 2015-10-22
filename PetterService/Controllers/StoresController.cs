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
//using System.ServiceModel.Channels;

namespace PetterService.Controllers
{
    public class StoresController : ApiController
    {
        // 1. 스토어 리스트 (X)
        // 2. 스토어 상세 (O)
        // 3. 스토어 등록 (O)
        // 4. 스토어 수정 (O)
        // 5. 스토어 삭제 (O)
        // 6. 스토어 ID 검색
        // 

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/Stores
        /// 스토어 리스트
        /// </summary>
        /// <param name="petterRequestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Store>))]
        public async Task<IHttpActionResult> GetStore([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<Store> petterResultType = new PetterResultType<Store>();
            List<Store> list = new List<Store>();
            // 서울 (경도: 126.9784, 위도: 37.5667)
            DbGeography currentLocation = DbGeography.FromText(string.Format("POINT({0} {1})", petterRequestType.Longitude, petterRequestType.Latitude));
            int distance = petterRequestType.Distance;

            var store = db.Stores.AsEnumerable();

            // 검색 조건 
            if (!String.IsNullOrEmpty(petterRequestType.Search))
            {
                store = store.Where(p => p.StoreName != null && p.StoreName.Contains(petterRequestType.Search));
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 거리
                case "distance":
                    {
                        list = store
                            .Where(p => petterRequestType.CodeID == petterRequestType.CodeID)
                            //.Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.StoreNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 리뷰수
                case "reviewcount":
                    {
                        list = store
                            .Where(p => petterRequestType.CodeID == petterRequestType.CodeID)
                            //.Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            //.OrderByDescending(p => p.ReviewCount)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 점수
                case "grade":
                    {
                        list = store
                            .Where(p => petterRequestType.CodeID == petterRequestType.CodeID)
                            //.Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            //.OrderByDescending(p => p.Grade)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 즐겨찾기
                case "bookmark":
                    {
                        list = store
                            .Where(p => petterRequestType.CodeID == petterRequestType.CodeID)
                            //.Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            //.OrderByDescending(p => p.Bookmark)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
                // 기본
                default:
                    {
                        list = store
                            .Where(p => petterRequestType.CodeID == petterRequestType.CodeID)
                            //.Where(p => p.Coordinate.Distance(currentLocation) <= distance)
                            .OrderByDescending(p => p.CompanyNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
            }
            #endregion 정렬방식

            petterResultType.IsSuccessful = true;
            petterResultType.AffectedRow = list.Count();
            petterResultType.JsonDataSet = list.ToList();
            return Ok(petterResultType);
        }

        /// <summary>
        /// GET: api/Stores/5
        /// 스토어 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreDTO>))]
        public async Task<IHttpActionResult> GetStore(int id)
        {
            PetterResultType<StoreDTO> petterResultType = new PetterResultType<StoreDTO>();
            List<StoreDTO> stores = new List<StoreDTO>();

            var store = await db.Stores.Where(p => p.StoreNo == id).Select(p => new StoreDTO
            {
                StoreNo = p.StoreNo,
                CompanyNo = p.CompanyNo,
                StoreName = p.StoreName,
                //StoreID = p.StoreID,
                Phone = p.Phone,
                StoreAddress = p.StoreAddress,
                FileName = p.FileName,
                FilePath = p.FilePath,
                StartTime = p.StartTime,
                EndTime = p.EndTime,
                Introduction = p.Introduction,
                Coordinate = p.Coordinate,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                StoreStats = p.StoreStats.ToList(),
                StoreServices = p.StoreServices.ToList(),
                StoreHolidays = p.StoreHolidays.ToList()
            }).SingleOrDefaultAsync();

            if (store == null)
            {
                return NotFound();
            }

            stores.Add(store);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = stores;
            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/Stores/5
        /// 스토어 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Store>))]
        public async Task<IHttpActionResult> PutStore(int id)
        {
            PetterResultType<Store> petterResultType = new PetterResultType<Store>();
            List<Store> stores = new List<Store>();
            List<StoreService> storeServices = new List<StoreService>();
            List<StoreHoliday> storeHolidays = new List<StoreHoliday>();
            string storeService = string.Empty;
            string storeHoliday = string.Empty;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Store store = await db.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StorePath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();
                        string oldFileName = HttpUtility.UrlDecode(content.Headers.ContentDisposition.FileName.Trim('"'));
                        string fileName = Utilities.additionFileName(oldFileName);

                        if (!FileExtension.StoreExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreWidth, FileSize.StoreHeight, ImageFormat.Png);
                        store.FileName = fileName;
                        store.FilePath = UploadPath.StorePath.Replace("~", "");
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "StoreNo":
                            //    store.StoreNo = int.Parse(item);
                            //    break;
                            //case "CompanyNo":
                            //    store.CompanyNo = int.Parse(item);
                            //    break;
                            //case "CodeID":
                            //    store.CodeID = item;
                            //    break;
                            case "StoreName":
                                store.StoreName = item;
                                break;
                            //case "StoreID":
                            //    store.StoreID = item;
                            //    break;
                            case "Phone":
                                store.Phone = item;
                                break;
                            case "StoreAddress":
                                store.StoreAddress = item;
                                break;
                            case "StartTime":
                                store.StartTime = item;
                                break;
                            case "EndTime":
                                store.EndTime = item;
                                break;
                            case "Introduction":
                                store.Introduction = item;
                                break;
                            case "Latitude":
                                store.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                store.Longitude = Convert.ToDouble(item);
                                break;
                            case "StoreServices":
                                storeService = item;
                                break;
                            case "StoreHolidays":
                                storeHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", store.Longitude, store.Latitude);
                store.Coordinate = DbGeography.FromText(point);
                store.DateModified = DateTime.Now;
                db.Entry(store).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   throw;
                }

                await DeleteStoreService(store);
                if (!string.IsNullOrWhiteSpace(storeService))
                {
                    List<StoreService> list = await AddStoreService(store, storeService);
                    store.StoreServices = list;
                }

                await this.DeleteStoreHoliday(store);
                if (!string.IsNullOrWhiteSpace(storeHoliday))
                {
                    List<StoreHoliday> list = await AddstoreHoliday(store, storeHoliday);
                    store.StoreHolidays = list;
                }

                stores.Add(store);
                petterResultType.AffectedRow = stores.Count();
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = stores;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/Stores
        /// 스토어 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Store>))]
        public async Task<IHttpActionResult> PostStore()
        {
            PetterResultType<Store> petterResultType = new PetterResultType<Store>();
            List<Store> stores = new List<Store>();
            List<StoreService> storeServices = new List<StoreService>();
            List<StoreHoliday> storeHolidays = new List<StoreHoliday>();
            Store store = new Store();
            string storeService = string.Empty;
            string storeHoliday = string.Empty;

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StorePath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();
                        string oldFileName = HttpUtility.UrlDecode(content.Headers.ContentDisposition.FileName.Trim('"'));
                        string fileName = Utilities.additionFileName(oldFileName);

                        if (!FileExtension.StoreExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreWidth, FileSize.StoreHeight, ImageFormat.Png);
                        store.FileName = fileName;
                        store.FilePath = UploadPath.StorePath.Replace("~", "");
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "CompanyNo":
                                store.CompanyNo = int.Parse(item);
                                break;
                            case "CodeID":
                                store.CodeID = item;
                                break;
                            case "StoreName":
                                store.StoreName = item;
                                break;
                            //case "StoreID":
                            //    store.StoreID = item;
                            //    break;
                            case "Phone":
                                store.Phone = item;
                                break;
                            case "StoreAddress":
                                store.StoreAddress = item;
                                break;
                            case "FileName":
                                store.FileName = item;
                                break;
                            case "FilePath":
                                store.FilePath = item;
                                break;
                            case "StartTime":
                                store.StartTime = item;
                                break;
                            case "EndTime":
                                store.EndTime = item;
                                break;
                            case "Introduction":
                                store.Introduction = item;
                                break;
                            case "Latitude":
                                store.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                store.Longitude = Convert.ToDouble(item);
                                break;
                            case "StoreServices":
                                storeService = item;
                                break;
                            case "StoreHolidays":
                                storeHoliday = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", store.Longitude, store.Latitude);
                store.Coordinate = DbGeography.FromText(point);
                store.StateFlag = "U";
                //store.WriteIP = "2222.2.22.22";
                store.WriteIP = Request.GetClientIpAddress();
                store.DateCreated = DateTime.Now;
                store.DateModified = DateTime.Now;
                db.Stores.Add(store);
                int num = await this.db.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(storeService))
                {
                    //List<StoreService> list = await AddStoreService(store, storeService);
                    List<StoreService> list = await AddStoreService(store, storeService);
                    store.StoreServices = list;
                }

                if (!string.IsNullOrWhiteSpace(storeHoliday))
                {
                    List<StoreHoliday> list = await AddstoreHoliday(store, storeHoliday);
                    store.StoreHolidays = list;
                }

                stores.Add(store);
                petterResultType.AffectedRow = stores.Count();
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = stores;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        // DELETE: api/Stores/5
        [ResponseType(typeof(Store))]
        public async Task<IHttpActionResult> DeleteStore(int id)
        {
            Store store = await db.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            db.Stores.Remove(store);
            await db.SaveChangesAsync();

            return Ok(store);
        }

        /// <summary>
        /// GET: api/Stores/StoreID/SmartPetter
        /// StoreID 중복 확인
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("StoreNo")]
        [ResponseType(typeof(PetterResultType<StoreDTO>))]
        public async Task<IHttpActionResult> GetStoreByStoreID(int id)
        {
            PetterResultType<StoreDTO> petterResultType = new PetterResultType<StoreDTO>();
            List<StoreDTO> stores = new List<StoreDTO>();

            var idCount = db.Stores.Count(e => e.StoreNo == id);

            //var store = await db.Stores.Where(p => p.StoreID == id).Select(p => new StoreDTO
            //{
            //    StoreNo = p.StoreNo,
            //    CompanyNo = p.CompanyNo,
            //    StoreName = p.StoreName,
            //    StoreID = p.StoreID,
            //    Phone = p.Phone,
            //    StoreAddress = p.StoreAddress,
            //    FileName = p.FileName,
            //    FilePath = p.FilePath,
            //    StartTime = p.StartTime,
            //    EndTime = p.EndTime,
            //    Introduction = p.Introduction,
            //    Coordinate = p.Coordinate,
            //    Latitude = p.Latitude,
            //    Longitude = p.Longitude,
            //    DateCreated = p.DateCreated,
            //    DateModified = p.DateModified,
            //    StoreStats = p.StoreStats.ToList(),
            //    StoreServices = p.StoreServices.ToList(),
            //    StoreHolidays = p.StoreHolidays.ToList()
            //}).SingleOrDefaultAsync();

            if (idCount == 0)
            {
                petterResultType.IsSuccessful = true;
                petterResultType.ScalarValue = idCount;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.ScalarValue = idCount;
            }

            return Ok(petterResultType);
        }

        private async Task<List<StoreService>> AddStoreService(Store store, string service)
        {
            List<StoreService> storeServices = new List<StoreService>();
            var arr = HttpUtility.UrlDecode(service.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                StoreService storeservice = new StoreService();

                storeservice.StoreNo = store.StoreNo;
                storeservice.CodeID = arr[i].ToString().Trim();

                storeServices.Add(storeservice);
            }

            db.StoreServices.AddRange(storeServices);
            await db.SaveChangesAsync();

            return storeServices;
        }

        private async Task DeleteStoreService(Store store)
        {
            var storeServices = await db.StoreServices.Where(p => p.StoreNo == store.StoreNo).ToListAsync();
            db.StoreServices.RemoveRange(storeServices);
        }

        private async Task<List<StoreHoliday>> AddstoreHoliday(Store store, string holiday)
        {
            List<StoreHoliday> storeHolidays = new List<StoreHoliday>();
            var arr = HttpUtility.UrlDecode(holiday.ToString()).Split(',');

            for (int i = 0; i < arr.Length; i++)
            {
                StoreHoliday storeHoliday = new StoreHoliday();

                storeHoliday.StoreNo = store.StoreNo;
                storeHoliday.CodeID = arr[i].ToString().Trim();

                storeHolidays.Add(storeHoliday);
            }

            db.StoreHolidays.AddRange(storeHolidays);
            await db.SaveChangesAsync();

            return storeHolidays;
        }

        private async Task DeleteStoreHoliday(Store store)
        {
            var storeHolidays = await db.StoreHolidays.Where(p => p.StoreNo == store.StoreNo).ToListAsync();
            db.StoreHolidays.RemoveRange(storeHolidays);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreExists(int id)
        {
            return db.Stores.Count(e => e.StoreNo == id) > 0;
        }
    }
}