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
    public class EventBoardsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/EventBoards
        public IQueryable<EventBoard> GetEventBoards()
        {
            return db.EventBoards;
        }

        /// <summary>
        /// GET: api/EventBoards/5
        /// 이벤트게시판 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<EventBoard>))]
        public async Task<IHttpActionResult> GetEventBoard(int id)
        {
            PetterResultType<EventBoard> petterResultType = new PetterResultType<EventBoard>();
            List<EventBoard> eventBoards = new List<EventBoard>();

            var eventBoard = await db.EventBoards.FindAsync(id);

            var pension = await db.EventBoards.Where(p => p.EventBoardNo == id).Select(p => new EventBoardDTO
            {
                EventBoardNo = p.EventBoardNo,
                MemberNo = p.MemberNo,
                Title = p.Title,
                Content = p.Content,
                StateFlag = p.StateFlag,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                DateDeleted = p.DateDeleted,
                EventBoardFiles = p.EventBoardFiles.ToList()
            }).SingleOrDefaultAsync();


            if (eventBoard == null)
            {
                return NotFound();
            }

            eventBoards.Add(eventBoard);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = eventBoards;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/EventBoards/5
        /// 이벤트게시판 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<EventBoard>))]
        public async Task<IHttpActionResult> PutEventBoard(int id)
        {
            PetterResultType<EventBoard> petterResultType = new PetterResultType<EventBoard>();
            List<EventBoard> eventBoards = new List<EventBoard>();
            List<EventBoardFile> eventBoardFiles = new List<Models.EventBoardFile>();
            //EventBoardFile eventBoardFile = new Models.EventBoardFile();
            //EventBoard eventBoard = new EventBoard();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            EventBoard eventBoard = await db.EventBoards.FindAsync(id);
            if (eventBoard == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.EventBoardPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        EventBoardFile eventBoardFile = new Models.EventBoardFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.EventBoardExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.EventBoardWidth, FileSize.EventBoardHeight, ImageFormat.Png);
                        eventBoardFile.EventBoardNo = eventBoard.EventBoardNo;
                        eventBoardFile.FileName = fileName;
                        eventBoardFile.FilePath = UploadPath.EventBoardPath;

                        eventBoardFiles.Add(eventBoardFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberNo":
                                eventBoard.MemberNo = int.Parse(item);
                                break;
                            case "Title":
                                eventBoard.Title = item;
                                break;
                            case "Content":
                                eventBoard.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                eventBoard.StateFlag = StateFlag.Use;
                eventBoard.DateModified = DateTime.Now;

                // 이벤트게시판 등록
                db.Entry(eventBoard).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 이벤트게시판 파일 등록
                db.EventBoardFiles.AddRange(eventBoardFiles);
                int num1 = await this.db.SaveChangesAsync();

                eventBoards.Add(eventBoard);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = eventBoards;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/EventBoards
        /// 이벤트게시판 수정
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<EventBoard>))]
        public async Task<IHttpActionResult> PostEventBoard()
        {
            PetterResultType<EventBoard> petterResultType = new PetterResultType<EventBoard>();
            List<EventBoard> eventBoards = new List<EventBoard>();
            List<EventBoardFile> eventBoardFiles = new List<Models.EventBoardFile>();
            
            EventBoard eventBoard = new EventBoard();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.EventBoardPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        EventBoardFile eventBoardFile = new Models.EventBoardFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.EventBoardExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.EventBoardWidth, FileSize.EventBoardHeight, ImageFormat.Png);
                        eventBoardFile.FileName = fileName;
                        eventBoardFile.FilePath = UploadPath.EventBoardPath;


                        eventBoardFiles.Add(eventBoardFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberNo":
                                eventBoard.MemberNo = int.Parse(item);
                                break;
                            case "Title":
                                eventBoard.Title = item;
                                break;
                            case "Content":
                                eventBoard.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                eventBoard.StateFlag = StateFlag.Use;
                eventBoard.DateCreated = DateTime.Now;
                eventBoard.DateModified = DateTime.Now;

                // 이벤트게시판 등록
                db.EventBoards.Add(eventBoard);
                int num = await this.db.SaveChangesAsync();

                // 이벤트게시판 파일 등록
                foreach (var item in eventBoardFiles)
                {
                    item.EventBoardNo = eventBoard.EventBoardNo;
                }
                db.EventBoardFiles.AddRange(eventBoardFiles);
                int num1 = await this.db.SaveChangesAsync();

                eventBoards.Add(eventBoard);
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = eventBoards;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/EventBoards/5
        /// 이벤트게시판 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(EventBoard))]
        public async Task<IHttpActionResult> DeleteEventBoard(int id)
        {
            // 인증 필요

            PetterResultType<EventBoard> petterResultType = new PetterResultType<EventBoard>();
            List<EventBoard> eventBoards = new List<EventBoard>();
            EventBoard eventBoard = await db.EventBoards.FindAsync(id);

            if (eventBoard == null)
            {
                return NotFound();
            }

            eventBoard.StateFlag = StateFlag.Delete;
            eventBoard.DateDeleted = DateTime.Now;
            db.Entry(eventBoard).State = EntityState.Modified;

            await db.SaveChangesAsync();

            eventBoards.Add(eventBoard);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = eventBoards;

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

        private bool EventBoardExists(int id)
        {
            return db.EventBoards.Count(e => e.EventBoardNo == id) > 0;
        }
    }
}