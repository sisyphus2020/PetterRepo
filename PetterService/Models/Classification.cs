using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class Classification
    {
        public virtual int CompanyNo { get; set; }
        [MaxLength(100)]
        public string PictureName { get; set; }
        [MaxLength(100)]
        public string PicturePath { get; set; }
        [MaxLength(4)]
        public string StartHours { get; set; }
        [MaxLength(4)]
        public string EndHours { get; set; }
        [MaxLength(200)]
        public string Introduction { get; set; }
        public DbGeography Coordinate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Index("IDX_CRADE"), Range(0, 10)]
        public decimal Grade { get; set; }
        [Index("IDX_REVIEWCOUNT")]
        public int ReviewCount { get; set; }
        [Index("IDX_BOOKMARK")]
        public int Bookmark { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}