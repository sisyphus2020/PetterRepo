using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class Company : Classification
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CompanyNo { get; set; }
        [Index("IDX_COMPANY_COMPANYNAME"), MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(200)]
        public string CompanyAddr { get; set; }
        //public string PictureName { get; set; }
        //public string PicturePath { get; set; }
        //public string StartHours { get; set; }
        //public string EndHours { get; set; }
        public string Holiday { get; set; }
        //public string Introduction { get; set; }
        //public DbGeography Coordinate { get; set; }
        //[Range(0, 10.00)]
        //public decimal Grade { get; set; }
        //public int ReviewCount { get; set; }
        //public int Bookmark { get; set; }
    }
}