using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetInfomation
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PetInfomationNo { get; set; }
        public int MemberNo { get; set; }
        [ForeignKey("PetKind")]
        [Column(Order = 0)]
        public string PetCategory { get; set; }
        [ForeignKey("PetKind")]
        [Column(Order = 1)]
        public string PetCode { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        [MaxLength(1)]
        public string Gender { get; set; }
        [MaxLength(1)]
        public string Marking { get; set; }
        [MaxLength(1)]
        public string Medication { get; set; }
        [MaxLength(200)]
        public string Feature { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }

        //[ForeignKey("PetKind")]
        public PetKind PetKind { get; set; }
    }
}