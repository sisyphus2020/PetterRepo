using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class NoticeStats : DateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoticeStatsNo { get; set; }
        public int NoticeNo { get; set; }
        public int ReplyCount { get; set; }

        // Navigation property
        [ForeignKey("NoticeNo")]
        public Notice Notice { get; set; }
    }
}