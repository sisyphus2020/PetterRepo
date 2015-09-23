using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreNewsStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreNewsStatsNo { get; set; }
        public int StoreNewsNo { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }

        // Navigation property
        [ForeignKey("StoreNewsNo")]
        public StoreNews StoreNews { get; set; }
    }
}