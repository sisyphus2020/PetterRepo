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
        // 2. 스토어 갤러리 상세 (O)
        // 3. 스토어 갤러리 등록 (O)
        // 4. 스토어 갤러리 수정 (O)
        // 5. 스토어 갤러리 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/StoreGalleries
        /// 스토어 갤러리 리스트
        /// </summary>
        /// <param name="petterRequestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGallery>))]
        public async Task<IHttpActionResult> GetStoreGalleries([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<StoreGallery> petterResultType = new PetterResultType<StoreGallery>();
            List<StoreGallery> list = new List<StoreGallery>();
            bool isSearch = false;

            // 검색 조건 
            if (!String.IsNullOrWhiteSpace(petterRequestType.Search))
            {
                isSearch = true;
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 댓글수
                case "replycount":
                    {
                        list = await db.StoreGalleries
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            //.OrderByDescending(p => p.ReviewCount)
                            .OrderByDescending(p => p.StoreGalleryNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToListAsync();
                        break;
                    }
                // 기본
                default:
                    {
                        list = await db.StoreGalleries
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            .OrderByDescending(p => p.StoreGalleryNo)
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
        /// GET: api/StoreGalleries/5
        /// 스토어 갤러리 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGalleryDTO>))]
        public async Task<IHttpActionResult> GetStoreGallery(int id)
        {
            PetterResultType<StoreGalleryDTO> petterResultType = new PetterResultType<StoreGalleryDTO>();
            List<StoreGalleryDTO> storeGalleries = new List<StoreGalleryDTO>();

            var storeGallery = await db.StoreGalleries.Where(p => p.StoreGalleryNo == id).Select(p => new StoreGalleryDTO
            {
                StoreGalleryNo = p.StoreGalleryNo,
                StoreNo = p.StoreNo,
                Content = p.Content,
                StateFlag = p.StateFlag,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                DateDeleted = p.DateDeleted,
                FileName = p.FileName,
                FilePath = p.FilePath,
                StoreGalleryStats = p.StoreGalleryStats.ToList(),
                StoreGalleryReplies = p.StoreGalleryReplies.ToList(),
                StoreGalleryFiles = p.StoreGalleryFiles.ToList()
            }).SingleOrDefaultAsync();


            if (storeGallery == null)
            {
                return NotFound();
            }

            storeGalleries.Add(storeGallery);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleries;

            return Ok(petterResultType);
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

                        // 갤러리 대표 이미지
                        if (fieldName == FieldName.StoreGalleryFieldName)
                        {
                            storeGallery.FileName = fileName;
                            storeGallery.FilePath = UploadPath.EventBoardPath;
                        }

                        storeGalleryFile.StoreGalleryNo = storeGallery.StoreGalleryNo;
                        storeGalleryFile.FileName = fileName;
                        storeGalleryFile.FilePath = UploadPath.StoreGalleryPath;
                        storeGalleryFile.DateModified = DateTime.Now;
                        storeGalleryFile.StateFlag = StateFlags.Use;

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

                        // 갤러리 대표 이미지
                        if (fieldName == FieldName.StoreGalleryFieldName)
                        {
                            storeGallery.FileName = fileName;
                            storeGallery.FilePath = UploadPath.EventBoardPath;
                        }

                        storeGalleryFile.FileName = fileName;
                        storeGalleryFile.FilePath = UploadPath.StoreGalleryPath;
                        storeGalleryFile.DateCreated = DateTime.Now;
                        storeGalleryFile.DateModified = DateTime.Now;
                        storeGalleryFile.StateFlag = StateFlags.Use;

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

        /// <summary>
        /// DELETE: api/StoreGalleries/5
        /// 스토어 갤러리 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreGallery>))]
        public async Task<IHttpActionResult> DeleteStoreGallery(int id)
        {
            // 인증 필요

            PetterResultType<StoreGallery> petterResultType = new PetterResultType<StoreGallery>();
            List<StoreGallery> storeGalleries = new List<StoreGallery>();
            StoreGallery storeGallery = await db.StoreGalleries.FindAsync(id);

            if (storeGallery == null)
            {
                return NotFound();
            }

            storeGallery.StateFlag = StateFlags.Delete;
            storeGallery.DateDeleted = DateTime.Now;
            db.Entry(storeGallery).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeGalleries.Add(storeGallery);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = storeGalleries;

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

        private bool StoreGalleryExists(int id)
        {
            return db.StoreGalleries.Count(e => e.StoreGalleryNo == id) > 0;
        }
    }
}