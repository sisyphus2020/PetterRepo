using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BeautyShopReviewFile : FileBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BeautyShopReviewFileNo { get; set; }
        public int BeautyShopReviewNo { get; set; }

        // Navigation property
        [ForeignKey("BeautyShopReviewNo")]
        public BeautyShopReview BeautyShopReview { get; set; }
    }
}