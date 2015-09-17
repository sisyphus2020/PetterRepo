using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class EventBoardStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventBoardStatsNo { get; set; }
        public int EventBoardNo { get; set; }
        public int ReplyCount { get; set; }

        // Navigation property
        [ForeignKey("EventBoardNo")]
        public EventBoard EventBoard { get; set; }
    }
}