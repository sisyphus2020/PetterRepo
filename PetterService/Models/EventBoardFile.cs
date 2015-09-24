using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class EventBoardFile : FileDateBase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EvnetBoardFileNo { get; set; }
        public int EventBoardNo { get; set; }

        // Navigation property
        [ForeignKey("EventBoardNo")]
        public EventBoard EventBoard { get; set; }
    }
}