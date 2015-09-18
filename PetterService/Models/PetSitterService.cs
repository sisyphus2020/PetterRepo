using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetSitterService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetSitterServiceNo { get; set; }
        public int PetSitterNo { get; set; }
        public int PetSitterServiceCode { get; set; }

        [ForeignKey("PetSitterNo")]
        public PetSitter PetSitter { get; set; }
    }
}