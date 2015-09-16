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

            //var EventBoards = await db.EventBoards.ToListAsync();

            // 검색 조건 
            //if (!String.IsNullOrWhiteSpace(petterRequestType.Search))
            //{
            //    //EventBoards = EventBoards.Where(p => p.Title.Contains(petterRequestType.Search));
            //}

            //// 상태 조건 
            //if (!String.IsNullOrWhiteSpace(petterRequestType.Search) && petterRequestType.StateFlag != "A")
            //{
            //    //EventBoards = EventBoards.Where(p => p.StateFlag == petterRequestType.StateFlag);
            //}

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
                            //.OrderByDescending(p => p.EventBoardNo)
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

        // GET: api/Notices/5
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

        // PUT: api/Notices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNotice(int id, Notice notice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notice.NoticeNo)
            {
                return BadRequest();
            }

            db.Entry(notice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Notices
        [ResponseType(typeof(Notice))]
        public async Task<IHttpActionResult> PostNotice(Notice notice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notices.Add(notice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = notice.NoticeNo }, notice);
        }

        // DELETE: api/Notices/5
        [ResponseType(typeof(Notice))]
        public async Task<IHttpActionResult> DeleteNotice(int id)
        {
            Notice notice = await db.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }

            db.Notices.Remove(notice);
            await db.SaveChangesAsync();

            return Ok(notice);
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