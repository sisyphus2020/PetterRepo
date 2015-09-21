using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreGalleryStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreGalleryStatsNo { get; set; }
        public int StoreGalleryNo { get; set; }
        public int LikeCount { get; set; }

        // Navigation property
        [ForeignKey("StoreGalleryNo")]
        public StoreGallery StoreGallery { get; set; }
    }
}