using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetKind
    {
        [Column("PetCategory", Order = 0, TypeName = "char"), MaxLength(3), Key]
        public string PetCategory { get; set; }
        [Column("PetCode", Order = 1, TypeName = "char"), MaxLength(4), Key]
        public string PetCode { get; set; }
        [MaxLength(50)]
        public string PetName { get; set; }
    }
}