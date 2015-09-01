using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class PensionService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PensionServiceNo { get; set; }
        public int PensionNo { get; set; }
        public int PensionServiceCode { get; set; }

        [ForeignKey("PensionNo")]
        public Pension Pension { get; set; }
    }
}