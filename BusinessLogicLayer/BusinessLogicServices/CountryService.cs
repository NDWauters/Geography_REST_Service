using System.Collections.Generic;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Country;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepo _countryRepo;

        public  CountryService(CountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public IEnumerable<CountryViewModel> GetAllCountries()
        {
            throw new System.NotImplementedException();
        }

        public CountryViewModel GetCountry()
        {
            throw new System.NotImplementedException();
        }
    }
}
