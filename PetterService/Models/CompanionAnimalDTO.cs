using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class CompanionAnimalDTO : FileDateBase
    {
        public int CompanionAnimalNo { get; set; }
        public int MemberNo { get; set; }
        public string PetCategory { get; set; }
        public string PetCode { get; set; }
        public string Name { get; set; }
        public byte Age { get; set; }
        public byte Weight { get; set; }
        public string Gender { get; set; }
        public string Neutralization { get; set; }
        public string Marking { get; set; }
        public string Medication { get; set; }
        public string Feature { get; set; }
        //public string FileName { get; set; }
        //public string FilePath { get; set; }
        //public string StateFlag { get; set; }

        // Navigation property
        //public Member Member { get; set; }

        //[ForeignKey("PetKind")]
        //public PetKind PetKind { get; set; }
    }
}