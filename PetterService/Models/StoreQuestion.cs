using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreQuestion : DateBase
    {
        [Column("StoreNo", Order = 0), Key]
        public int StoreNo { get; set; }
        [Column("MemberNo", Order = 1), Key]
        public int MemberNo { get; set; }
        [MaxLength(100)]
        public string StoreName { get; set; }

        // Navigation property
        [ForeignKey("MemberNo")]
        public Member Member { get; set; }
        [ForeignKey("StoreNo")]
        public Store Store { get; set; }

    }
}