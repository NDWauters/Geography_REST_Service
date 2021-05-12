using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class CityRepo : IRepository<City>
    {
        private readonly GeographyContext _db;

        public CityRepo(GeographyContext db)
        {
            _db = db;
        }
        public IEnumerable<City> GetAll()
        {
            return _db.Cities.Include("Countries");
        }

        public IEnumerable<City> GetAll(IList<int> cityIds)
        {
            return _db.Cities
                .Include("Countries")
                .Where(x => cityIds.Contains(x.CityID));
        }

        public City Get(int id)
        {
            return _db.Cities
                .Include("Countries")
                .FirstOrDefault(x => x.CityID == id);
        }

        public int Add(City city)
        {
            _db.Cities.Add(city);
            _db.SaveChanges();

            return city.CityID;
        }

        public void Remove(int id)
        {
            var city = _db.Cities.First(x => x.CityID == id);

            _db.Cities.Remove(city);
            _db.SaveChanges();
        }

        public void Update(City city)
        {
            var cityDb = _db.Cities.First(x => x.CityID == city.CityID);

            cityDb.Name = city.Name;
            cityDb.CountryID = city.CountryID;
            cityDb.IsCapital = city.IsCapital;
            cityDb.Population = city.Population;

            _db.Cities.Update(cityDb);
            _db.SaveChanges();
        }

        public bool Exists(string name)
        {
            var city = _db.Cities.FirstOrDefault(x => x.Name == name);

            if (city != null)
            {
                return true;
            }

            return false;
        }

        public bool Exists(int id)
        {
            var city = _db.Cities.FirstOrDefault(x => x.CityID == id);

            if (city != null)
            {
                return true;
            }

            return false;
        }
    }
}
