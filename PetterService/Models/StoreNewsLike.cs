using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreNewsLike : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreNewsLikeNo { get; set; }
        public int StoreNewsNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        [ForeignKey("StoreNewsNo")]
        public StoreNews StoreNews { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}