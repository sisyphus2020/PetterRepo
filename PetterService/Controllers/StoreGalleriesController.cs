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
    public class StoreGalleriesController : ApiController
    {
        // 1. 스토어 갤러리 리스트 (X)
        // 2. 스토어 갤러리 상세 (X)
        // 3. 스토어 갤러리 등록 (O)
        // 4. 스토어 갤러리 수정 (O)
        // 5. 스토어 갤러리 삭제 (X)

        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/StoreGalleries
        public IQueryable<StoreGallery> GetStoreGalleries()
        {
            return db.StoreGalleries;
        }

        // GET: api/StoreGalleries/5
        [ResponseType(typeof(StoreGallery))]
        public async Task<IHttpActionResult> GetStoreGallery(int id)
        {
            StoreGallery storeGallery = await db.StoreGalleries.FindAsync(id);
            if (storeGallery == null)
            {
                return NotFound();
            }

            return Ok(storeGallery);
        }

        /// <summary>
        /// PUT: api/StoreGalleries/5
        /// 스토어 갤러리 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGallery>))]
        public async Task<IHttpActionResult> PutStoreGallery(int id)
        {
            PetterResultType<StoreGallery> petterResultType = new PetterResultType<StoreGallery>();
            List<StoreGallery> storeGalleries = new List<StoreGallery>();
            List<StoreGalleryFile> storeGalleryFiles = new List<StoreGalleryFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            StoreGallery storeGallery = await db.StoreGalleries.FindAsync(id);
            if (storeGallery == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StoreGalleryPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        StoreGalleryFile storeGalleryFile = new StoreGalleryFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.StoreGalleryExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreGalleryWidth, FileSize.StoreGalleryHeight, ImageFormat.Png);
                        storeGalleryFile.StoreGalleryNo = storeGallery.StoreGalleryNo;
                        storeGalleryFile.FileName = fileName;
                        storeGalleryFile.FilePath = UploadPath.StoreGalleryPath;

                        storeGalleryFiles.Add(storeGalleryFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "StoreNo":
                            //    storeGallery.StoreNo = int.Parse(item);
                            //    break;
                            case "Content":
                                storeGallery.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                storeGallery.StateFlag = StateFlags.Use;
                storeGallery.DateModified = DateTime.Now;

                // 스토어 갤러리 수정
                db.Entry(storeGallery).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 스토어 갤러리 파일 등록
                db.StoreGalleryFiles.AddRange(storeGalleryFiles);
                int num1 = await this.db.SaveChangesAsync();

                storeGalleries.Add(storeGallery);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = storeGalleries;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/StoreGalleries
        /// 스토어 갤러리 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGallery>))]
        public async Task<IHttpActionResult> PostStoreGallery()
        {
            PetterResultType<StoreGallery> petterResultType = new PetterResultType<StoreGallery>();
            List<StoreGallery> storeGalleries = new List<StoreGallery>();
            List<StoreGalleryFile> storeGalleryFiles = new List<StoreGalleryFile>();

            StoreGallery storeGallery = new StoreGallery();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StoreGalleryPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        StoreGalleryFile storeGalleryFile = new StoreGalleryFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.StoreGalleryExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreGalleryWidth, FileSize.StoreGalleryHeight, ImageFormat.Png);
                        storeGalleryFile.FileName = fileName;
                        storeGalleryFile.FilePath = UploadPath.StoreGalleryPath;

                        storeGalleryFiles.Add(storeGalleryFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "StoreNo":
                                storeGallery.StoreNo = int.Parse(item);
                                break;
                            case "Content":
                                storeGallery.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                storeGallery.StateFlag = StateFlags.Use;
                storeGallery.DateCreated = DateTime.Now;
                storeGallery.DateModified = DateTime.Now;

                // 스토어 갤러리 등록
                db.StoreGalleries.Add(storeGallery);
                int num = await this.db.SaveChangesAsync();

                // 스토어 갤러리 파일 등록
                foreach (var item in storeGalleryFiles)
                {
                    item.StoreGalleryNo = storeGallery.StoreGalleryNo;
                }

                db.StoreGalleryFiles.AddRange(storeGalleryFiles);
                int num1 = await this.db.SaveChangesAsync();

                storeGalleries.Add(storeGallery);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = storeGalleries;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        // DELETE: api/StoreGalleries/5
        [ResponseType(typeof(StoreGallery))]
        public async Task<IHttpActionResult> DeleteStoreGallery(int id)
        {
            StoreGallery storeGallery = await db.StoreGalleries.FindAsync(id);
            if (storeGallery == null)
            {
                return NotFound();
            }

            db.StoreGalleries.Remove(storeGallery);
            await db.SaveChangesAsync();

            return Ok(storeGallery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGalleryExists(int id)
        {
            return db.StoreGalleries.Count(e => e.StoreGalleryNo == id) > 0;
        }
    }
}