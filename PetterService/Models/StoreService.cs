using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreServiceNo { get; set; }
        public int StoreNo { get; set; }
        [Index("IX_STORE_CODEID"), MaxLength(6), Column("CodeID", TypeName = "char")]
        public string CodeID { get; set; }

        // Navigation property
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }
    }
}