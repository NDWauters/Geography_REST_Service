using System.Collections.Generic;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.River;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.BusinessLogicServices
{
    public class RiverService: IRiverService
    {
        private readonly RiverRepo _riverRepo;

        public RiverService(RiverRepo riverRepo)
        {
            _riverRepo = riverRepo;
        }

        public IEnumerable<RiverViewModel> GetAllRivers()
        {
            throw new System.NotImplementedException();
        }

        public RiverViewModel GetRiver()
        {
            throw new System.NotImplementedException();
        }
    }
}
