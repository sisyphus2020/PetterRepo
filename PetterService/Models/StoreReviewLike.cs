using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreReviewLike : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreReviewLikeNo { get; set; }
        public int StoreReviewNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        [ForeignKey("StoreReviewNo")]
        public StoreReview StoreReview { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}