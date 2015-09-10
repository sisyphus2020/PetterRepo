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
        [Key]
        [Column("PetCategory", Order = 0, TypeName = "char"), MaxLength(3)]
        public string PetCategory { get; set; }
        [Key]
        [Column("PetCode", Order = 1, TypeName = "char"), MaxLength(4)]
        public string PetCode { get; set; }

        [MaxLength(50)]
        public string PetName { get; set; }
    }
}