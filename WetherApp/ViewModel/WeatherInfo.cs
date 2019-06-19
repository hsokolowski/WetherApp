using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WetherApp.ViewModel
{
    public class WeatherInfo
    {
        public Town town { get; set; }
        public List<List> list { get; set; }
    }
    public class Town
    {
        public string name { get; set; }
        public string country { get; set; }
    }
    public class Temp
    {
        public double min { get; set; }
        public double max { get; set; }
    }

    public class Weather
    {
        public string description { get; set; }
        public string icon { get; set; }
    }
    public class List
    {
        public Temp temp { get; set; }
        public int humidity { get; set; }
        public List<Weather> weather { get; set; }
    }
}