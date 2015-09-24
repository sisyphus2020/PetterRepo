using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class BoardDTO : FileDateBase
    {
        public int BoardNo { get; set; }
        public int StoreNo { get; set; }
        public string CodeID { get; set; }
        public string Content { get; set; }

        //public int isCount { get; set; }

        // Navigation property
        public List<BoardStats> BoardStats { get; set; }
        public List<BoardFile> BoardFiles { get; set; }
        public List<BoardLike> BoardLikes { get; set; }
        public List<BoardReply> BoardReplies { get; set; }
    }
}