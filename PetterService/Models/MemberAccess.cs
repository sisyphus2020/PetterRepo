using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class MemberAccess
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MemberAccessNo { get; set; }
        [Index("IDX_MEMBERACCESS_MEMBERID"), MaxLength(50)]
        public string MemberID { get; set; }
        [MaxLength(1)]
        public string AccessResult { get; set; }
        public DateTime DateCreated { get; set; }

        // Navigation property
        //[ForeignKey("MemberNo")]
        //public Member Member { get; set; }
    }
}