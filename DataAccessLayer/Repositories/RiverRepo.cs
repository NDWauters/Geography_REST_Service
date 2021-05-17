using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class RiverRepo : IRiverRepo
    {
        private readonly GeographyContext _db;

        public RiverRepo(GeographyContext db)
        {
            _db = db;
        }

        public IEnumerable<River> GetAll(IList<int> riverIds = null)
        {
            if (riverIds != null)
            {
                return _db.Rivers
                    .Where(x => riverIds.Contains(x.RiverID));
            }

            return _db.Rivers.Include("Countries");
        }

        public River Get(int id)
        {
            return _db.Rivers
                .Include("Countries")
                .FirstOrDefault(x => x.RiverID == id);
        }

        public int Add(River river)
        {
            _db.Rivers.Add(river);
            _db.SaveChanges();

            return river.RiverID;
        }

        public void Remove(int id)
        {
            var river = _db.Rivers.First(x => x.RiverID == id);

            _db.Rivers.Remove(river);
            _db.SaveChanges();
        }

        public void Update(River river)
        {
            var riverDb = _db.Rivers.First(x => x.RiverID == river.RiverID);

            riverDb.Name = river.Name;
            riverDb.Countries = river.Countries;

            _db.Rivers.Update(riverDb);
            _db.SaveChanges();
        }

        public bool Exists(string name)
        {
            var river = _db.Rivers.FirstOrDefault(x => x.Name == name);

            if (river != null)
            {
                return true;
            }

            return false;
        }
    }
}
