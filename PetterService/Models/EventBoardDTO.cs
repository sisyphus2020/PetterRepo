using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class EventBoardDTO : BoardBase
    {
        public int EventBoardNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        public List<EventBoardStats> EventBoardStats { get; set; }
        public List<EventBoardFile> EventBoardFiles { get; set; }
        public List<EventBoardReply> EventBoardReplys { get; set; }
    }
}