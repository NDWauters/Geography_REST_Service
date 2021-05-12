using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.City;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICityService
    {
        IEnumerable<CityViewModel> GetAllCities();
        CityViewModel GetCity(int id);
        CityViewModel CreateCity(CityModel cModel);
        CityViewModel UpdateCity(CityModel cModel);
        void RemoveCity(int id);
    }
}