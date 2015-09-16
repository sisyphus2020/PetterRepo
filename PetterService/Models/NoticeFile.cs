using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class NoticeFile : FileBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NoticeFileNo { get; set; }
        public int NoticeNo { get; set; }

        // Navigation property
        [ForeignKey("NoticeNo")]
        public Notice Notice { get; set; }
    }
}