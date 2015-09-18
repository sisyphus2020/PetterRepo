using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class Store : Classification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreNo { get; set; }
        [Index("IDX_BEAUTYSHOP_StoreName"), MaxLength(100)]
        public string StoreName { get; set; }
        [MaxLength(200)]
        public string StoreAddress { get; set; }
        
        // Navigation property
        [ForeignKey("CompanyNo")]
        public Company Company { get; set; }
        public ICollection<StoreStats> StoreStats { get; set; }
        public ICollection<StoreService> StoreServices { get; set; }
        public ICollection<StoreHoliday> StoreHolidays { get; set; }
    }
}