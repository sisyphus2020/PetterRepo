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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PetKindNo { get; set; }
        //[Column("PetCategory", Order = 0, TypeName = "char"), MaxLength(3), Key]
        [MaxLength(3), Column("PetCategory", TypeName = "char")]
        public string PetCategory { get; set; }
        //[Column("PetCode", Order = 1, TypeName = "char"), MaxLength(4), Key]
        [MaxLength(4), Column("PetCode", TypeName = "char")]
        public string PetCode { get; set; }
        [MaxLength(50)]
        public string PetName { get; set; }
    }
}