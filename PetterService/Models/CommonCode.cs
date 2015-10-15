using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class CommonCode : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodeNo { get; set; }
        [Index("IX_COMMONCODE_CODEID", IsUnique = true), MaxLength(6), Column("CodeID", TypeName = "char")]
        public string CodeID { get; set; }
        [Index("IX_COMMONCODE_PARENTCODEID"), MaxLength(3), Column("ParentCodeID", TypeName = "char")]
        public string ParentCodeID { get; set; }
        public int OrderNo { get; set; }
        [MaxLength(50)]
        public string CodeName { get; set; }
        [MaxLength(100)]
        public string CodeDescription { get; set; }


        // Navigation property
        //[ForeignKey("ParentCodeID")]
        //public CommonCode CommonCode { get; set; }

    }
}
