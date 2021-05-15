using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class CountryRepo : IRepository<Country>
    {
        private readonly GeographyContext _db;

        public CountryRepo(GeographyContext db)
        {
            _db = db;
        }

        public IEnumerable<Country> GetAll(IList<int> countryIds = null)
        {
            if (countryIds != null)
            {
                return _db.Countries
                    .Include("Cities")
                    .Include("Rivers")
                    .Where(x => countryIds.Contains(x.CountryID));
            }

            return _db.Countries
                .Include("Cities")
                .Include("Rivers");
        }

        public Country Get(int id)
        {
            return _db.Countries
                .Include("Cities")
                .Include("Rivers")
                .FirstOrDefault(x => x.CountryID == id);
        }

        public int Add(Country country)
        {
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.CountryID;
        }

        public void Remove(int id)
        {
            var country = _db.Countries.First(x => x.CountryID == id);

            _db.Countries.Remove(country);
            _db.SaveChanges();
        }

        public void Update(Country country)
        {
            var countrytDb = _db.Countries.First(x => x.CountryID == country.CountryID);

            countrytDb.Name = country.Name;
            countrytDb.ContinentID = country.ContinentID;
            countrytDb.Cities = country.Cities;
            countrytDb.Rivers = country.Rivers;
            countrytDb.Population = country.Population;
            countrytDb.Surface = country.Surface;

            _db.Countries.Update(countrytDb);
            _db.SaveChanges();
        }

        public bool Exists(string name)
        {
            var country = _db.Countries.FirstOrDefault(x => x.Name == name);

            if (country != null)
            {
                return true;
            }

            return false;
        }
    }
}
