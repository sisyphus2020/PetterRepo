using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BeautyShopService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopServiceNo { get; set; }
        public int BeautyShopNo { get; set; }
        public int BeautyShopServiceCode { get; set; }

        // Navigation property
        [ForeignKey("BeautyShopNo")]
        public BeautyShop BeautyShop { get; set; }
    }
}