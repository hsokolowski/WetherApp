using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WetherApp.Models;
using WetherApp.ViewModel;

namespace WetherApp.Repository
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAll();
        City Get(int id);
        City Add(City item);
        bool Update(City item);
        bool Delete(int id);
    }
}
