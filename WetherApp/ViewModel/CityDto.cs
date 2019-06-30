using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //[RegularExpression(" ^ - [0 - 9] + $ | ^ [0 - 9] + $ ", ErrorMessage = "Tempeture must be numeric")]
        public string temp { get; set; }
    }
}