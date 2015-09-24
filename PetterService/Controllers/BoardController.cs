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
        // 1. 게시판 리스트 (X)
        // 2. 게시판 상세 (O)
        // 3. 게시판 등록 (O)
        // 4. 게시판 수정 (O)
        // 5. 게시판 삭제 (O)

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/Board
        /// 게시판 리스트
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
            petterResultType.AffectedRow = list.Count();
            petterResultType.JsonDataSet = list.ToList();
            return Ok(petterResultType);
        }

        /// <summary>
        /// GET: api/Board/5
        /// 게시판 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<BoardDTO>))]
        public async Task<IHttpActionResult> GetBoard(int id)
        //public async Task<IHttpActionResult> GetBoard(int id, int memberNo)
        {
            PetterResultType<BoardDTO> petterResultType = new PetterResultType<BoardDTO>();
            List<BoardDTO> list = new List<BoardDTO>();

            var board = await db.Boards.Where(p => p.BoardNo == id).Select(p => new BoardDTO
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


            if (board == null)
            {
                return NotFound();
            }

            list.Add(board);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = list;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/Board/5
        /// 게시판 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> PutBoard(int id)
        {
            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> list = new List<Board>();
            List<BoardFile> boardFiles = new List<BoardFile>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Board board = await db.Boards.FindAsync(id);
            if (board == null)
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
                        BoardFile boardFile = new BoardFile();
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
                            board.FileName = fileName;
                            board.FilePath = UploadPath.BoardPath;
                        }

                        boardFile.BoardNo = board.BoardNo;
                        boardFile.FileName = fileName;
                        boardFile.FilePath = UploadPath.BoardPath;
                        boardFile.DateModified = DateTime.Now;
                        boardFile.StateFlag = StateFlags.Use;

                        boardFiles.Add(boardFile);
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
                                board.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                board.StateFlag = StateFlags.Use;
                board.DateModified = DateTime.Now;

                // 게시판 수정
                db.Entry(board).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                // 게시판 파일 등록
                db.BoardFiles.AddRange(boardFiles);
                int num1 = await this.db.SaveChangesAsync();

                list.Add(board);
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
        /// 게시판 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> PostBoard()
        {
            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> list = new List<Board>();
            List<BoardFile> boardFiles = new List<BoardFile>();

            Board board = new Board();

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
                        BoardFile boardFile = new BoardFile();
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
                            board.FileName = fileName;
                            board.FilePath = UploadPath.BoardPath;
                        }

                        boardFile.FileName = fileName;
                        boardFile.FilePath = UploadPath.BoardPath;
                        boardFile.DateCreated = DateTime.Now;
                        boardFile.DateModified = DateTime.Now;
                        boardFile.StateFlag = StateFlags.Use;

                        boardFiles.Add(boardFile);
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "StoreNo":
                                board.StoreNo = int.Parse(item);
                                break;
                            case "CodeID":
                                board.CodeID = item;
                                break;
                            case "Content":
                                board.Content = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                board.StateFlag = StateFlags.Use;
                board.DateCreated = DateTime.Now;
                board.DateModified = DateTime.Now;

                // 게시판 등록
                db.Boards.Add(board);
                int num = await this.db.SaveChangesAsync();

                // 게시판 파일 등록
                foreach (var item in boardFiles)
                {
                    item.BoardNo = board.BoardNo;
                }

                db.BoardFiles.AddRange(boardFiles);
                int num1 = await this.db.SaveChangesAsync();

                list.Add(board);
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
        /// 게시판 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Board>))]
        public async Task<IHttpActionResult> DeleteBoard(int id)
        {
            // 인증 필요

            PetterResultType<Board> petterResultType = new PetterResultType<Board>();
            List<Board> list = new List<Board>();
            Board board = await db.Boards.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            board.StateFlag = StateFlags.Delete;
            board.DateDeleted = DateTime.Now;
            db.Entry(board).State = EntityState.Modified;

            await db.SaveChangesAsync();

            list.Add(board);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = list;

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