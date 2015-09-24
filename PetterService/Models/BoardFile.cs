using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class BoardFile : FileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BoardFileNo { get; set; }
        public int BoardNo { get; set; }

        // Navigation property
        [ForeignKey("BoardNo")]
        public Board Board { get; set; }
    }
}