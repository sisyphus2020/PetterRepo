﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Common
{
    public class PetterRequestType
    {
        private int distance;
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string SortBy { get; set; }
        public bool Reverse { get; set; }
        public string Search { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int Distance
        {
            get { return this.distance; }
            set { this.distance = value * 1000; }
        }

        public PetterRequestType()
        {
            this.CurrentPage = 1;
            this.ItemsPerPage = 10;
            this.SortBy = "basic";
            this.Reverse = false;
            this.Search = string.Empty;
            this.Latitude = 126.975971;
            this.Longitude = 37.571483;
            this.Distance = 5;
        }
    }
}