using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreReviewStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreReviewStatsNo { get; set; }
        public int StoreReviewNo { get; set; }
        public int LikeCount { get; set; }

        // Navigation property
        [ForeignKey("StoreReviewNo")]
        public StoreReview StoreReview { get; set; }
    }
}