using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetterOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetterOptionNo { get; set; }
        public string PetterOptionCategory { get; set; }
        public int PetterOptionCode { get; set; }
        public string PetterOptionName { get; set; }
    }
}