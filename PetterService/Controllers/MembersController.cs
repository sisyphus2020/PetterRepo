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
    public class MembersController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/Members
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        // GET: api/Members/5
        //[ResponseType(typeof(Member))]
        //public async Task<IHttpActionResult> GetMember(int id)
        //{
        //    Member member = await db.Members.FindAsync(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(member);
        //}

        //GET: api/Members/MemberID/sisyphus2020@naver.com
        [HttpGet]
        [ActionName("MemberID")]
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> GetMemberByID(string memberID)
        {
            PetterResultType<MemberDTO> petterResultType = new PetterResultType<MemberDTO>();
            var memberDetail = await db.Members.Where(p => p.MemberID == memberID.Trim().ToLower()).Select(p => new MemberDTO
            {
                MemberNo = p.MemberNo,
                MemberID = p.MemberID,
                Password = p.Password,
                NickName = p.NickName,
                PictureName = p.PictureName,
                PicturePath = p.PicturePath,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).SingleOrDefaultAsync();

            if (memberDetail == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = memberDetail;
            return Ok(petterResultType);
        }

        //GET: api/Members/MemberID/sisyphus2020@naver.com
        [HttpGet]
        [ActionName("MemberCheck")]
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> GetMemberCheckByID(string memberID)
        {
            PetterResultType<MemberDTO> petterResultType = new PetterResultType<MemberDTO>();
            var memberDetail = await db.Members.Where(p => p.MemberID == memberID.Trim().ToLower()).SingleOrDefaultAsync();

            if (memberDetail != null)
            {
                petterResultType.IsSuccessful = true;
                petterResultType.ErrorMessage = ResultMessage.MemberSearchByID;
                //petterResultType.JsonDataSet = memberDetail;
            }

            petterResultType.IsSuccessful = false;
            petterResultType.ErrorMessage = ResultErrorMessage.MemberSearchByID;

            return Ok(petterResultType);
        }

        //GET: api/Members/NickName/victory
        [HttpGet]
        [ActionName("NickName")]
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> GetMemberByNickName(string nickName)
        {
            PetterResultType<MemberDTO> petterResultType = new PetterResultType<MemberDTO>();
            var memberDetail = await db.Members.Where(p => p.NickName == nickName.Trim().ToLower()).Select(p => new MemberDTO
            {
                MemberNo = p.MemberNo,
                MemberID = p.MemberID,
                Password = p.Password,
                NickName = p.NickName,
                PictureName = p.PictureName,
                PicturePath = p.PicturePath,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).SingleOrDefaultAsync();

            if (memberDetail == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = memberDetail;
            return Ok(petterResultType);
        }

        //GET: api/Members/MemberID/sisyphus2020@naver.com
        [HttpGet]
        [ActionName("MemberCheck")]
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> GetMemberCheckByNickName(string nickName)
        {
            PetterResultType<MemberDTO> petterResultType = new PetterResultType<MemberDTO>();
            var memberDetail = await db.Members.Where(p => p.MemberID == nickName.Trim().ToLower()).SingleOrDefaultAsync();

            if (memberDetail != null)
            {
                petterResultType.IsSuccessful = true;
                petterResultType.ErrorMessage = ResultMessage.MemberSearchByNickName;
                //petterResultType.JsonDataSet = memberDetail;
            }

            petterResultType.IsSuccessful = false;
            petterResultType.ErrorMessage = ResultErrorMessage.MemberSearchByNickName;

            return Ok(petterResultType);
        }

        // PUT: api/Members/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMember(int id, Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.MemberNo)
            {
                return BadRequest();
            }

            db.Entry(member).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        // POST: api/Members
        [ResponseType(typeof(Member))]
        public async Task<IHttpActionResult> PostMember(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Members.Add(member);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = member.MemberNo }, member);
        }

        // DELETE: api/Members/5
        [ResponseType(typeof(Member))]
        public async Task<IHttpActionResult> DeleteMember(int id)
        {
            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            await db.SaveChangesAsync();

            return Ok(member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberNo == id) > 0;
        }
    }
}