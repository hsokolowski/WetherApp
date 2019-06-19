using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WetherApp.Models
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int humidity { get; set; }
        public string temp { get; set; }
    }
}