using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreGalleryFile : FileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreGalleryFileNo { get; set; }
        public int StoreGalleryNo { get; set; }

        // Navigation property
        [ForeignKey("StoreGalleryNo")]
        public StoreGallery StoreGallery { get; set; }
    }
}