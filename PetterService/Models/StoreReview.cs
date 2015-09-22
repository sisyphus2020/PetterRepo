using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreReview : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreReviewNo { get; set; }
        public int StoreNo { get; set; }
        public int MemberNo { get; set; }
        public double Grade { get; set; }
        public string Content { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }
        public ICollection<StoreReviewFile> StoreReviewFiles { get; set; }
        public ICollection<StoreReviewLike> StoreReviewLikes { get; set; }
        public ICollection<StoreReviewStats> StoreReviewStats { get; set; }
    }
}