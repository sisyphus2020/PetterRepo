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
    public class StoreNewsController : ApiController
    {
        // 1. 스토어 소식 리스트 (X)
        // 2. 스토어 소식 상세 (O)
        // 3. 스토어 소식 등록 (O)
        // 4. 스토어 소식 수정 (O)
        // 5. 스토어 소식 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/StoreNews
        /// 스토어 소식 리스트
        /// </summary>
        /// <param name="petterRequestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNews>))]
        public async Task<IHttpActionResult> GetStoreNews([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<StoreNews> petterResultType = new PetterResultType<StoreNews>();
            List<StoreNews> list = new List<StoreNews>();
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
                        list = await db.StoreNews
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            //.OrderByDescending(p => p.ReviewCount)
                            .OrderByDescending(p => p.StoreNewsNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToListAsync();
                        break;
                    }
                // 기본
                default:
                    {
                        list = await db.StoreNews
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            .OrderByDescending(p => p.StoreNewsNo)
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
        /// GET: api/StoreNews/5
        /// 스토어 소식 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNewsDTO>))]
        public async Task<IHttpActionResult> GetStoreNews(int id)
        //public async Task<IHttpActionResult> GetStoreNews(int id, int memberNo)
        {
            PetterResultType<StoreNewsDTO> petterResultType = new PetterResultType<StoreNewsDTO>();
            List<StoreNewsDTO> list = new List<StoreNewsDTO>();

            var storeNews = await db.StoreNews.Where(p => p.StoreNewsNo == id).Select(p => new StoreNewsDTO
            {
                StoreNewsNo = p.StoreNewsNo,
                StoreNo = p.StoreNo,
                Content = p.Content,
                StateFlag = p.StateFlag,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                DateDeleted = p.DateDeleted,
                FileName = p.FileName,
                FilePath = p.FilePath,
                StoreNewsStats = p.StoreNewsStats.ToList(),
                StoreNewsFiles = p.StoreNewsFiles.ToList(),
                StoreNewsLikes = p.StoreNewsLikes.ToList(),
                //isCount = p.StoreNewsLikes.Where(p.MemberNO == memberNo),
                StoreNewsReplies = p.StoreNewsReplies.ToList()
            }).SingleOrDefaultAsync();


            if (storeNews == null)
            {
                return NotFound();
            }

            list.Add(storeNews);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = list;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/StoreNews/5
        /// 스토어 소식 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNews>))]
        public async Task<IHttpActionResult> PutStoreNews(int id)
        {
            PetterResultType<StoreNews> petterResultType = new PetterResultType<StoreNews>();
            List<StoreNews> list = new List<StoreNews>();
            List<StoreNewsFile> StoreNewsFiles = new List<StoreNewsFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            StoreNews StoreNews = await db.StoreNews.FindAsync(id);
            if (StoreNews == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StoreNewsPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        StoreNewsFile StoreNewsFile = new StoreNewsFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.StoreNewsExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreNewsWidth, FileSize.StoreNewsHeight, ImageFormat.Png);

                        // 소식 대표 이미지
                        if (fieldName == FieldName.StoreNewsFieldName)
                        {
                            StoreNews.FileName = fileName;
                            StoreNews.FilePath = UploadPath.EventBoardPath;
                        }

                        StoreNewsFile.StoreNewsNo = StoreNews.StoreNewsNo;
                        StoreNewsFile.FileName = fileName;
                        StoreNewsFile.FilePath = UploadPath.StoreNewsPath;
                        StoreNewsFile.DateModified = DateTime.Now;
                        StoreNewsFile.StateFlag = StateFlags.Use;

                        StoreNewsFiles.Add(StoreNewsFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "StoreNo":
                            //    StoreNews.StoreNo = int.Parse(item);
                            //    break;
                            case "Content":
                                StoreNews.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                StoreNews.StateFlag = StateFlags.Use;
                StoreNews.DateModified = DateTime.Now;

                // 스토어 소식 수정
                db.Entry(StoreNews).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 스토어 소식 파일 등록
                db.StoreNewsFiles.AddRange(StoreNewsFiles);
                int num1 = await this.db.SaveChangesAsync();

                list.Add(StoreNews);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = list;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/StoreNews
        /// 스토어 소식 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNews>))]
        public async Task<IHttpActionResult> PostStoreNews()
        {
            PetterResultType<StoreNews> petterResultType = new PetterResultType<StoreNews>();
            List<StoreNews> list = new List<StoreNews>();
            List<StoreNewsFile> StoreNewsFiles = new List<StoreNewsFile>();

            StoreNews storeNews = new StoreNews();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.StoreNewsPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        StoreNewsFile storeNewsFile = new StoreNewsFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.StoreNewsExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreNewsWidth, FileSize.StoreNewsHeight, ImageFormat.Png);

                        // 소식 대표 이미지
                        if (fieldName == FieldName.StoreNewsFieldName)
                        {
                            storeNews.FileName = fileName;
                            storeNews.FilePath = UploadPath.EventBoardPath;
                        }

                        storeNewsFile.FileName = fileName;
                        storeNewsFile.FilePath = UploadPath.StoreNewsPath;
                        storeNewsFile.DateCreated = DateTime.Now;
                        storeNewsFile.DateModified = DateTime.Now;
                        storeNewsFile.StateFlag = StateFlags.Use;

                        StoreNewsFiles.Add(storeNewsFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "StoreNo":
                                storeNews.StoreNo = int.Parse(item);
                                break;
                            case "Content":
                                storeNews.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                storeNews.StateFlag = StateFlags.Use;
                storeNews.DateCreated = DateTime.Now;
                storeNews.DateModified = DateTime.Now;

                // 스토어 소식 등록
                db.StoreNews.Add(storeNews);
                int num = await this.db.SaveChangesAsync();

                // 스토어 소식 파일 등록
                foreach (var item in StoreNewsFiles)
                {
                    item.StoreNewsNo = storeNews.StoreNewsNo;
                }

                db.StoreNewsFiles.AddRange(StoreNewsFiles);
                int num1 = await this.db.SaveChangesAsync();

                list.Add(storeNews);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = list;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/StoreNews/5
        /// 스토어 소식 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<StoreNews>))]
        public async Task<IHttpActionResult> DeleteStoreNews(int id)
        {
            // 인증 필요

            PetterResultType<StoreNews> petterResultType = new PetterResultType<StoreNews>();
            List<StoreNews> storeGalleries = new List<StoreNews>();
            StoreNews StoreNews = await db.StoreNews.FindAsync(id);

            if (StoreNews == null)
            {
                return NotFound();
            }

            StoreNews.StateFlag = StateFlags.Delete;
            StoreNews.DateDeleted = DateTime.Now;
            db.Entry(StoreNews).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeGalleries.Add(StoreNews);
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

        private bool StoreNewsExists(int id)
        {
            return db.StoreNews.Count(e => e.StoreNewsNo == id) > 0;
        }
    }
}