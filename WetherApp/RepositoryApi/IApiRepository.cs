using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WetherApp.Models;

namespace WetherApp.RepositoryApi
{
    public interface IApiRepository
    {
        City GetFromApi(string name);
        bool UpdateAll(IEnumerable<City> list);
    }
}
