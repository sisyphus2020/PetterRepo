using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class Classification : GeographyFileDateBase
    {
        public virtual int CompanyNo { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        //[MaxLength(100)]
        //public string FileName { get; set; }
        //[MaxLength(100)]
        //public string FilePath { get; set; }
        [Column("StartTime", TypeName = "char"), MaxLength(4)]
        public string StartTime { get; set; }
        [Column("EndTime", TypeName = "char"), MaxLength(4)]
        public string EndTime { get; set; }
        [MaxLength(200)]
        public string Introduction { get; set; }
        //public DbGeography Coordinate { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }
        //[Index("IDX_GRADE"), Range(0, 10)]
        //public double Grade { get; set; }
        //[Index("IDX_REVIEWCOUNT")]
        //public int ReviewCount { get; set; }
        //[Index("IDX_BOOKMARK")]
        //public int Bookmark { get; set; }
        //[MaxLength(1), Column("StateFlag", TypeName = "char")]
        //public string StateFlag{ get; set; }
        [MaxLength(20)]
        public string WriteIP { get; set; }
        [MaxLength(20)]
        public string ModifyIP { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime DateModified { get; set; }
        //public DateTime DateDeleted { get; set; }
    }
}