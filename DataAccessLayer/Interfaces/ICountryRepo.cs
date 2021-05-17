using DataAccessLayer.Model;

namespace DataAccessLayer.Interfaces
{
    public interface ICountryRepo : IRepository<Country>
    {
        bool Exists(int id, string name);
    }
}
