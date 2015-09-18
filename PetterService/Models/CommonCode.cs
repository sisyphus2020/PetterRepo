using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class CommonCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoommonCodeNo { get; set; }
        //[Key, Column(Order = 0), MaxLength(20)]
        [MaxLength(20)]
        public string Category { get; set; }
        //[Key, Column(Order = 1, TypeName = "char"), MaxLength(4)]
        [MaxLength(4), Column("Code", TypeName = "char")]
        public string Code { get; set; }
        [MaxLength(50)]
        public string CodeName { get; set; }
    }
}