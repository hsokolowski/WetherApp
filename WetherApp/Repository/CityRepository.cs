using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using WetherApp.DAL;
using WetherApp.Models;
using WetherApp.Repository;
using WetherApp.ViewModel;

namespace WetherApp.Repository
{
    public class CityRepository : ICityRepository
    {
        public DB db = new DB();

        public IEnumerable<City> GetAll()
        {
            return db.Cities;
        }

        public City Get(int id)
        {
            return db.Cities.FirstOrDefault(x => x.id == id);
        }

        public City Add(City item)
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
            City _city = new City() { id = id };
            db.Cities.Attach(_city);
            db.Cities.Remove(_city);
            db.SaveChanges();
            return true;
        }

        public bool Update(City item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }


            City tmp = Get(item.id);
            if(tmp==null)
            {
                return false;
            }

            //db.Entry(item).State = EntityState.Modified;   
            
            try
            {
                db.Set<City>().AddOrUpdate(item);
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

        
    }
}