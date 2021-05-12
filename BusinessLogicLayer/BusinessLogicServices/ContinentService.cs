using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Continent;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class ContinentService : IContinentService
    {
        private readonly ContinentRepo _continentRepo;

        public ContinentService(ContinentRepo continentRepo)
        {
            _continentRepo = continentRepo;
        }

        public IEnumerable<ContinentViewModel> GetAllContinents()
        {
            return _continentRepo.GetAll().Select(x => new ContinentViewModel
            {
                ContinentID = x.ContinentID.ToString(),
                Name = x.Name,
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

            var newID = _continentRepo.Add(new Continent
            {
                Name = cModel.Name
            });

            return new ContinentViewModel
            {
                ContinentID = newID.ToString(),
                Name = cModel.Name
            };
        }

        public ContinentViewModel UpdateContinent(ContinentModel cModel)
        {
            if (cModel.ContinentID <= 0)
            {
                throw new ContinentException($"UpdateContinent: {cModel.ContinentID} is an invalid ID.");
            }

            if (_continentRepo.Exists(cModel.Name))
            {
                throw new ContinentException("UpdateContinent: Continent already exists.");
            }

            var continent = _continentRepo.Get(cModel.ContinentID);

            if (continent == null)
            {
                throw new ContinentException($"UpdateContinent: No Continent found with ID: {cModel.ContinentID}.");
            }

            continent.Name = cModel.Name;

            _continentRepo.Update(continent);

            return new ContinentViewModel
            {
                Name = continent.Name
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
