using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PensionDTO : Classification
    {
        public int PensionNo { get; set; }
        public string PensionName { get; set; }
        public string PensionAddr { get; set; }
        
        public List<PensionService> PensionServices { get; set; }
        public List<PensionHoliday> PensionHolidays { get; set; }
    }
}