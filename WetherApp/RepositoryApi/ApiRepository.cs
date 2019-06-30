using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using WetherApp.DAL;
using WetherApp.Models;

namespace WetherApp.RepositoryApi
{
    public class ApiRepository : IApiRepository
    {
        public DB db = new DB();

        string appid = "a4e8a4397c4019fed558b5baf7a0d911";

        public City GetFromApi(string name)
        {
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=a4e8a4397c4019fed558b5baf7a0d911", name);

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
                    _city.temp = (jObj["main"]["temp"].ToString());


                    return _city;
                }
                catch (Exception e)
                {
                    return null;
                }

            }

        }

        public bool UpdateAll(IEnumerable<City> list)
        {
            if (list != null)
            {
                foreach (City c in list)
                {
                    string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=a4e8a4397c4019fed558b5baf7a0d911", c.name);
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            string json = client.DownloadString(url);
                            JObject jObj = JObject.Parse(json);

                            c.country= jObj["sys"]["country"].ToString();
                            c.temp = jObj["main"]["temp"].ToString();
                            c.humidity = (int)jObj["main"]["humidity"];
                            db.Set<City>().AddOrUpdate(c);
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
                    }
                }
                return true;
            }
            return false;

        }
    }
}