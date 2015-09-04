using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PetterService.Models;
using PetterService.Common;

namespace PetterService.Controllers
{
    public class LoginController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/Login
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        /// <summary>
        /// GET api/Login?memberID={memberID}&password={password}
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> GetMember(string memberID, string password)
        {
            PetterResultType<MemberDTO> petterResultType = new PetterResultType<MemberDTO>();
            var memberDetail = await db.Members.Where(p => p.MemberID == memberID.Trim().ToLower() & p.Password == password).Select(p => new MemberDTO
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

        // PUT: api/Login/5
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

        // POST: api/Login
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

        // DELETE: api/Login/5
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