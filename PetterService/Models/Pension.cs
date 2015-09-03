using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class Pension : Classification
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PensionNo { get; set; }
        public string PensionName { get; set; }
        public string PensionAddr { get; set; }

        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<PensionService> PensionServices { get; set; }
        public ICollection<PensionHoliday> PensionHolidays { get; set; }
    }
}