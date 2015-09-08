using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PetterService.Models
{
    public class Company : Classification
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override int CompanyNo { get; set; }
        [Index("IDX_COMPANY_COMPANYNAME"), MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(200)]
        public string CompanyAddr { get; set; }
    }
}