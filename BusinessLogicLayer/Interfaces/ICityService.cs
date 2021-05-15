using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.City;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICityService
    {
        IEnumerable<CityViewModel> GetAllCities();
        CityViewModel GetCity(int countryID, int cityID);
        CityViewModel CreateCity(CityModel cModel);
        CityViewModel UpdateCity(CityModel cModel);
        void RemoveCity(int id);
    }
}