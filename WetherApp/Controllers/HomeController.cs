using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WetherApp.Models;
using WetherApp.ViewModel;

namespace WetherApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(City c)
        {
            ViewBag.Checking = false;
            if (c.name != null)
            {
                ViewBag.Checking = true;
                return View(c);
            }
            else return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
      
        [HttpPost]
        public ActionResult GetCityFromApi(FormCollection formCollection)
        {
            string city = formCollection["city"].ToString();
            //city = "London";
            string appid = "a4e8a4397c4019fed558b5baf7a0d911";
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=a4e8a4397c4019fed558b5baf7a0d911", city);

            City _city = new City();
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                //WeatherInfo _info = (new JavaScriptSerializer()).Deserialize<WeatherInfo>(json);
                JObject jObj = JObject.Parse(json);

                _city.name = jObj["name"].ToString();
                _city.country = jObj["sys"]["country"].ToString();
                _city.humidity = (int)jObj["main"]["humidity"];
                _city.temp = jObj["main"]["temp"].ToString();
            }

            return RedirectToAction("Index", _city);
        }
    }
}