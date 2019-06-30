using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WetherApp.DAL;
using WetherApp.Models;
using WetherApp.Repository;
using WetherApp.ViewModel;

namespace WetherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _serviceWeather;

        public HomeController(ICityRepository service, IMapper mapper)
        {
            _serviceWeather = service;
            _mapper = mapper;
        }


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

        public ActionResult About() //telerik
        {
            ViewBag.Message = "Your application description page.";
            IEnumerable<CityDto> list = _serviceWeather.GetAll()
                .Select(w => _mapper.Map<CityDto>(w))
                .ToList();
            return View(list);
        }

        public ActionResult Contact()  //normal
        {
            IEnumerable<CityDto> list = _serviceWeather.GetAll()
                .Select(w => _mapper.Map<CityDto>(w))
                .ToList();

            return View(list);
        }

        [HttpPost]
        public ActionResult GetCityFromApi(FormCollection formCollection)
        {
            string city = formCollection["city"].ToString();
            //city = "London";
            string appid = "a4e8a4397c4019fed558b5baf7a0d911";
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=a4e8a4397c4019fed558b5baf7a0d911", city);

            CityDto _city = new CityDto();
            using (WebClient client = new WebClient())
            {
                try
                {
                    string json = client.DownloadString(url);
                    JObject jObj = JObject.Parse(json);

                    _city.name = jObj["name"].ToString();
                    _city.country = jObj["sys"]["country"].ToString();
                    _city.humidity = (int)jObj["main"]["humidity"];
                    _city.temp = jObj["main"]["temp"].ToString();
                    TempData["getCity"] = _city;

                    return RedirectToAction("Index", _city);
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "Wrong city name!";
                    ViewBag.Checking = false;
                    return View("Index");
                }

            }


        }

        public ActionResult Add()
        {
            CityDto city = (CityDto)TempData["getCity"];
            _serviceWeather.Add(city);

            return RedirectToAction("Contact");
        }
        public ActionResult Delete(int id)
        {
            _serviceWeather.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            return View(_mapper.Map<CityDto>(_serviceWeather.Get(id.Value)));
        }

        public ActionResult Edit(int id)
        {
            CityDto city = _serviceWeather.Get(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CityDto c)
        {
            _serviceWeather.Update(c);
            return RedirectToAction("Contact");
        }

        public ActionResult UpdateTempeture()
        {
            IEnumerable<CityDto> list = _serviceWeather.GetAll()
                .Select(w => _mapper.Map<CityDto>(w))
                .ToList();


            TempData["CityErrorMessage"] = (_serviceWeather.UpdateAll(list)) ? "All cities have current weather." : "Error! Something goes wrong.";

            return RedirectToAction("Contact");
            //foreach(CityDto c in list)
            //{
            //    string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=a4e8a4397c4019fed558b5baf7a0d911", c.name);
            //    using (WebClient client = new WebClient())
            //    {
            //        try
            //        {
            //            string json = client.DownloadString(url);
            //            JObject jObj = JObject.Parse(json);

            //            c.temp = jObj["main"]["temp"].ToString();
            //            _serviceWeather.Update(c);
            //        }
            //        catch (Exception e)
            //        {
            //            ViewBag.CityError = "Error! Wrong city name. Check the correctness.";
            //            ViewBag.CityId = c.id;
            //            return RedirectToAction("Contact");
            //        }
            //    }
            //}

            //return RedirectToAction("Contact");
        }
    }
}