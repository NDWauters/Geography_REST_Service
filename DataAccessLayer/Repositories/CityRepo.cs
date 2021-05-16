using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repositories
{
    public class CityRepo : ICityRepo
    {
        private readonly GeographyContext _db;

        public CityRepo(GeographyContext db)
        {
            _db = db;
        }

        public IEnumerable<City> GetAll(IList<int> cityIds = null)
        {
            if (cityIds != null)
            {
                return _db.Cities
                    .Where(x => cityIds.Contains(x.CityID));
            }

            return _db.Cities;
        }

        public City Get(int id)
        {
            return _db.Cities
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
    }
}
