using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class MemberDTO
    {
        public int MemberNo { get; set; }
        public string MemberID { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public DbGeography Coordinate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}