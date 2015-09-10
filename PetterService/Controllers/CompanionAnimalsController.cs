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
    public class CompanionAnimalsController : ApiController
    {
        private PetterServiceContext db = new PetterServiceContext();

        // GET: api/CompanionAnimals
        public IQueryable<CompanionAnimal> GetCompanionAnimals()
        {
            return db.CompanionAnimals;
        }

        // GET: api/CompanionAnimals/5
        [ResponseType(typeof(CompanionAnimal))]
        public async Task<IHttpActionResult> GetCompanionAnimal(int id)
        {
            CompanionAnimal companionAnimal = await db.CompanionAnimals.FindAsync(id);
            if (companionAnimal == null)
            {
                return NotFound();
            }

            return Ok(companionAnimal);
        }

        // PUT: api/CompanionAnimals/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCompanionAnimal(int id, CompanionAnimal companionAnimal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != companionAnimal.CompanionAnimalNo)
            {
                return BadRequest();
            }

            db.Entry(companionAnimal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanionAnimalExists(id))
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

        // POST: api/CompanionAnimals
        [ResponseType(typeof(PetterResultType<CompanionAnimal>))]
        public async Task<IHttpActionResult> PostCompanionAnimal()
        {
            PetterResultType<CompanionAnimal> petterResultType = new PetterResultType<CompanionAnimal>();
            //List<BeautyShopService> beautyShopServices = new List<BeautyShopService>();
            //List<BeautyShopHoliday> beautyShopHolidays = new List<BeautyShopHoliday>();
            CompanionAnimal companionAnimal = new CompanionAnimal();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.CompanionAnimalPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();

                        string fileName = Utilities.additionFileName(content.Headers.ContentDisposition.FileName.Trim('"'));

                        if (!FileExtension.CompanionAnimalExtensions.Any(x => x.Equals(Path.GetExtension(fileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
                        {
                            petterResultType.IsSuccessful = false;
                            petterResultType.JsonDataSet = null;
                            petterResultType.ErrorMessage = ResultErrorMessage.FileTypeError;
                            return Ok(petterResultType);
                        }

                        string fullPath = Path.Combine(folder, fileName);
                        File.WriteAllBytes(fullPath, file);
                        string thumbnamil = Path.GetFileNameWithoutExtension(fileName) + "_thumbnail" + Path.GetExtension(fileName);

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.BeautyShopWidth, FileSize.BeautyShopHeight, ImageFormat.Png);
                        companionAnimal.PictureName = fileName;
                        companionAnimal.PicturePath = UploadPath.CompanionAnimalPath;
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberNo":
                                companionAnimal.MemberNo = int.Parse(item);
                                break;
                            case "PetCategory":
                                companionAnimal.PetCategory = item;
                                break;
                            case "PetCode":
                                companionAnimal.PetCode = item;
                                break;
                            case "Name":
                                companionAnimal.Name = item;
                                break;
                            case "Age":
                                companionAnimal.Age = byte.Parse(item);
                                break;
                            case "Weight":
                                companionAnimal.Weight = byte.Parse(item);
                                break;
                            case "Gender":
                                companionAnimal.Gender = item;
                                break;
                            case "Marking":
                                companionAnimal.Marking = item;
                                break;
                            case "Medication":
                                companionAnimal.Medication = item;
                                break;
                            case "Feature":
                                companionAnimal.Feature = item;
                                break;
                            case "PictureName":
                                companionAnimal.PictureName = item;
                                break;
                            case "PicturePath":
                                companionAnimal.PicturePath = item;
                                break;
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                companionAnimal.StateFlag = StateFlag.Use;
                companionAnimal.DateCreated = DateTime.Now;
                companionAnimal.DateModified = DateTime.Now;
                companionAnimal.DateDeleted = DateTime.Now;
                db.CompanionAnimals.Add(companionAnimal);
                int num = await this.db.SaveChangesAsync();

                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = companionAnimal;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        // DELETE: api/CompanionAnimals/5
        [ResponseType(typeof(CompanionAnimal))]
        public async Task<IHttpActionResult> DeleteCompanionAnimal(int id)
        {
            CompanionAnimal companionAnimal = await db.CompanionAnimals.FindAsync(id);
            if (companionAnimal == null)
            {
                return NotFound();
            }

            db.CompanionAnimals.Remove(companionAnimal);
            await db.SaveChangesAsync();

            return Ok(companionAnimal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanionAnimalExists(int id)
        {
            return db.CompanionAnimals.Count(e => e.CompanionAnimalNo == id) > 0;
        }
    }
}