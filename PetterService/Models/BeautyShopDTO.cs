using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BeautyShopDTO : Classification
    {
        public int BeautyShopNo { get; set; }
        public string BeautyShopName { get; set; }
        public string BeautyShopAddr { get; set; }
        
        public List<BeautyShopService> BeautyShopServices { get; set; }
        public List<BeautyShopHoliday> BeautyShopHolidays { get; set; }
    }
}