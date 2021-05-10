using System.Collections.Generic;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.City;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class CityService : ICityService
    {
        private readonly CityRepo _cityRepo;

        public CityService(CityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }

        public IEnumerable<CityViewModel> GetAllRivers()
        {
            throw new System.NotImplementedException();
        }

        public CityViewModel GetRiver()
        {
            throw new System.NotImplementedException();
        }
    }
}
