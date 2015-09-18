using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class EventBoardReply : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EventBoardReplyNo { get; set; }
        public int EventBoardNo { get; set; }
        public int MemberNo { get; set; }
        public string Reply { get; set; }
        //[MaxLength(1), Column("StateFlag", TypeName = "char")]
        //public string StateFlag { get; set; }

        // Navigation property
        // 순환참조 오류 발생
        //[ForeignKey("EventBoardNo")]
        //public EventBoard EventBoard { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}