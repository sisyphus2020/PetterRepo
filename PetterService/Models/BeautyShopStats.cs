using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BeautyShopStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopStatsNo { get; set; }
        public int BeautyShopNo { get; set; }
        [Range(0, 10)]
        public double Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }

        // Navigation property
        [ForeignKey("BeautyShopNo")]
        public BeautyShop BeautyShop { get; set; }
    }
}