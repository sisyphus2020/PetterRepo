using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class NoticeReply : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NoticeReplyNo { get; set; }
        public int NoticeNo { get; set; }
        public int MemberNo { get; set; }
        public string Reply { get; set; }
        //[MaxLength(1), Column("StateFlag", TypeName = "char")]
        //public string StateFlag { get; set; }

        // Navigation property
        // 순환참조 오류 발생
        //[ForeignKey("NoticeNo")]
        //public Notice Notice { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}