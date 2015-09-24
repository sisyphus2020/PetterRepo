using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BoardLike : DateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BoardLikeNo { get; set; }
        public int BoardNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        [ForeignKey("BoardNo")]
        public Board Board { get; set; }
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
    }
}