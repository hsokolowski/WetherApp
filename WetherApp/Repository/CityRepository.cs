using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using WetherApp.DAL;
using WetherApp.Repository;
using WetherApp.ViewModel;

namespace WetherApp.Repository
{
    public class CityRepository : ICityRepository
    {
        public DB db = new DB();

        public IEnumerable<CityDto> GetAll()
        {
            return db.Cities;
        }

        public CityDto Get(int id)
        {
            return db.Cities.FirstOrDefault(x => x.id == id);
        }

        public CityDto Add(CityDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            db.Cities.Add(item);
            db.SaveChanges();
            return item;
        }

        public bool Delete(int id)
        {
            CityDto _city = new CityDto() { id = id };
            db.Cities.Attach(_city);
            db.Cities.Remove(_city);
            db.SaveChanges();
            return true;
        }

        public bool Update(CityDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }


            CityDto tmp = Get(item.id);
            if(tmp==null)
            {
                return false;
            }

            //db.Entry(item).State = EntityState.Modified;   
            
            try
            {
                db.Set<CityDto>().AddOrUpdate(item);
                db.SaveChanges();
                return true;
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

        public bool UpdateAll(IEnumerable<CityDto> list)
        {
            if (list != null)
            {
                foreach (CityDto c in list)
                {
                    string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=a4e8a4397c4019fed558b5baf7a0d911", c.name);
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            string json = client.DownloadString(url);
                            JObject jObj = JObject.Parse(json);

                            c.temp = jObj["main"]["temp"].ToString();
                            c.humidity = (int)jObj["main"]["humidity"];
                            db.Set<CityDto>().AddOrUpdate(c);
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