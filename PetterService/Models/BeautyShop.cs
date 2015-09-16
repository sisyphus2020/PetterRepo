using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BeautyShop : Classification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopNo { get; set; }
        [Index("IDX_BEAUTYSHOP_BEAUTYSHOPNAME"), MaxLength(100)]
        public string BeautyShopName { get; set; }
        [MaxLength(200)]
        public string BeautyShopAddr { get; set; }
        
        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<BeautyShopStats> BeautyShopStats { get; set; }
        public ICollection<BeautyShopService> BeautyShopServices { get; set; }
        public ICollection<BeautyShopHoliday> BeautyShopHolidays { get; set; }
    }
}