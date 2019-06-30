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
using WetherApp.RepositoryApi;
using WetherApp.ViewModel;

namespace WetherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _serviceDb;
        private readonly IApiRepository _serviceApi;

        public HomeController(ICityRepository service, IMapper mapper, IApiRepository api)
        {
            _serviceDb = service;
            _mapper = mapper;
            _serviceApi = api;
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
            IEnumerable<CityDto> list = _serviceDb.GetAll()
                .Select(w => _mapper.Map<CityDto>(w))
                .ToList();
            return View(list);
        }

        public ActionResult Contact()  //normal
        {
            IEnumerable<CityDto> list = _serviceDb.GetAll()
                .Select(w => _mapper.Map<CityDto>(w))
                .ToList();

            return View(list);
        }

        [HttpPost]
        public ActionResult GetCityFromApi(FormCollection formCollection)
        {
            string cityName = formCollection["city"].ToString();

            City city = _serviceApi.GetFromApi(cityName);
            if(city==null)
            {
                ViewBag.MessageError = "Wrong city name!";
                ViewBag.Checking = false;
                return View("Index");
            }
            TempData["getCity"] = city;

            return RedirectToAction("Index", city);
        }

        public ActionResult Add()
        {
            City city = (City)TempData["getCity"];
            _serviceDb.Add(city);

            return RedirectToAction("Contact");
        }
        public ActionResult Delete(int id)
        {
            _serviceDb.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null) id = 1;
            return View(_mapper.Map<CityDto>(_serviceDb.Get(id.Value)));
        }

        public ActionResult Edit(int id)
        {
            City city = _serviceDb.Get(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(City c)
        {
            _serviceDb.Update(c);
            return RedirectToAction("Contact");
        }

        public ActionResult UpdateTempeture()
        {
            IEnumerable<City> list = _serviceDb.GetAll()
                .Select(w => _mapper.Map<City>(w))
                .ToList();

            if((_serviceApi.UpdateAll(list)))
            {
                TempData["CityErrorMessage"] = "All cities have current weather.";
                TempData["CityErrorMessageColor"] =  "green";
            }
            else
            {
                TempData["CityErrorMessage"] = "Error! Something goes wrong.";
                TempData["CityErrorMessageColor"] = "red";
            }
            
            return RedirectToAction("Contact");
        }
    }
}