using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BeautyShopDTO
    {
        public int BeautyShopNo { get; set; }
        public int CompanyNo { get; set; }
        public string BeautyShopName { get; set; }
        public string BeautyShopAddr { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public string StartBeautyShop { get; set; }
        public string EndBeautyShop { get; set; }
        public string Introduction { get; set; }
        public DbGeography Coordinate { get; set; }
        public Decimal Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public List<BeautyShopService> BeautyShopServices { get; set; }
        public List<BeautyShopHoliday> BeautyShopHolidays { get; set; }
    }
}