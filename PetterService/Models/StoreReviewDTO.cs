using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class StoreReviewDTO : FileDateBase
    {
        public int StoreReviewNo { get; set; }
        public int StoreNo { get; set; }
        public int MemberNo { get; set; }
        public double Grade { get; set; }
        public string Content { get; set; }

        // Navigation property
        public List<StoreReviewFile> StoreReviewFiles { get; set; }
        public List<StoreReviewLike> StoreReviewLikes { get; set; }
        public List<StoreReviewStats> StoreReviewStats { get; set; }
    }
}