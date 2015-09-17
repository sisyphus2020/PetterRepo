using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BeautyShopReview : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopReviewNo { get; set; }
        public int BeautyShopNo { get; set; }
        public int MemberNo { get; set; }
        //[Index("IDX_GRADE"), Range(0, 10)]
        public double Grade { get; set; }
        public string Content { get; set; }
        [MaxLength(1), Column("StateFlag", TypeName = "char")]
        public string StateFlag { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
        //[ForeignKey("BeautyShopNo")]
        //public BeautyShop BeautyShop { get; set; }
        //public ICollection<BeautyShopReviewFile> BeautyShopReviewFiles { get; set; }
        //public ICollection<BeautyShopReviewLike> BeautyShopReviewLikes { get; set; }
    }
}