using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class Notice : BoardBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NoticeNo { get; set; }
        public int MemberNo { get; set; }
        //[Index("IDX_REVIEWCOUNT")]
        //public int ReviewCount { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
        public ICollection<NoticeFile> NoticeFiles { get; set; }
        public ICollection<NoticeReply> NoticeReplies { get; set; }
    }
}