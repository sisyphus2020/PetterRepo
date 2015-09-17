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
    public class BeautyShopReviewsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/BeautyShopReviews
        public IQueryable<BeautyShopReview> GetBeautyShopReviews()
        {
            return db.BeautyShopReviews;
        }

        // GET: api/BeautyShopReviews/5
        [ResponseType(typeof(BeautyShopReview))]
        public async Task<IHttpActionResult> GetBeautyShopReview(int id)
        {
            BeautyShopReview beautyShopReview = await db.BeautyShopReviews.FindAsync(id);
            if (beautyShopReview == null)
            {
                return NotFound();
            }

            return Ok(beautyShopReview);
        }

        /// <summary>
        /// PUT: api/BeautyShopReviews/5
        /// 미용리뷰 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BeautyShopReview>))]
        public async Task<IHttpActionResult> PutBeautyShopReview(int id)
        {
            PetterResultType<BeautyShopReview> petterResultType = new PetterResultType<BeautyShopReview>();
            List<BeautyShopReview> beautyShopReviews = new List<BeautyShopReview>();
            List<BeautyShopReviewFile> beautyShopReviewFiles = new List<BeautyShopReviewFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            BeautyShopReview beautyShopReview = await db.BeautyShopReviews.FindAsync(id);
            if (beautyShopReview == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.BeautyShopReviewPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        BeautyShopReviewFile beautyShopReviewFile = new BeautyShopReviewFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.BeautyShopReviewExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BeautyShopReviewWidth, FileSize.BeautyShopReviewHeight, ImageFormat.Png);
                        beautyShopReviewFile.BeautyShopReviewNo = beautyShopReview.BeautyShopReviewNo;
                        beautyShopReviewFile.FileName = fileName;
                        beautyShopReviewFile.FilePath = UploadPath.BeautyShopReviewPath;

                        beautyShopReviewFiles.Add(beautyShopReviewFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "BeautyShopNo":
                            //    beautyShopReview.BeautyShopNo = int.Parse(item);
                            //    break;
                            //case "MemberNo":
                            //    beautyShopReview.MemberNo = int.Parse(item);
                            //    break;
                            case "Content":
                                beautyShopReview.Content = item;
                                break;
                            case "Grade":
                                beautyShopReview.Grade = Convert.ToDouble(item);
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                beautyShopReview.StateFlag = StateFlags.Use;
                beautyShopReview.DateModified = DateTime.Now;

                // 미용리뷰 수정
                db.Entry(beautyShopReview).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 미용리뷰 파일 등록
                db.BeautyShopReviewFiles.AddRange(beautyShopReviewFiles);
                int num1 = await this.db.SaveChangesAsync();

                beautyShopReviews.Add(beautyShopReview);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = beautyShopReviews;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/BeautyShopReviews
        /// 미용리뷰 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BeautyShopReview>))]
        public async Task<IHttpActionResult> PostBeautyShopReview()
        {
            PetterResultType<BeautyShopReview> petterResultType = new PetterResultType<BeautyShopReview>();
            List<BeautyShopReview> beautyShopReviews = new List<BeautyShopReview>();
            List<BeautyShopReviewFile> beautyShopReviewFiles = new List<BeautyShopReviewFile>();

            BeautyShopReview beautyShopReview = new BeautyShopReview();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.BeautyShopReviewPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        BeautyShopReviewFile beautyShopReviewFile = new BeautyShopReviewFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.BeautyShopReviewExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BeautyShopReviewWidth, FileSize.BeautyShopReviewHeight, ImageFormat.Png);
                        beautyShopReviewFile.FileName = fileName;
                        beautyShopReviewFile.FilePath = UploadPath.BeautyShopReviewPath;

                        beautyShopReviewFiles.Add(beautyShopReviewFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "BeautyShopNo":
                                beautyShopReview.BeautyShopNo = int.Parse(item);
                                break;
                            case "MemberNo":
                                beautyShopReview.MemberNo = int.Parse(item);
                                break;
                            case "Content":
                                beautyShopReview.Content = item;
                                break;
                            case "Grade":
                                beautyShopReview.Grade = Convert.ToDouble(item);
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                beautyShopReview.StateFlag = StateFlags.Use;
                beautyShopReview.DateCreated = DateTime.Now;
                beautyShopReview.DateModified = DateTime.Now;

                // 미용리뷰 등록
                db.BeautyShopReviews.Add(beautyShopReview);
                int num = await this.db.SaveChangesAsync();

                // 미용리뷰 파일 등록
                foreach (var item in beautyShopReviewFiles)
                {
                    item.BeautyShopReviewNo = beautyShopReview.BeautyShopReviewNo;
                }

                db.BeautyShopReviewFiles.AddRange(beautyShopReviewFiles);
                int num1 = await this.db.SaveChangesAsync();

                beautyShopReviews.Add(beautyShopReview);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = beautyShopReviews;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        // DELETE: api/BeautyShopReviews/5
        [ResponseType(typeof(BeautyShopReview))]
        public async Task<IHttpActionResult> DeleteBeautyShopReview(int id)
        {
            BeautyShopReview beautyShopReview = await db.BeautyShopReviews.FindAsync(id);
            if (beautyShopReview == null)
            {
                return NotFound();
            }

            db.BeautyShopReviews.Remove(beautyShopReview);
            await db.SaveChangesAsync();

            return Ok(beautyShopReview);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeautyShopReviewExists(int id)
        {
            return db.BeautyShopReviews.Count(e => e.BeautyShopReviewNo == id) > 0;
        }
    }
}