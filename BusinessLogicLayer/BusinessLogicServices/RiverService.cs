using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.River;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class RiverService: IRiverService
    {
        private readonly RiverRepo _riverRepo;
        private readonly CountryRepo _countryRepo;

        public RiverService(RiverRepo riverRepo, CountryRepo countryRepo)
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

            var countries = _countryRepo.GetAll(rModel.Countries).ToList();

            var newID = _riverRepo.Add(new River(rModel.Name, rModel.Length, countries));

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

            if (_riverRepo.Exists(rModel.Name))
            {
                throw new RiverException("UpdateRiver: river already exists.");
            }

            var river = _riverRepo.Get(rModel.RiverID);

            if (river == null)
            {
                throw new RiverException($"UpdateRiver: No river found with ID: {rModel.RiverID}.");
            }

            var countries = _countryRepo.GetAll(rModel.Countries).ToList();

            river.Name = rModel.Name;
            river.Length = rModel.Length;
            river.Countries = countries;

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
    }
}
