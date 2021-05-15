using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(IList<int> IDs = null);
        T Get(int id);
        int Add(T model);
        void Remove(int id);
        void Update(T model);
        bool Exists(string name);
    }
}
