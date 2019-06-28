using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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


            //int index = db.Cities.FindIndex(p => p.id == item.id);
            //if (index == -1)
            //{
            //    return false;
            //}

            db.Entry(item).State = EntityState.Modified;       
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
            return true;
        }
    }
}