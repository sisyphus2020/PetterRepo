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
using System.Drawing.Imaging;
using System.Web;

namespace PetterService.Controllers
{
    public class NoticesController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/Notices
        /// 공지게시판 리스트
        /// </summary>
        /// <param name="petterRequestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Notice>))]
        public async Task<IHttpActionResult> GetNoitices([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<Notice> petterResultType = new PetterResultType<Notice>();
            List<Notice> list = new List<Notice>();
            bool isSearch = false;

            // 검색 조건 
            if (!String.IsNullOrWhiteSpace(petterRequestType.Search))
            {
                isSearch = true;
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 리뷰수
                case "reviewcount":
                    {
                        list = await db.Notices
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Title.Contains(petterRequestType.Search) : 1 == 1)
                            //.OrderByDescending(p => p.ReviewCount)
                            .OrderByDescending(p => p.NoticeNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToListAsync();
                        break;
                    }
                // 기본
                default:
                    {
                        list = await db.Notices
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Title.Contains(petterRequestType.Search) : 1 == 1)
                            .OrderByDescending(p => p.NoticeNo)
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
        /// GET: api/Notices/5
        /// 공지게시판 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<NoticeDTO>))]
        public async Task<IHttpActionResult> GetNotice(int id)
        {
            PetterResultType<NoticeDTO> petterResultType = new PetterResultType<NoticeDTO>();
            List<NoticeDTO> notices = new List<NoticeDTO>();

            //var notice = await db.Notices.FindAsync(id);

            var notice = await db.Notices.Where(p => p.NoticeNo == id).Select(p => new NoticeDTO
            {
                NoticeNo = p.NoticeNo,
                MemberNo = p.MemberNo,
                Title = p.Title,
                Content = p.Content,
                StateFlag = p.StateFlag,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                DateDeleted = p.DateDeleted,
                NoticeFiles = p.NoticeFiles.ToList(),
                NoticeReplies = p.NoticeReplies.ToList()
            }).SingleOrDefaultAsync();


            if (notice == null)
            {
                return NotFound();
            }

            notices.Add(notice);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = notices;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/Notices/5
        /// 공지게시판 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Notice>))]
        public async Task<IHttpActionResult> PutNotice(int id)
        {
            PetterResultType<Notice> petterResultType = new PetterResultType<Notice>();
            List<Notice> notices = new List<Notice>();
            List<NoticeFile> noticeFiles = new List<NoticeFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Notice notice = await db.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.NoticePath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        NoticeFile noticeFile = new NoticeFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.NoticeExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.NoticeWidth, FileSize.NoticeHeight, ImageFormat.Png);
                        noticeFile.NoticeNo = notice.NoticeNo;
                        noticeFile.FileName = fileName;
                        noticeFile.FilePath = UploadPath.NoticePath.Replace("~", "");

                        noticeFiles.Add(noticeFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberNo":
                                notice.MemberNo = int.Parse(item);
                                break;
                            case "Title":
                                notice.Title = item;
                                break;
                            case "Content":
                                notice.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                notice.StateFlag = StateFlags.Use;
                notice.DateModified = DateTime.Now;

                // 공지게시판 수정
                db.Entry(notice).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 공지게시판 파일 등록
                db.NoticeFiles.AddRange(noticeFiles);
                int num1 = await this.db.SaveChangesAsync();

                notices.Add(notice);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = notices;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/Notices
        /// 공지게피판 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Notice>))]
        public async Task<IHttpActionResult> PostNotice()
        {
            PetterResultType<Notice> petterResultType = new PetterResultType<Notice>();
            List<Notice> notices = new List<Notice>();
            List<NoticeFile> noticeFiles = new List<NoticeFile>();

            Notice notice = new Notice();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.NoticePath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        NoticeFile noticeFile = new NoticeFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.NoticeExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.NoticeWidth, FileSize.NoticeHeight, ImageFormat.Png);
                        noticeFile.FileName = fileName;
                        noticeFile.FilePath = UploadPath.NoticePath.Replace("~", "");

                        noticeFiles.Add(noticeFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberNo":
                                notice.MemberNo = int.Parse(item);
                                break;
                            case "Title":
                                notice.Title = item;
                                break;
                            case "Content":
                                notice.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                notice.StateFlag = StateFlags.Use;
                notice.DateCreated = DateTime.Now;
                notice.DateModified = DateTime.Now;

                // 공지게시판 등록
                db.Notices.Add(notice);
                int num = await this.db.SaveChangesAsync();

                // 공지게시판 파일 등록
                foreach (var item in noticeFiles)
                {
                    item.NoticeNo = notice.NoticeNo;
                }
                
                db.NoticeFiles.AddRange(noticeFiles);
                int num1 = await this.db.SaveChangesAsync();

                notices.Add(notice);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = notices;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/Notices/5
        /// 공지게시판 삭제 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Notice>))]
        public async Task<IHttpActionResult> DeleteNotice(int id)
        {
            PetterResultType<Notice> petterResultType = new PetterResultType<Notice>();
            List<Notice> notices = new List<Notice>();
            Notice notice = await db.Notices.FindAsync(id);

            if (notice == null)
            {
                return NotFound();
            }

            notice.StateFlag = StateFlags.Delete;
            notice.DateDeleted = DateTime.Now;
            db.Entry(notice).State = EntityState.Modified;

            await db.SaveChangesAsync();

            notices.Add(notice);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = notices;

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

        private bool NoticeExists(int id)
        {
            return db.Notices.Count(e => e.NoticeNo == id) > 0;
        }
    }
}