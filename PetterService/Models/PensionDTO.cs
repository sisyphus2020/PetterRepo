using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PensionDTO
    {
        public int PensionNo { get; set; }
        public int CompanyNo { get; set; }
        public string PensionName { get; set; }
        public string PensionAddr { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public string StartPensionHours { get; set; }
        public string EndPensionHours { get; set; }
        public string Introduction { get; set; }
        public DbGeography Coordinate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Decimal Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public List<PensionService> PensionServices { get; set; }
        public List<PensionHoliday> PensionHolidays { get; set; }
    }
}