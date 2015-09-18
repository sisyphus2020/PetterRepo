using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreReviewFile : FileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreReviewFileNo { get; set; }
        public int StoreReviewNo { get; set; }

        // Navigation property
        [ForeignKey("StoreReviewNo")]
        public StoreReview StoreReview { get; set; }
    }
}