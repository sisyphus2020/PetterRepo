using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BeautyShopBookmark : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopBookmarkNo { get; set; }
        public int BeautyShopNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        [ForeignKey("BeautyShopNo")]
        public BeautyShop BeautyShop { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}