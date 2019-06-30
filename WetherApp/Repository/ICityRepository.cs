using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WetherApp.ViewModel;

namespace WetherApp.Repository
{
    public interface ICityRepository
    {
        IEnumerable<CityDto> GetAll();
        CityDto Get(int id);
        CityDto Add(CityDto item);
        bool Update(CityDto item);
        bool Delete(int id);
        bool UpdateAll(IEnumerable<CityDto> list);
    }
}
