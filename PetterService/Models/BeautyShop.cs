using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class BeautyShop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeautyShopNo { get; set; }
        public int CompanyNo { get; set; }
        public string BeautyShopName { get; set; }
        public string BeautyShopAddr { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public string StartBeautyShopHours { get; set; }
        public string EndBeautyShopHours { get; set; }
        public string Introduction { get; set; }
        public DbGeography Coordinate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Range(0, 10)]
        public Decimal Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        
        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<BeautyShopService> BeautyShopServices { get; set; }
        public ICollection<BeautyShopHoliday> BeautyShopHolidays { get; set; }
    }
}