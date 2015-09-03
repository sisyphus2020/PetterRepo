using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class Classification
    {
        public int CompanyNo { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public string StartHours { get; set; }
        public string EndHours { get; set; }
        public string Introduction { get; set; }
        public DbGeography Coordinate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Range(0, 10)]
        public decimal Grade { get; set; }
        public int ReviewCount { get; set; }
        public int Bookmark { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}