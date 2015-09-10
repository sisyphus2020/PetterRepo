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
using System.Data.Entity.Spatial;
using System.Web.Hosting;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

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

        //GET: api/Members/5
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> GetMember(int id)
        {
            PetterResultType<MemberDTO> petterResultType = new PetterResultType<MemberDTO>();
            var memberDetail = await db.Members.Where(p => p.MemberNo == id).Select(p => new MemberDTO
            {
                MemberNo = p.MemberNo,
                MemberID = p.MemberID,
                Password = p.Password,
                NickName = p.NickName,
                PictureName = p.PictureName,
                PicturePath = p.PicturePath,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified
            }).SingleOrDefaultAsync();

            if (memberDetail == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = memberDetail;
            return Ok(petterResultType);
        }

        /// <summary>
        /// GET: api/Members/MemberID/sisyphus2020@naver.com
        /// MemberID 중복 확인
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
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
                Longitude = p.Longitude,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified
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
                Longitude = p.Longitude,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified
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
        public async Task<IHttpActionResult> PutMember(int id)
        {
            PetterResultType<Member> petterResultType = new PetterResultType<Member>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.MemberPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.MemberExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.MemberWidth, FileSize.MemberHeight, ImageFormat.Png);
                        member.PictureName = fileName;
                        member.PicturePath = UploadPath.MemberPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "Password":
                                member.Password = item;
                                break;
                            //case "NickName":
                            //    member.NickName = item;
                            //    break;
                            case "PictureName":
                                member.PictureName = item;
                                break;
                            case "PicturePath":
                                member.PicturePath = item;
                                break;
                            case "Latitude":
                                member.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                member.Longitude = Convert.ToDouble(item);
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", member.Latitude, member.Longitude);
                member.Coordinate = DbGeography.FromText(point);
                member.DateModified = DateTime.Now;
                db.Entry(member).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                                
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = member;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        //// POST: api/Members
        //[ResponseType(typeof(PetterResultType<Member>))]
        //public async Task<IHttpActionResult> PostMember(Member member)
        //{
        //    PetterResultType<Member> petterResultType = new PetterResultType<Member>();

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    string point = string.Format("POINT({0} {1})", member.Latitude, member.Longitude);
        //    member.Coordinate = DbGeography.FromText(point);
        //    member.DateCreated = DateTime.Now;
        //    member.DateModified = DateTime.Now;

        //    db.Members.Add(member);
        //    await db.SaveChangesAsync();

        //    petterResultType.IsSuccessful = true;
        //    petterResultType.JsonDataSet = member;
        //    return Ok(petterResultType);
        //}


        // POST: api/Members
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> PostMember()
        {
            PetterResultType<Member> petterResultType = new PetterResultType<Member>();
            Member member = new Member();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.MemberPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.MemberExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.MemberWidth, FileSize.MemberHeight, ImageFormat.Png);
                        member.PictureName = fileName;
                        member.PicturePath = UploadPath.MemberPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberNo":
                                member.MemberNo = int.Parse(item);
                                break;
                            case "MemberID":
                                member.MemberID = item.ToLower();
                                break;
                            case "Password":
                                member.Password = item;
                                break;
                            case "NickName":
                                member.NickName = item;
                                break;
                            case "PictureName":
                                member.PictureName = item;
                                break;
                            case "PicturePath":
                                member.PicturePath = item;
                                break;
                            case "Latitude":
                                member.Latitude = Convert.ToDouble(item);
                                break;
                            case "Longitude":
                                member.Longitude = Convert.ToDouble(item);
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                string point = string.Format("POINT({0} {1})", member.Latitude, member.Longitude);
                member.Coordinate = DbGeography.FromText(point);
                member.StateFlag = StateFlag.Use;
                member.Route = Route.App;
                member.DateCreated = DateTime.Now;
                member.DateModified = DateTime.Now;
                db.Members.Add(member);
                int num = await this.db.SaveChangesAsync();

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = member;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        //// DELETE: api/Members/5
        //[ResponseType(typeof(Member))]
        //public async Task<IHttpActionResult> DeleteMember(int id)
        //{
        //    Member member = await db.Members.FindAsync(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Members.Remove(member);
        //    await db.SaveChangesAsync();

        //    return Ok(member);
        //}

        // DELETE: api/Members/5
        [ResponseType(typeof(PetterResultType<Member>))]
        public async Task<IHttpActionResult> DeleteMember(int id)
        {
            PetterResultType<Member> petterResultType = new PetterResultType<Member>();

            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.StateFlag = StateFlag.Delete;
            db.Entry(member).State = EntityState.Modified;

            await db.SaveChangesAsync();

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = member;

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

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberNo == id) > 0;
        }
    }
}