using System.Collections.Generic;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Continent;
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
            throw new System.NotImplementedException();
        }

        public ContinentViewModel GetContinent()
        {
            throw new System.NotImplementedException();
        }
    }
}
