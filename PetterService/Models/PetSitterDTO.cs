using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetSitterDTO : Classification
    {
        public int PetSitterNo { get; set; }
        public string PetSitterName { get; set; }
        public string PetSitterAddr { get; set; }

        public List<PetSitterService> PetSitterServices { get; set; }
        public List<PetSitterHoliday> PetSitterHolidays { get; set; }
    }
}