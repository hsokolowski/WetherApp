using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WetherApp.Models;
using WetherApp.ViewModel;

namespace WetherApp.DAL
{
    public class DB : DbContext
    {
        public DB() : base("Cities")
        {

        }
        public DbSet<City> Cities { get; set; }
    }
}