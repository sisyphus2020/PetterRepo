using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BoardStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardStatsNo { get; set; }
        public int BoardNo { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }

        // Navigation property
        [ForeignKey("BoardNo")]
        public Board Board { get; set; }
    }
}