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
using WetherApp.ViewModel;

namespace WetherApp.Controllers
{
    public class HomeController : Controller
    {
        private DB db = new DB();

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
            List<CityDto> list = db.Cities.ToList();
            return View(list);
        }

        public ActionResult Contact()
        {
            List<CityDto> list = db.Cities.ToList();

            return View(list);
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
                catch(Exception e)
                {
                    ViewBag.MessageError = "Wrong city name!";
                    ViewBag.Checking = false;
                    return View("Index");
                }
                
            }

            
        }
        
        public ActionResult Add()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<City, CityDto>();
            });

            IMapper mapper = config.CreateMapper();
            var source = (City)TempData["getCity"];
            var dest = mapper.Map<City, CityDto>(source);

            db.Cities.Add(dest);
            db.SaveChanges();

            return RedirectToAction("Contact");
        }
        public ActionResult Delete(int id)
        {
            using (DB db = new DB())
            {
                CityDto _city = new CityDto() { id = id };
                db.Cities.Attach(_city);
                db.Cities.Remove(_city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Details(int id)
        {
            CityDto _city = new CityDto();
            using (DB db = new DB())
            {
                
                db.Cities.Attach(_city);
                _city = db.Cities.Find(id);
            }
            return View(_city);
        }

        public ActionResult Edit(int id)
        {
            using (DB db = new DB())
            {
                CityDto _city = db.Cities.Where(s => s.id == id).FirstOrDefault();
                return View(_city);
            }
                
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CityDto c)
        {
            using (DB db = new DB())
            {
                db.Entry(c).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                return RedirectToAction("Index");
            }
            
        }
    }
}