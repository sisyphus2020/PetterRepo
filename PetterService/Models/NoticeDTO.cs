using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class NoticeDTO : BoardBase
    {
        public int NoticeNo { get; set; }
        public int MemberNo { get; set; }

        // Navigation property
        public List<NoticeFile> NoticeFiles { get; set; }
        public List<NoticeReply> NoticeReplies { get; set; }
    }
}