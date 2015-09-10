using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class Geography
    {
        public DbGeography Coordinate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}