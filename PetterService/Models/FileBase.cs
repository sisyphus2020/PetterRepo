using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class FileBase
    {
        [MaxLength(100)]
        public string PictureName { get; set; }
        [MaxLength(100)]
        public string PicturePath { get; set; }
    }
}