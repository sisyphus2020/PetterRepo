using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class Store : StoreBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreNo { get; set; }
        
        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<StoreStats> StoreStats { get; set; }
        public ICollection<StoreService> StoreServices { get; set; }
        public ICollection<StoreHoliday> StoreHolidays { get; set; }
    }
}