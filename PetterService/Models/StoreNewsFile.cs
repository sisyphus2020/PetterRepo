using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreNewsFile : FileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StoreNewsFileNo { get; set; }
        public int StoreNewsNo { get; set; }

        // Navigation property
        [ForeignKey("StoreNewsNo")]
        public StoreNews StoreNews { get; set; }
    }
}