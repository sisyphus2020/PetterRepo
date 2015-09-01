using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class PensionHoliday
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PensionHolidayNo { get; set; }
        public int PensionNo { get; set; }
        public int PensionHolidayCode { get; set; }

        [ForeignKey("PensionNo")]
        public Pension Pension { get; set; }
    }
}