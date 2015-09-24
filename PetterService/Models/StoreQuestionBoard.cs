using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreQuestionBoard : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreQuestionBoardNo { get; set; }
        [ForeignKey("StoreQuestion"), Column(Order = 0)]
        public int StoreNo { get; set; }
        [ForeignKey("StoreQuestion"), Column(Order = 1)]
        public int MemberNo { get; set; }
        public int? StaffMemberNo { get; set; }
        public int? ParentBoardNo { get; set; }
        public int? Thread { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }

        // Navigation property
        //[ForeignKey("MemberNo")]
        public virtual StoreQuestion StoreQuestion { get; set; }
        //[ForeignKey("StoreNo")]
        //public virtual StoreQuestion StoreQuestion2 { get; set; }
    }
}