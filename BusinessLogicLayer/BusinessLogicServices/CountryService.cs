using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Country;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepo _countryRepo;
        private readonly ContinentRepo _continentRepo;
        private readonly RiverRepo _riverRepo;
        private readonly CityRepo _cityRepo;

        public  CountryService(
            CountryRepo countryRepo,
            ContinentRepo continentRepo,
            RiverRepo riverRepo,
            CityRepo cityRepo)
        {
            _countryRepo = countryRepo;
            _continentRepo = continentRepo;
            _riverRepo = riverRepo;
            _cityRepo = cityRepo;
        }

        public IEnumerable<CountryViewModel> GetAllCountries()
        {
            return _countryRepo.GetAll().Select(x => new CountryViewModel
            {
                CountryID = x.CountryID.ToString(),
                Name = x.Name,
                Population = x.Population,
                Surface = x.Surface,
                ContinentID = x.ContinentID.ToString(),
                Cities = x.Cities
                    .Select(y => y.CityID.ToString())
                    .ToList(),
                Rivers = x.Rivers
                    .Select(y => y.RiverID.ToString())
                    .ToList()
            });
        }

        public CountryViewModel GetCountry(int id)
        {
            if (id <= 0)
            {
                throw new CountryException($"GetCountry: {id} is an invalid ID.");
            }

            var country = _countryRepo.Get(id);

            if (country == null)
            {
                throw new CountryException($"GetCountry: No country found with ID: {id}.");
            }

            return new CountryViewModel
            {
                Name = country.Name,
                Population = country.Population,
                Surface = country.Surface,
                ContinentID = country.ContinentID.ToString(),
                CountryID = country.CountryID.ToString(),
                Cities = country.Cities
                    .Select(y => y.CityID.ToString())
                    .ToList(),
                Rivers = country.Rivers
                    .Select(y => y.RiverID.ToString())
                    .ToList()
            };
        }

        public CountryViewModel CreateCountry(CountryModel cModel)
        {
            if (cModel.ContinentID <= 0)
            {
                throw new CountryException($"CreateCountry - continent: {cModel.ContinentID} is an invalid ID.");
            }

            if (cModel.Surface <= 0)
            {
                throw new CountryException("CreateCountry: surface of country is null or lower.");
            }

            if (cModel.Population <= 0)
            {
                throw new CountryException("CreateCountry: population of country is null or lower.");
            }

            if (_countryRepo.Exists(cModel.Name))
            {
                throw new CountryException("CreateCountry: Country already exists.");
            }

            if (!_continentRepo.Exists(cModel.ContinentID))
            {
                throw new CountryException("CreateCountry: Continent doesn't exist.");
            }

            var cities = _cityRepo.GetAll(cModel.Cities).ToList();
            var rivers = _riverRepo.GetAll(cModel.Rivers).ToList();

            var newID = _countryRepo.Add(new Country(cModel.Name, cModel.Population, cModel.Surface, cities, rivers));

            return new CountryViewModel
            {
                CountryID = newID.ToString(),
                Name = cModel.Name,
                ContinentID = cModel.ContinentID.ToString(),
                Population = cModel.Population,
                Surface = cModel.Surface
            };
        }

        public CountryViewModel UpdateCountry(CountryModel cModel)
        {
            if (cModel.CountryID <= 0)
            {
                throw new CountryException($"UpdateCountry - country: {cModel.CountryID} is an invalid ID.");
            }

            if (cModel.ContinentID <= 0)
            {
                throw new CountryException($"UpdateCountry - continent: {cModel.ContinentID} is an invalid ID.");
            }

            if (cModel.Surface <= 0)
            {
                throw new CountryException("UpdateCountry: surface of country is null or lower.");
            }

            if (cModel.Population <= 0)
            {
                throw new CountryException("UpdateCountry: population of country is null or lower.");
            }

            if (_countryRepo.Exists(cModel.Name))
            {
                throw new CountryException("UpdateCountry: Country already exists.");
            }

            if (!_continentRepo.Exists(cModel.ContinentID))
            {
                throw new CountryException("UpdateCountry: Continent doesn't exists.");
            }

            var country = _countryRepo.Get(cModel.CountryID);

            if (country == null)
            {
                throw new CountryException($"UpdateCountry: No country found with ID: {cModel.CountryID}.");
            }

            var cities = _cityRepo.GetAll(cModel.Cities).ToList();
            var rivers = _riverRepo.GetAll(cModel.Rivers).ToList();

            country.Name = cModel.Name;
            country.ContinentID = cModel.ContinentID;
            country.Population = cModel.Population;
            country.Surface = cModel.Surface;
            country.Cities = cities;
            country.Rivers = rivers;

            _countryRepo.Update(country);

            return new CountryViewModel
            {
                Name = country.Name,
                Population = country.Population,
                Surface = country.Surface,
                ContinentID = country.ContinentID.ToString(),
                CountryID = country.CountryID.ToString()
            };
        }

        public void RemoveCountry(int id)
        {
            if (!_countryRepo.Exists(id))
            {
                throw new CountryException("RemoveCountry: Country doesn't exists.");
            }

            var country = _countryRepo.Get(id);

            if (country.Cities.Count != 0)
            {
                throw new CountryException("RemoveCustomer: Cities not empty");
            }

            _countryRepo.Remove(id);
        }
    }
}
