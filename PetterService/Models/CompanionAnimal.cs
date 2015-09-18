using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class CompanionAnimal : FileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CompanionAnimalNo { get; set; }
        public int MemberNo { get; set; }
        public int PetKindNo { get; set; }
        //[ForeignKey("PetKind")]
        //[Column(Order = 0)]
        //public string PetCategory { get; set; }
        //[ForeignKey("PetKind")]
        //[Column(Order = 1)]
        //public string PetCode { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [Column("Age", TypeName = "tinyint")]
        public byte Age { get; set; }
        [Column("Weight", TypeName = "tinyint")]
        public byte Weight { get; set; }
        [MaxLength(1), Column("Gender", TypeName = "char")]
        public string Gender { get; set; }
        [MaxLength(1), Column("Neutralization", TypeName = "char")]
        public string Neutralization { get; set; }
        [MaxLength(1), Column("Marking", TypeName = "char")]
        public string Marking { get; set; }
        [MaxLength(1), Column("Medication", TypeName = "char")]
        public string Medication { get; set; }
        [MaxLength(200)]
        public string Feature { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }

        //[ForeignKey("PetKindNo")]
        //public PetKind PetKind { get; set; }
    }
}