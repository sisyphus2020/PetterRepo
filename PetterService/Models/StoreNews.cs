using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetterService.Models
{
    public class StoreNews : FileDateBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreNewsNo { get; set; }
        public int StoreNo { get; set; }
        [MaxLength(4000)]
        public string Content { get; set; }

        // Navigation property
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }
        public ICollection<StoreNewsStats> StoreNewsStats { get; set; }
        public ICollection<StoreNewsFile> StoreNewsFiles { get; set; }
        public ICollection<StoreNewsLike> StoreNewsLikes { get; set; }
        public ICollection<StoreNewsReply> StoreNewsReplies { get; set; }
    }
}