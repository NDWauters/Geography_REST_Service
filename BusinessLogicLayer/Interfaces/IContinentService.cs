using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.Continent;

namespace BusinessLogicLayer.Interfaces
{
    public interface IContinentService
    {
        IEnumerable<ContinentViewModel> GetAllContinents();
        ContinentViewModel GetContinent(int id);
        ContinentViewModel CreateContinent(ContinentModel cModel);
        ContinentViewModel UpdateContinent(ContinentModel cModel);
        void RemoveContinent(int id);
    }
}