using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.City;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class CityService : ICityService
    {
        private readonly CityRepo _cityRepo;
        private readonly CountryRepo _countryRepo;

        public CityService(CityRepo cityRepo, CountryRepo countryRepo)
        {
            _cityRepo = cityRepo;
            _countryRepo = countryRepo;
        }

        public IEnumerable<CityViewModel> GetAllCities()
        {
            return _cityRepo.GetAll().Select(x => new CityViewModel
            {
                CityID = x.CityID.ToString(),
                Name = x.Name,
                CountryID = x.CountryID.ToString(),
                IsCapital = x.IsCapital,
                Population = x.Population
            });
        }

        public CityViewModel GetCity(int id)
        {
            if (id <= 0)
            {
                throw new CityException($"GetCity: {id} is an invalid ID.");
            }

            var city = _cityRepo.Get(id);

            if (city == null)
            {
                throw new CityException($"GetCity: No city found with ID: {id}.");
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

            var newID = _cityRepo.Add(new City(cModel.Name, cModel.Population, cModel.IsCapital, cModel.CountryID));

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

            if (_cityRepo.Exists(cModel.Name))
            {
                throw new CityException("UpdateCity: city already exists.");
            }

            var country = _countryRepo.Get(cModel.CountryID);

            if (country == null)
            {
                throw new CityException("UpdateCity: country doesn't exist.");
            }

            var city = _cityRepo.Get(cModel.CityID);

            if (city == null)
            {
                throw new CityException($"UpdateCity: No city found with ID: {cModel.CityID}.");
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
