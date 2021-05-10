using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.Continent;

namespace BusinessLogicLayer.Interfaces
{
    public interface IContinentService
    {
        IEnumerable<ContinentViewModel> GetAllContinents();
        ContinentViewModel GetContinent();
    }
}