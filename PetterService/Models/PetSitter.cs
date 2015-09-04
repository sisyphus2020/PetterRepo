using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetSitter : Classification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetSitterNo { get; set; }
        [MaxLength(100)]
        public string PetSitterName { get; set; }
        [MaxLength(200)]
        public string PetSitterAddr { get; set; }

        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<PetSitterService> PetSitterServices { get; set; }
        public ICollection<PetSitterHoliday> PetSitterHolidays { get; set; }
    }
}