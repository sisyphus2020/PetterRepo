using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using static PetterService.Common.EnumHelper;

namespace PetterService.Common
{
    public class Uploads
    {
        UploadType uploadType { get; set; } //Upload File extension Type[Default All]
        String savePath { get; set; } //Save the file path [Default WebRoot]
        String newName { get; set; } //Save As [Default Orignal FileName]
        String newTypeName { get; set; }

        public Uploads() : this(UploadPath.Temp, null, UploadType.All) { }

        public Uploads(String savePath) : this(savePath, null, UploadType.All) { }

        public Uploads(UploadType uploadType) : this(UploadPath.Temp, null, uploadType) { }

        public Uploads(String savePath, String newName) : this(savePath, newName, UploadType.All) { }

        public Uploads(String savePath, String newName, UploadType uploadType)
        {
            this.uploadType = uploadType;
            this.savePath = savePath;
            this.newName = newName;
        }


        //Parameter : Web FileUpload Control 
        public string Save(FileUpload fileUpload)
        {
            string result = null;
            if (fileUpload.Equals(null)) return result;

            string filename = fileUpload.FileName;
            if (filename.Equals("")) return result;

            if (!(new Validate(filename, uploadType).Checked())) return result;

            string fileExtension = System.IO.Path.GetExtension(filename);
            if (newName.Equals(null) || newName.Trim().Length == 0) newName = System.IO.Path.GetFileNameWithoutExtension(filename);

            result = newName + fileExtension;


            fileUpload.SaveAs(savePath + result);


            return result;
        }

        //Parameter : Web FileUpload Control 
        public string DefaultNameSave(FileUpload fileUpload)
        {
            string result = null;
            if (fileUpload.Equals(null)) return result;

            string filename = fileUpload.FileName;
            if (filename.Equals("")) return result;

            if (!(new Validate(filename, uploadType).Checked())) return result;

            string fileExtension = System.IO.Path.GetExtension(filename);
            result = newName + "_" + System.IO.Path.GetFileNameWithoutExtension(filename) + fileExtension;

            fileUpload.SaveAs(savePath + result);


            return result;
        }

        //File name change(Stored on the server)
        //Parameter(Target FileName(Ex. test.jpg), Sava As FileName(Ex. NewTest)
        public string ReName(string oldFileName, string newName)
        {
            string newFileName = null;

            if (newName != null && newName.Trim().Length > 0 && System.IO.File.Exists(savePath + oldFileName))
            {
                newFileName = newName + System.IO.Path.GetExtension(oldFileName);

                System.IO.File.Move(savePath + oldFileName, savePath + newFileName);
            }

            return newFileName;
        }

        public string ReName(string oldFileName)
        {
            return ReName(oldFileName, this.newName);
        }


        public string UpdateSave(string oldFileName, FileUpload fileUpload)
        {
            string result = null;
            if (fileUpload.Equals(null)) return result;

            string filename = fileUpload.FileName;
            if (filename.Equals("")) return result;

            if (!(new Validate(filename, uploadType).Checked())) return result;

            string fileExtension = System.IO.Path.GetExtension(filename);
            if (newName.Equals(null) || newName.Trim().Length == 0) newName = System.IO.Path.GetFileNameWithoutExtension(filename);

            result = newName + fileExtension;

            if (newName != null && newName.Trim().Length > 0 && System.IO.File.Exists(savePath + oldFileName))
            {
                System.IO.File.Delete(savePath + oldFileName);
                fileUpload.SaveAs(savePath + result);
            }

            return result;
        }

        public string DefaultNameUpdate(string oldFileName, FileUpload fileUpload)
        {
            string result = null;
            if (fileUpload.Equals(null)) return result;

            string filename = fileUpload.FileName;
            if (filename.Equals("")) return result;

            if (!(new Validate(filename, uploadType).Checked())) return result;

            string fileExtension = System.IO.Path.GetExtension(filename);
            if (newName != null && newName.Trim().Length > 0 && System.IO.File.Exists(savePath + oldFileName))
            {
                result = newName + "_" + System.IO.Path.GetFileNameWithoutExtension(filename) + fileExtension;
                System.IO.File.Delete(savePath + oldFileName);
                fileUpload.SaveAs(savePath + result);
            }

            return result;
        }

        /// <summary>
        /// Upload Files Extension Type check
        /// </summary>
        class Validate
        {
            UploadType uploadType { get; set; }
            String fileName { get; set; }

            public Validate() : this(null, UploadType.All) { }

            public Validate(String fileName) : this(fileName, UploadType.All) { }

            public Validate(String fileName, UploadType uploadType)
            {
                this.uploadType = uploadType;
                this.fileName = fileName;
            }

            public Boolean Checked()
            {
                if (UploadType.All.Equals(uploadType)) return true;

                Regex Pattern = new Regex(EnumHelper.toDescription(uploadType));
                return Pattern.IsMatch(fileName);

            }


        }

    }
}