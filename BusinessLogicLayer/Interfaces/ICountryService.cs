using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.Country;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<CountryViewModel> GetAllCountries();
        CountryViewModel GetCountry(int countryID);
        CountryViewModel CreateCountry(CountryModel cModel);
        CountryViewModel UpdateCountry(CountryModel cModel);
        void RemoveCountry(int id);
    }
}