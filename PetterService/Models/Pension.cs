using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class Pension
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PensionNo { get; set; }
        public int CompanyNo { get; set; }
        public string PensionName { get; set; }
        public string PensionAddr { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public string StartPension { get; set; }
        public string EndPension { get; set; }
        public string Introduction { get; set; }
        public DbGeography Coordinate { get; set; }
        [Range(0, 10)]
        public decimal Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<PensionService> PensionServices { get; set; }
        public ICollection<PensionHoliday> PensionHolidays { get; set; }
    }
}