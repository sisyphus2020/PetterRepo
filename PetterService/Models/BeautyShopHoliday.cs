using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BeautyShopHoliday
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopHolidayNo { get; set; }
        public int BeautyShopNo { get; set; }
        public int BeautyShopHolidayCode { get; set; }

        // Navigation property
        [ForeignKey("BeautyShopNo")]
        public BeautyShop BeautyShop { get; set; }
    }
}