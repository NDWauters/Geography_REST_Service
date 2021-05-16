using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.River;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class RiverService: IRiverService
    {
        private readonly IRiverRepo _riverRepo;
        private readonly ICountryRepo _countryRepo;

        public RiverService(IRiverRepo riverRepo, ICountryRepo countryRepo)
        {
            _riverRepo = riverRepo;
            _countryRepo = countryRepo;
        }

        public IEnumerable<RiverViewModel> GetAllRivers()
        {
            return _riverRepo.GetAll().Select(x => new RiverViewModel
            {
                Name = x.Name,
                Length = x.Length,
                RiverID = x.RiverID.ToString(),
                Countries = x.Countries
                    .Select(y => y.CountryID.ToString())
                    .ToList()
            });
        }

        public RiverViewModel GetRiver(int id)
        {
            if (id <= 0)
            {
                throw new RiverException($"GetRiver: {id} is an invalid ID.");
            }

            var river = _riverRepo.Get(id);

            if (river == null)
            {
                throw new RiverException($"GetRiver: No river found with ID: {id}.");
            }

            return new RiverViewModel
            {
                Name = river.Name,
                RiverID = river.RiverID.ToString(),
                Length = river.Length,
                Countries = river.Countries
                    .Select(y => y.CountryID.ToString())
                    .ToList()
            };
        }

        public RiverViewModel CreateRiver(RiverModel rModel)
        {
            if (_riverRepo.Exists(rModel.Name))
            {
                throw new RiverException("CreateRiver: river already exists.");
            }

            var checkCountries = CheckCountries(rModel.Countries);

            if (!string.IsNullOrEmpty(checkCountries.Item1) && checkCountries.Item2 == null)
            {
                throw new RiverException($"CreateRiver: {checkCountries.Item1}");
            }

            var newID = _riverRepo.Add(new River(rModel.Name, rModel.Length, checkCountries.Item2));

            return new RiverViewModel
            {
                Name = rModel.Name,
                RiverID = newID.ToString(),
                Length = rModel.Length
            };
        }

        public RiverViewModel UpdateRiver(RiverModel rModel)
        {
            if (rModel.RiverID <= 0)
            {
                throw new RiverException($"UpdateRiver: {rModel.RiverID} is an invalid ID.");
            }

            var river = _riverRepo.Get(rModel.RiverID);

            if (river == null)
            {
                throw new RiverException($"UpdateRiver: No river found with ID: {rModel.RiverID}.");
            }

            var checkCountries = CheckCountries(rModel.Countries);

            if (!string.IsNullOrEmpty(checkCountries.Item1) && checkCountries.Item2 == null)
            {
                throw new RiverException($"UpdateRiver: {checkCountries.Item1}");
            }

            river.Name = rModel.Name;
            river.Length = rModel.Length;
            river.Countries = checkCountries.Item2;

            _riverRepo.Update(river);

            return new RiverViewModel
            {
                Name = river.Name,
                Length = river.Length,
                RiverID = river.RiverID.ToString()
            };
        }

        public void RemoveRiver(int id)
        {
            if (id <= 0)
            {
                throw new RiverException($"RemoveRiver: {id} is an invalid ID.");
            }

            var river = _riverRepo.Get(id);

            if (river == null)
            {
                throw new RiverException("RemoveRiver: river doesn't exists.");
            }

            _riverRepo.Remove(id);
        }

        public (string, IList<Country>) CheckCountries(IList<int> countryIds)
        {
            string errorMessage;

            if (countryIds == null || countryIds.Count == 0)
            {
                errorMessage = "A river needs to be located in at least 1 country.";

                return (errorMessage, null);
            }

            bool allCountriesExist = true;
            IList<Country> countries = new List<Country>();

            foreach (var countryID in countryIds)
            {
                var country = _countryRepo.Get(countryID);

                if (country == null)
                {
                    allCountriesExist = false;
                }
                else
                {
                    countries.Add(country);
                }
            }

            if (!allCountriesExist)
            {
                errorMessage = "Not all given countries exist.";

                return (errorMessage, null);
            }

            return (null, countries);
        }
    }
}
