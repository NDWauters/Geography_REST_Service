using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ContinentRepo : IRepository<Continent>
    {
        private readonly GeographyContext _db;

        public ContinentRepo(GeographyContext db)
        {
            _db = db;
        }

        public IEnumerable<Continent> GetAll(IList<int> continentIDs = null)
        {
            if (continentIDs != null)
            {
                return _db.Continents
                    .Include("Countries")
                    .Where(x => continentIDs.Contains(x.ContinentID));
            }

            return _db.Continents.Include("Countries");
        }

        public Continent Get(int id)
        {
            return _db.Continents
                .Include("Countries")
                .FirstOrDefault(x => x.ContinentID == id);
        }

        public int Add(Continent continent)
        {
            _db.Continents.Add(continent);
            _db.SaveChanges();

            return continent.ContinentID;
        }

        public void Remove(int id)
        {
            var continent = _db.Continents.First(x => x.ContinentID == id);

            _db.Continents.Remove(continent);
            _db.SaveChanges();
        }

        public void Update(Continent continent)
        {
            var continentDb = _db.Continents.First(x => x.ContinentID == continent.ContinentID);

            continentDb.Name = continent.Name;
            continentDb.Countries = continent.Countries;

            _db.Continents.Update(continentDb);
            _db.SaveChanges();
        }

        public bool Exists(string name)
        {
            var continent = _db.Continents.FirstOrDefault(x => x.Name == name);

            if (continent != null)
            {
                return true;
            }

            return false;
        }
    }
}
