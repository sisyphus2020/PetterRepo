using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreGalleryReply : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreGalleryReplyNo { get; set; }
        public int StoreGalleryNo { get; set; }
        public int MemberNo { get; set; }
        [MaxLength(200)]
        public string Reply { get; set; }

        // Navigation property
        [ForeignKey("StoreGalleryNo")]
        public StoreGallery StoreGallery { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}