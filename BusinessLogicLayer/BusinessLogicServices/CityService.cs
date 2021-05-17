using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.City;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class CityService : ICityService
    {
        private readonly ICityRepo _cityRepo;
        private readonly ICountryRepo _countryRepo;
        private readonly IContinentRepo _continentRepo;

        public CityService(ICityRepo cityRepo,
            ICountryRepo countryRepo,
            IContinentRepo continentRepo)
        {
            _cityRepo = cityRepo;
            _countryRepo = countryRepo;
            _continentRepo = continentRepo;
        }

        public IEnumerable<CityViewModel> GetAllCities()
        {
            return _cityRepo.GetAll()
                .Select(x => new CityViewModel
                {
                    CityID = x.CityID.ToString(),
                    Name = x.Name,
                    CountryID = x.CountryID.ToString(),
                    IsCapital = x.IsCapital,
                    Population = x.Population
                });
        }

        public CityViewModel GetCity(int cityID)
        {
            if (cityID <= 0)
            {
                throw new CityException($"GetCity - city: {cityID} is an invalid ID.");
            }

            var city = _cityRepo.Get(cityID);

            if (city == null)
            {
                throw new CityException($"GetCity: No city found with ID: {cityID}.");
            }

            return new CityViewModel
            {
                Name = city.Name,
                CityID = city.CityID.ToString(),
                CountryID = city.CountryID.ToString(),
                IsCapital = city.IsCapital,
                Population = city.Population
            };
        }

        public CityViewModel CreateCity(CityModel cModel)
        {
            if (_cityRepo.Exists(cModel.Name))
            {
                throw new CityException("CreateCity: City already exists.");
            }

            var country = _countryRepo.Get(cModel.CountryID);

            if (country == null)
            {
                throw new CityException($"CreateCity: No country found with ID: {cModel.CountryID}");
            }

            var popCities = (country.Cities.Sum(x => x.Population) + cModel.Population);

            if (popCities > country.Population)
            {
                throw new CityException(
                    "CreateCity: Population of cities combined in this country are bigger than the population of the country itself");
            }

            var newID = _cityRepo.Add(new City(cModel.Name, cModel.Population, cModel.IsCapital, country));

            return new CityViewModel
            {
                CityID = newID.ToString(),
                CountryID = cModel.CountryID.ToString(),
                Population = cModel.Population,
                IsCapital = cModel.IsCapital,
                Name = cModel.Name
            };
        }

        public CityViewModel UpdateCity(CityModel cModel)
        {
            if (cModel.CityID <= 0)
            {
                throw new CityException($"UpdateCity - city: {cModel.CityID} is an invalid ID.");
            }

            if (cModel.CountryID <= 0)
            {
                throw new CityException($"UpdateCity - country: {cModel.CountryID} is an invalid ID.");
            }

            var country = _countryRepo.Get(cModel.CountryID);

            if (country == null)
            {
                throw new CityException("UpdateCity: country doesn't exist.");
            }

            var popCities = (country.Cities.Sum(x => x.Population) + cModel.Population);

            if (popCities > country.Population)
            {
                throw new CityException(
                    "UpdateCity: Population of cities combined in this country are bigger than the population of the country itself");
            }

            var city = _cityRepo.Get(cModel.CityID);

            if (city == null)
            {
                throw new CityException($"UpdateCity: No city found with ID: {cModel.CityID}.");
            }

            if (city.Name != cModel.Name)
            {
                if (_cityRepo.Exists(cModel.Name))
                {
                    throw new CityException("UpdateCity: City with this name already exists.");
                }
            }

            city.Name = cModel.Name;
            city.IsCapital = cModel.IsCapital;
            city.CountryID = cModel.CountryID;
            city.Population = cModel.Population;

            _cityRepo.Update(city);

            return new CityViewModel
            {
                Name = city.Name,
                CountryID = city.CountryID.ToString(),
                CityID = city.CityID.ToString(),
                IsCapital = city.IsCapital,
                Population = city.Population
            };
        }

        public void RemoveCity(int id)
        {
            if (id <= 0)
            {
                throw new CityException($"RemoveCity: {id} is an invalid ID.");
            }

            var city = _cityRepo.Get(id);

            if (city == null)
            {
                throw new CityException("RemoveCity: city doesn't exists.");
            }

            _cityRepo.Remove(id);
        }
    }
}
