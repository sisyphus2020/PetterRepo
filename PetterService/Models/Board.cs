using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class Board : FileDateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardNo { get; set; }
        public int StoreNo { get; set; }
        [Index("IX_STORE_CODEID"), MaxLength(6), Column("CodeID", TypeName = "char")]
        public string CodeID { get; set; }
        [MaxLength(4000)]
        public string Content { get; set; }

        // Navigation property
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }
        public ICollection<BoardStats> BoardStats { get; set; }
        public ICollection<BoardFile> BoardFiles { get; set; }
        public ICollection<BoardLike> BoardLikes { get; set; }
        public ICollection<BoardReply> BoardReplies { get; set; }
    }
}