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
        public string localization { get; set; }
        public int humidity { get; set; }
        public string temp { get; set; }
    }
}