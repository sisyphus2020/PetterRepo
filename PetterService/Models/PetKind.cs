using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetKind
    {
        [Key]
        [Column(Order = 0), MaxLength(20)]
        public string PetCategory { get; set; }
        [Key]
        [Column(Order = 1), MaxLength(4)]
        public string PetCode { get; set; }

        [MaxLength(50)]
        public string PetName { get; set; }
    }
}