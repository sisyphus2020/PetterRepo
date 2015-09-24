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
    public class BoardController : ApiController
    {
        // 1. 스토어 소식 리스트 (X)
        // 2. 스토어 소식 상세 (O)
        // 3. 스토어 소식 등록 (O)
        // 4. 스토어 소식 수정 (O)
        // 5. 스토어 소식 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/Board
        /// 스토어 소식 리스트
        /// </summary>
        /// <param name="petterRequestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> GetBoard([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> list = new List<Board>();
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
                        list = await db.Boards
                            .Where(p => petterRequestType.CodeID == "A02003" ? p.CodeID == "A02003" : p.CodeID != "A02003" )
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            //.OrderByDescending(p => p.ReviewCount)
                            .OrderByDescending(p => p.BoardNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToListAsync();
                        break;
                    }
                // 기본
                default:
                    {
                        list = await db.Boards
                            .Where(p => petterRequestType.CodeID == "A02003" ? p.CodeID == "A02003" : p.CodeID != "A02003")
                            .Where(p => petterRequestType.StateFlag == "A" ? 1 == 1 : p.StateFlag == petterRequestType.StateFlag)
                            .Where(p => isSearch ? p.Content.Contains(petterRequestType.Search) : 1 == 1)
                            .OrderByDescending(p => p.BoardNo)
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
        /// GET: api/Board/5
        /// 스토어 소식 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardDTO>))]
        public async Task<IHttpActionResult> GetBoard(int id)
        //public async Task<IHttpActionResult> GetBoard(int id, int memberNo)
        {
            PetterResultType<BoardDTO> petterResultType = new PetterResultType<BoardDTO>();
            List<BoardDTO> list = new List<BoardDTO>();

            var Board = await db.Boards.Where(p => p.BoardNo == id).Select(p => new BoardDTO
            {
                BoardNo = p.BoardNo,
                StoreNo = p.StoreNo,
                CodeID = p.CodeID,
                Content = p.Content,
                StateFlag = p.StateFlag,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                DateDeleted = p.DateDeleted,
                FileName = p.FileName,
                FilePath = p.FilePath,
                BoardStats = p.BoardStats.ToList(),
                BoardFiles = p.BoardFiles.ToList(),
                BoardLikes = p.BoardLikes.ToList(),
                //isCount = p.BoardLikes.Where(p.MemberNO == memberNo),
                BoardReplies = p.BoardReplies.ToList()
            }).SingleOrDefaultAsync();


            if (Board == null)
            {
                return NotFound();
            }

            list.Add(Board);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = list;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/Board/5
        /// 스토어 소식 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> PutBoard(int id)
        {
            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> list = new List<Board>();
            List<BoardFile> BoardFiles = new List<BoardFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Board Board = await db.Boards.FindAsync(id);
            if (Board == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.BoardPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        BoardFile BoardFile = new BoardFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.BoardExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BoardWidth, FileSize.BoardHeight, ImageFormat.Png);

                        // 소식 대표 이미지
                        if (fieldName == FieldName.BoardFieldName)
                        {
                            Board.FileName = fileName;
                            Board.FilePath = UploadPath.BoardPath;
                        }

                        BoardFile.BoardNo = Board.BoardNo;
                        BoardFile.FileName = fileName;
                        BoardFile.FilePath = UploadPath.BoardPath;
                        BoardFile.DateModified = DateTime.Now;
                        BoardFile.StateFlag = StateFlags.Use;

                        BoardFiles.Add(BoardFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "StoreNo":
                            //    Board.StoreNo = int.Parse(item);
                            //    break;
                            case "Content":
                                Board.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                Board.StateFlag = StateFlags.Use;
                Board.DateModified = DateTime.Now;

                // 스토어 소식 수정
                db.Entry(Board).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 스토어 소식 파일 등록
                db.BoardFiles.AddRange(BoardFiles);
                int num1 = await this.db.SaveChangesAsync();

                list.Add(Board);
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
        /// POST: api/Board
        /// 스토어 소식 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> PostBoard()
        {
            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> list = new List<Board>();
            List<BoardFile> BoardFiles = new List<BoardFile>();

            Board Board = new Board();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.BoardPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        BoardFile BoardFile = new BoardFile();
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.BoardExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BoardWidth, FileSize.BoardHeight, ImageFormat.Png);

                        // 소식 대표 이미지
                        if (fieldName == FieldName.BoardFieldName)
                        {
                            Board.FileName = fileName;
                            Board.FilePath = UploadPath.BoardPath;
                        }

                        BoardFile.FileName = fileName;
                        BoardFile.FilePath = UploadPath.BoardPath;
                        BoardFile.DateCreated = DateTime.Now;
                        BoardFile.DateModified = DateTime.Now;
                        BoardFile.StateFlag = StateFlags.Use;

                        BoardFiles.Add(BoardFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "StoreNo":
                                Board.StoreNo = int.Parse(item);
                                break;
                            case "CodeID":
                                Board.CodeID = item;
                                break;
                            case "Content":
                                Board.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                Board.StateFlag = StateFlags.Use;
                Board.DateCreated = DateTime.Now;
                Board.DateModified = DateTime.Now;

                // 스토어 소식 등록
                db.Boards.Add(Board);
                int num = await this.db.SaveChangesAsync();

                // 스토어 소식 파일 등록
                foreach (var item in BoardFiles)
                {
                    item.BoardNo = Board.BoardNo;
                }

                db.BoardFiles.AddRange(BoardFiles);
                int num1 = await this.db.SaveChangesAsync();

                list.Add(Board);
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
        /// DELETE: api/Board/5
        /// 스토어 소식 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> DeleteBoard(int id)
        {
            // 인증 필요

            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> storeGalleries = new List<Board>();
            Board Board = await db.Boards.FindAsync(id);

            if (Board == null)
            {
                return NotFound();
            }

            Board.StateFlag = StateFlags.Delete;
            Board.DateDeleted = DateTime.Now;
            db.Entry(Board).State = EntityState.Modified;

            await db.SaveChangesAsync();

            storeGalleries.Add(Board);
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

        private bool BoardExists(int id)
        {
            return db.Boards.Count(e => e.BoardNo == id) > 0;
        }
    }
}