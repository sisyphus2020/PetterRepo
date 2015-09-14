using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class FileDateBase : DateBase
    {
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength(100)]
        public string FilePath { get; set; }
    }
}