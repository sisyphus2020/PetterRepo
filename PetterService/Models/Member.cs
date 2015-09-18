using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class Member : GeographyFileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MemberNo { get; set; }
        [Index("IDX_MEMBER_MEMBERID",  IsUnique = true), MaxLength(50)]
        public string MemberID { get; set; }
        [MaxLength(200)]
        public string Password { get; set; }
        [Index("IDX_MEMBER_NICKNAME", IsUnique = true), MaxLength(50)]
        public string NickName { get; set; }
        //[MaxLength(1), Column("StateFlag", TypeName = "char")]
        //public string StateFlag { get; set; }
        [MaxLength(1), Column("Route", TypeName = "char")]
        public string Route { get; set; }
    }
}