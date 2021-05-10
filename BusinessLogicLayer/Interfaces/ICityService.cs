using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.City;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICityService
    {
        IEnumerable<CityViewModel> GetAllRivers();
        CityViewModel GetRiver();
    }
}