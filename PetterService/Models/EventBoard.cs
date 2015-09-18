using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class EventBoard : BoardBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EventBoardNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
        public ICollection<EventBoardStats> EventBoardStats { get; set; }
        public ICollection<EventBoardFile> EventBoardFiles { get; set; }
        public ICollection<EventBoardReply> EventBoardReplys { get; set; }
    }
}