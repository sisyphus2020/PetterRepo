using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class CommonCode
    {
        [Key, Column(Order = 0), MaxLength(20)]
        public string Category { get; set; }
        [Key, Column(Order = 1, TypeName = "char"), MaxLength(4)]
        public string Code { get; set; }
        [MaxLength(50)]
        public string CodeName { get; set; }
    }
}