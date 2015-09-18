using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreStatsNo { get; set; }
        public int StoreNo { get; set; }
        [Range(0, 10)]
        public double Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }

        // Navigation property
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }
    }
}