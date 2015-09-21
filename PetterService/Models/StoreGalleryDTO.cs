using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreGalleryDTO : FileDateBase
    {
        public int StoreGalleryNo { get; set; }
        public int StoreNo { get; set; }
        public string Content { get; set; }

        // Navigation property
        public List<StoreGalleryStats> StoreGalleryStats { get; set; }
        public List<StoreGalleryFile> StoreGalleryFiles { get; set; }
        public List<StoreGalleryReply> StoreGalleryReplies { get; set; }
    }
}