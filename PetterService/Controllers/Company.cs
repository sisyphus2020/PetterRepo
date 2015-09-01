using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class Company
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CompanyNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddr { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public string StartShopHours { get; set; }
        public string EndShopHours { get; set; }
        public string Holiday { get; set; }
        public string Introduction { get; set; }
        public DbGeography Geography { get; set; }
        [Range(0, 10.00)]
        public decimal Grade { get; set; }
        public int ReviewCount { get; set; }
        public int BookMark { get; set; }
    }
}