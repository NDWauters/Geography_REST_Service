using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.Country;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<CountryViewModel> GetAllCountries();
        CountryViewModel GetCountry();
    }
}