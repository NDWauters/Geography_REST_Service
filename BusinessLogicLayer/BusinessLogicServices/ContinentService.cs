using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Continent;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class ContinentService : IContinentService
    {
        private readonly IContinentRepo _continentRepo;
        private readonly ICountryRepo _countryRepo;

        public ContinentService(IContinentRepo continentRepo, ICountryRepo countryRepo)
        {
            _continentRepo = continentRepo;
            _countryRepo = countryRepo;
        }

        public IEnumerable<ContinentViewModel> GetAllContinents()
        {
            return _continentRepo.GetAll().Select(x => new ContinentViewModel
            {
                ContinentID = x.ContinentID.ToString(),
                Name = x.Name,
                Population = x.Population,
                Countries = x.Countries.Select(y => y.CountryID
                    .ToString())
                    .ToList()
            });
        }

        public ContinentViewModel GetContinent(int id)
        {
            if (id <= 0)
            {
                throw new ContinentException($"GetContinent: {id} is an invalid ID.");
            }

            var continent = _continentRepo.Get(id);

            if (continent == null)
            {
                throw new ContinentException($"GetContinent: No continent found with ID: {id}.");
            }

            return new ContinentViewModel
            {
                Name = continent.Name,
                Population = continent.Population,
                Countries = continent.Countries
                    .Select(x => x.CountryID.ToString())
                    .ToList()
            };
        }

        public ContinentViewModel CreateContinent(ContinentModel cModel)
        {
            if (_continentRepo.Exists(cModel.Name))
            {
                throw new ContinentException("CreateContinent: Continent already exists.");
            }
            
            IList<Country> countries = new List<Country>();

            if (cModel.Countries != null && cModel.Countries.Count != 0)
            {
                countries = _countryRepo
                    .GetAll(cModel.Countries)
                    .ToList();

                if (countries.Count == 0)
                {
                    throw new ContinentException("CreateContinent: Not all given countries exist.");
                }
            }
            
            int newID = _continentRepo.Add(new Continent(cModel.Name, countries));

            return new ContinentViewModel
            {
                ContinentID = newID.ToString(),
                Name = cModel.Name,
                Population = countries.Sum(x => x.Population)
            };
        }

        public ContinentViewModel UpdateContinent(ContinentModel cModel)
        {
            if (cModel.ContinentID <= 0)
            {
                throw new ContinentException($"UpdateContinent: {cModel.ContinentID} is an invalid ID.");
            }

            var continent = _continentRepo.Get(cModel.ContinentID);

            if (continent == null)
            {
                throw new ContinentException($"UpdateContinent: No Continent found with ID: {cModel.ContinentID}.");
            }
            
            IList<Country> countries = new List<Country>();

            if (cModel.Countries != null && cModel.Countries.Count != 0)
            {
                countries = _countryRepo
                    .GetAll(cModel.Countries)
                    .ToList();

                if (countries.Count == 0)
                {
                    throw new ContinentException("UpdateContinent: Not all given countries exist.");
                }
            }

            continent.Name = cModel.Name;
            continent.Countries = countries;

            _continentRepo.Update(continent);

            return new ContinentViewModel
            {
                Name = continent.Name,
                Population = countries.Sum(x => x.Population),
                Countries = continent.Countries
                    .Select(x => x.CountryID.ToString())
                    .ToList()
            };
        }

        public void RemoveContinent(int id)
        {
            if (id <= 0)
            {
                throw new ContinentException($"RemoveContinent: {id} is an invalid ID.");
            }

            var continent = _continentRepo.Get(id);

            if (continent == null)
            {
                throw new ContinentException("RemoveContinent: Continent doesn't exists.");
            }

            if (continent.Countries.Count != 0)
            {
                throw new ContinentException("RemoveContinent: Countries not empty");
            }

            _continentRepo.Remove(id);
        }
    }
}
