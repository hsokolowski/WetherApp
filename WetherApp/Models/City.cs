using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WetherApp.Models
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        [RegularExpression(@"[A-Z]{2,3}$",ErrorMessage = "Only uppercase Letters are allowed.")]
        public string country { get; set; }
        [Range(0, 100)]
        public int humidity { get; set; }
        
        public string temp { get; set; }
    }
}