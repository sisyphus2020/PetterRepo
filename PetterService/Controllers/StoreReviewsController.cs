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
using System.Web.Hosting;
using System.IO;
using System.Web;
using System.Drawing.Imaging;

namespace PetterService.Controllers
{
    public class StoreReviewsController : ApiController
    {
        // 1. 스토어 리뷰 리스트 (O)
        // 2. 스토어 리뷰 상세 (O)
        // 3. 스토어 리뷰 등록 (O)
        // 4. 스토어 리뷰 수정 (O)
        // 5. 스토어 리뷰 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/StoreReviews 
        /// 스토어 리뷰 리스트
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReview>))]
        public async Task<IHttpActionResult> GetStoreReviews([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<StoreReview> petterResultType = new PetterResultType<StoreReview>();
            List<StoreReview> list = new List<StoreReview>();
            bool isSearch = false;

            // 검색 조건 
            if (!String.IsNullOrWhiteSpace(petterRequestType.Search))
            {
                isSearch = true;
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 평점
                case "grade":
                    {
                        list = await db.StoreReviews
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            //.Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            //.OrderByDescending(p => p.grade)
                            .OrderByDescending(p => p.StoreReviewNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToListAsync();
                        break;
                    }
                // 기본
                default:
                    {
                        list = await db.StoreReviews
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            //.Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            .OrderByDescending(p => p.StoreReviewNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToListAsync();
                        break;
                    }
            }
            #endregion 정렬방식

            if (list == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = list.ToList();

            return Ok(petterResultType);
        }

        /// <summary>
        /// GET: api/StoreReviews/5
        /// 스토어 리뷰 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReviewDTO>))]
        public async Task<IHttpActionResult> GetStoreReview(int id)
        {
            //StoreReview storeReview = await db.StoreReviews.FindAsync(id);
            PetterResultType<StoreReviewDTO> petterResultType = new PetterResultType<StoreReviewDTO>();
            List<StoreReviewDTO> storeReviews = new List<StoreReviewDTO>();

            var storeReview = await db.StoreReviews.Where(p => p.StoreReviewNo == id).Select(p => new StoreReviewDTO
            {
                StoreReviewNo = p.StoreReviewNo,
                StoreNo = p.StoreNo,
                MemberNo = p.MemberNo,
                Grade = p.Grade,
                Content = p.Content,
                StateFlag = p.StateFlag,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                DateDeleted = p.DateDeleted,
                //FileName = p.FileName,
                //FilePath = p.FilePath,
                StoreReviewStats = p.StoreReviewStats.ToList(),
                StoreReviewFiles = p.StoreReviewFiles.ToList(),
                StoreReviewLikes = p.StoreReviewLikes.ToList()
            }).SingleOrDefaultAsync();


            if (storeReview == null)
            {
                return NotFound();
            }

            storeReviews.Add(storeReview);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeReviews;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/StoreReviews/5
        /// 스토어 리뷰 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReview>))]
        public async Task<IHttpActionResult> PutstoreReview(int id)
        {
            PetterResultType<StoreReview> petterResultType = new PetterResultType<StoreReview>();
            List<StoreReview> storeReviews = new List<StoreReview>();
            List<StoreReviewFile> storeReviewFiles = new List<StoreReviewFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            StoreReview storeReview = await db.StoreReviews.FindAsync(id);
            if (storeReview == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StoreReviewPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        StoreReviewFile storeReviewFile = new StoreReviewFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.StoreReviewExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreReviewWidth, FileSize.StoreReviewHeight, ImageFormat.Png);
                        storeReviewFile.StoreReviewNo = storeReview.StoreReviewNo;
                        storeReviewFile.FileName = fileName;
                        storeReviewFile.FilePath = UploadPath.StoreReviewPath;
                        storeReviewFile.DateModified = DateTime.Now;
                        storeReviewFile.StateFlag = StateFlags.Use;

                        storeReviewFiles.Add(storeReviewFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "StoreNo":
                            //    storeReview.StoreNo = int.Parse(item);
                            //    break;
                            //case "MemberNo":
                            //    storeReview.MemberNo = int.Parse(item);
                            //    break;
                            case "Content":
                                storeReview.Content = item;
                                break;
                            case "Grade":
                                storeReview.Grade = Convert.ToDouble(item);
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                storeReview.StateFlag = StateFlags.Use;
                storeReview.DateModified = DateTime.Now;

                // 스토어 리뷰 수정
                db.Entry(storeReview).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 스토어 리뷰 파일 등록
                db.StoreReviewFiles.AddRange(storeReviewFiles);
                int num1 = await this.db.SaveChangesAsync();

                storeReviews.Add(storeReview);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = storeReviews;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/StoreReviews
        /// 스토어 리뷰 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReview>))]
        public async Task<IHttpActionResult> PostStoreReview()
        {
            PetterResultType<StoreReview> petterResultType = new PetterResultType<StoreReview>();
            List<StoreReview> storeReviews = new List<StoreReview>();
            List<StoreReviewFile> storeReviewFiles = new List<StoreReviewFile>();

            StoreReview storeReview = new StoreReview();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StoreReviewPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        StoreReviewFile storeReviewFile = new StoreReviewFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.StoreReviewExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreReviewWidth, FileSize.StoreReviewHeight, ImageFormat.Png);
                        storeReviewFile.FileName = fileName;
                        storeReviewFile.FilePath = UploadPath.StoreReviewPath;
                        storeReviewFile.DateCreated = DateTime.Now;
                        storeReviewFile.DateModified = DateTime.Now;
                        storeReviewFile.StateFlag = StateFlags.Use;

                        storeReviewFiles.Add(storeReviewFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "StoreNo":
                                storeReview.StoreNo = int.Parse(item);
                                break;
                            case "MemberNo":
                                storeReview.MemberNo = int.Parse(item);
                                break;
                            case "Content":
                                storeReview.Content = item;
                                break;
                            case "Grade":
                                storeReview.Grade = Convert.ToDouble(item);
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                storeReview.StateFlag = StateFlags.Use;
                storeReview.DateCreated = DateTime.Now;
                storeReview.DateModified = DateTime.Now;

                // 스토어 리뷰 등록
                db.StoreReviews.Add(storeReview);
                int num = await this.db.SaveChangesAsync();

                // 스토어 리뷰 파일 등록
                foreach (var item in storeReviewFiles)
                {
                    item.StoreReviewNo = storeReview.StoreReviewNo;
                }

                db.StoreReviewFiles.AddRange(storeReviewFiles);
                int num1 = await this.db.SaveChangesAsync();

                storeReviews.Add(storeReview);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = storeReviews;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/StoreReviews/5
        /// 스토어 리뷰 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreReview>))]
        public async Task<IHttpActionResult> DeletestoreReview(int id)
        {
            //인증 필요

            PetterResultType<StoreReview> petterResultType = new PetterResultType<StoreReview>();
            List<StoreReview> storeReviews = new List<StoreReview>();
            StoreReview storeReview = await db.StoreReviews.FindAsync(id);

            if (storeReview == null)
            {
                return NotFound();
            }

            storeReview.StateFlag = StateFlags.Delete;
            storeReview.DateDeleted = DateTime.Now;
            db.Entry(storeReview).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeReviews.Add(storeReview);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeReviews;

            return Ok(petterResultType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool storeReviewExists(int id)
        {
            return db.StoreReviews.Count(e => e.StoreReviewNo == id) > 0;
        }
    }
}