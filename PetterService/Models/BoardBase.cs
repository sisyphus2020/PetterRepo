using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BoardBase : DateBase
    {
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(4000)]
        public string Content { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength(100)]
        public string FilePath { get; set; }
        [MaxLength(1), Column("StateFlag", TypeName = "char")]
        public string StateFlag { get; set; }
    }
}