using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class EventBoardReply : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EventBoardReplyNo { get; set; }
        public int MemberNo { get; set; }
        public string Reply { get; set; }
        [MaxLength(1), Column("StateFlag", TypeName = "char")]
        public string StateFlag { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}