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
    public class CommonCodesController : ApiController
    {
        // 1. 공통코드 리스트 (X)
        // 2. 스토어 상세 (O)
        // 3. 스토어 등록 (O)
        // 4. 스토어 수정 (O)
        // 5. 스토어 삭제 (O)
        // 6. 스토어 ID 검색

        private PetterServiceContext db = new PetterServiceContext();

        /// <summary>
        /// GET: api/CommonCodes
        /// 공통코드 리스트(Search : ParentCodeID)
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<CommonCode>))]
        public async Task<IHttpActionResult> GetCommonCodes([FromUri] PetterRequestType petterRequestType)
        {
            PetterResultType<CommonCode> petterResultType = new PetterResultType<CommonCode>();
            List<CommonCode> list = new List<CommonCode>();

            var commonCode = db.CommonCodes.AsEnumerable();

            // 검색 조건 
            if (!String.IsNullOrEmpty(petterRequestType.Search))
            {
                commonCode = commonCode.Where(p => p.ParentCodeID != null && p.ParentCodeID.Contains(petterRequestType.Search));
            }

            #region 정렬 방식
            switch (petterRequestType.SortBy)
            {
                // 기본
                default:
                    {
                        list = commonCode
                            .OrderBy(p => p.OrderNo)
                            .Skip((petterRequestType.CurrentPage - 1) * petterRequestType.ItemsPerPage)
                            .Take(petterRequestType.ItemsPerPage).ToList();
                        break;
                    }
            }
            #endregion 정렬방식

            petterResultType.IsSuccessful = true;
            petterResultType.AffectedRow = list.Count();
            petterResultType.JsonDataSet = list.ToList();
            return Ok(petterResultType);
        }

        // GET: api/CommonCodes/5
        [ResponseType(typeof(CommonCode))]
        public async Task<IHttpActionResult> GetCommonCode(int id)
        {
            CommonCode commonCode = await db.CommonCodes.FindAsync(id);
            if (commonCode == null)
            {
                return NotFound();
            }

            return Ok(commonCode);
        }

        // PUT: api/CommonCodes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommonCode(int id, CommonCode commonCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commonCode.CodeNo)
            {
                return BadRequest();
            }

            db.Entry(commonCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommonCodeExists(id))
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

        // POST: api/CommonCodes
        [ResponseType(typeof(CommonCode))]
        public async Task<IHttpActionResult> PostCommonCode(CommonCode commonCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CommonCodes.Add(commonCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = commonCode.CodeNo }, commonCode);
        }

        // DELETE: api/CommonCodes/5
        [ResponseType(typeof(CommonCode))]
        public async Task<IHttpActionResult> DeleteCommonCode(int id)
        {
            CommonCode commonCode = await db.CommonCodes.FindAsync(id);
            if (commonCode == null)
            {
                return NotFound();
            }

            db.CommonCodes.Remove(commonCode);
            await db.SaveChangesAsync();

            return Ok(commonCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommonCodeExists(int id)
        {
            return db.CommonCodes.Count(e => e.CodeNo == id) > 0;
        }
    }
}