﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WetherApp.ViewModel
{
    public class CityDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int humidity { get; set; }
        public double temp { get; set; }
    }
}