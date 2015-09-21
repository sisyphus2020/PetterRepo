using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreDTO : StoreBase
    {
        public int StoreNo { get; set; }
        //public string StoreName { get; set; }
        //public string StoreAddress { get; set; }

        public List<StoreStats> StoreStats { get; set; }
        public List<StoreService> StoreServices { get; set; }
        public List<StoreHoliday> StoreHolidays { get; set; }
    }
}