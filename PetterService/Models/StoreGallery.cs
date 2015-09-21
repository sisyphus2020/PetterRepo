using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreGallery : FileDateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreGalleryNo { get; set; }
        public int StoreNo { get; set; }
        [MaxLength(200)]
        public string Content { get; set; }

        // Navigation property
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }
    }
}