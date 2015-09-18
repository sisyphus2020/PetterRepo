using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreBase : GeographyFileDateBase
    {
        public virtual int CompanyNo { get; set; }
        [Index("IX_STORE_STORENAME"), MaxLength(100)]
        public string StoreName { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(200)]
        public string StoreAddress { get; set; }
        [Column("StartTime", TypeName = "char"), MaxLength(4)]
        public string StartTime { get; set; }
        [Column("EndTime", TypeName = "char"), MaxLength(4)]
        public string EndTime { get; set; }
        [MaxLength(200)]
        public string Introduction { get; set; }
        [MaxLength(20)]
        public string WriteIP { get; set; }
        [MaxLength(20)]
        public string ModifyIP { get; set; }
    }
}