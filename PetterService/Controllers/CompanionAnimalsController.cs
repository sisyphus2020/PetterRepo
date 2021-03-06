﻿using System;
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

        /// <summary>
        /// GET: api/CompanionAnimals
        /// 반려동물 등록 리스트
        /// </summary>
        /// <param name="companionAnimal"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<CompanionAnimal>))]
        public async Task<IHttpActionResult> GetCompanionAnimals([FromUri] CompanionAnimal companionAnimal)
        {
            PetterResultType<CompanionAnimal> petterResultType = new PetterResultType<CompanionAnimal>();
            var list = await db.CompanionAnimals.Where(p => p.MemberID == companionAnimal.MemberID).ToListAsync();

            if (list == null)
            {
                return NotFound();
            }

            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = list;
            return Ok(petterResultType);
        }

        ///// <summary>
        ///// GET: api/CompanionAnimals/5
        ///// 반려동물 상세 정보
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        [ResponseType(typeof(PetterResultType<CompanionAnimal>))]
        public async Task<IHttpActionResult> GetCompanionAnimal(int id)
        {
            PetterResultType<CompanionAnimal> petterResultType = new PetterResultType<CompanionAnimal>();
            List<CompanionAnimal> cmpanionAnimals = new List<CompanionAnimal>();

            var companionAnimal = await db.CompanionAnimals.Where(p => p.CompanionAnimalNo == id).SingleOrDefaultAsync();

            if (companionAnimal == null)
            {
                return NotFound();
            }

            cmpanionAnimals.Add(companionAnimal);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = cmpanionAnimals;

            return Ok(petterResultType);
        }

        /// <summary>
        /// PUT: api/CompanionAnimals/5
        /// 반려동물 수정
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<CompanionAnimal>))]
        public async Task<IHttpActionResult> PutCompanionAnimal(int id)
        {
            PetterResultType<CompanionAnimal> petterResultType = new PetterResultType<CompanionAnimal>();
            List<CompanionAnimal> companionAnimals = new List<CompanionAnimal>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            CompanionAnimal companionAnimal = await db.CompanionAnimals.FindAsync(id);
            if (companionAnimal == null)
            {
                return NotFound();
            }

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
                        string oldFileName = HttpUtility.UrlDecode(content.Headers.ContentDisposition.FileName.Trim('"'));
                        string fileName = Utilities.additionFileName(oldFileName);

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

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreWidth, FileSize.StoreHeight, ImageFormat.Png);
                        companionAnimal.FileName = fileName;
                        companionAnimal.FilePath = UploadPath.CompanionAnimalPath.Replace("~", "");
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            //case "CompanionAnimalNo":
                            //    companionAnimal.CompanionAnimalNo = int.Parse(item);
                            //    break;
                            //case "MemberID":
                            //    companionAnimal.MemberID = item;
                            //    break;
                            case "CodeID":
                                companionAnimal.CodeID = item;
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
                            case "Neutralization":
                                companionAnimal.Neutralization = item;
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
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                companionAnimal.StateFlag = StateFlags.Use;
                companionAnimal.DateModified = DateTime.Now;
                db.Entry(companionAnimal).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                companionAnimals.Add(companionAnimal);
                petterResultType.AffectedRow = companionAnimals.Count();
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = companionAnimals;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// POST: api/CompanionAnimals
        /// 반려동물 등록
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PetterResultType<CompanionAnimal>))]
        public async Task<IHttpActionResult> PostCompanionAnimal()
        {
            PetterResultType<CompanionAnimal> petterResultType = new PetterResultType<CompanionAnimal>();
            List<CompanionAnimal> companionAnimals = new List<CompanionAnimal>();
            CompanionAnimal companionAnimal = new CompanionAnimal();

            if (Request.Content.IsMimeMultipartContent())
            {
                string folder = HostingEnvironment.MapPath(UploadPath.CompanionAnimalPath);
                Utilities.CreateDirectory(folder);

                var provider = await Request.Content.ReadAsMultipartAsync();

                //var provider = new MultipartMemoryStreamProvider();
                //await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var content in provider.Contents)
                {
                    string fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        var file = await content.ReadAsByteArrayAsync();
                        string oldFileName = HttpUtility.UrlDecode(content.Headers.ContentDisposition.FileName.Trim('"'));

                        string fileName = Utilities.additionFileName(oldFileName);

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

                        Utilities.ResizeImage(fullPath, thumbnamil, FileSize.StoreWidth, FileSize.StoreHeight, ImageFormat.Png);
                        companionAnimal.FileName = fileName;
                        companionAnimal.FilePath = UploadPath.CompanionAnimalPath.Replace("~", "");
                    }
                    else
                    {
                        string str = await content.ReadAsStringAsync();
                        string item = HttpUtility.UrlDecode(str);

                        #region switch case
                        switch (fieldName)
                        {
                            case "MemberID":
                                companionAnimal.MemberID = item;
                                break;
                            case "CodeID":
                                companionAnimal.CodeID = item;
                                break;
                            //case "PetCategory":
                            //    companionAnimal.PetCategory = item;
                            //    break;
                            //case "PetCode":
                            //    companionAnimal.PetCode = item;
                            //    break;
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
                            case "Neutralization":
                                companionAnimal.Neutralization = item;
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
                            default:
                                break;
                        }
                        #endregion switch case
                    }
                }

                companionAnimal.StateFlag = StateFlags.Use;
                companionAnimal.DateCreated = DateTime.Now;
                companionAnimal.DateModified = DateTime.Now;
                db.CompanionAnimals.Add(companionAnimal);
                int num = await this.db.SaveChangesAsync();

                companionAnimals.Add(companionAnimal);
                petterResultType.AffectedRow = companionAnimals.Count();
                petterResultType.IsSuccessful = true;
                petterResultType.JsonDataSet = companionAnimals;
            }
            else
            {
                petterResultType.IsSuccessful = false;
                petterResultType.JsonDataSet = null;
            }

            return Ok(petterResultType);
        }

        /// <summary>
        /// DELETE: api/CompanionAnimals/5
        /// 반려동물 삭제처리(상태플래그 삭제(D)로 변경)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(CompanionAnimal))]
        public async Task<IHttpActionResult> DeleteCompanionAnimal(int id)
        {
            // 삭제 권한 처리 필요

            PetterResultType<CompanionAnimal> petterResultType = new PetterResultType<CompanionAnimal>();
            List<CompanionAnimal> companionAnimals = new List<CompanionAnimal>();
            CompanionAnimal companionAnimal = await db.CompanionAnimals.FindAsync(id);

            if (companionAnimal == null)
            {
                return NotFound();
            }

            companionAnimal.StateFlag = StateFlags.Delete;
            companionAnimal.DateDeleted = DateTime.Now;
            db.Entry(companionAnimal).State = EntityState.Modified;

            await db.SaveChangesAsync();

            companionAnimals.Add(companionAnimal);
            petterResultType.IsSuccessful = true;
            petterResultType.JsonDataSet = companionAnimals;

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

        private bool CompanionAnimalExists(int id)
        {
            return db.CompanionAnimals.Count(e => e.CompanionAnimalNo == id) > 0;
        }
    }
}