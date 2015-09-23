using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreNewsDTO : FileDateBase
    {
        public int StoreNewsNo { get; set; }
        public int StoreNo { get; set; }
        public string Content { get; set; }

        //public int isCount { get; set; }

        // Navigation property
        public List<StoreNewsStats> StoreNewsStats { get; set; }
        public List<StoreNewsFile> StoreNewsFiles { get; set; }
        public List<StoreNewsLike> StoreNewsLikes { get; set; }
        public List<StoreNewsReply> StoreNewsReplies { get; set; }
    }
}