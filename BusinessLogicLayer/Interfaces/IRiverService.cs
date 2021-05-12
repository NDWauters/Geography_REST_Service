using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.River;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRiverService
    {
        IEnumerable<RiverViewModel> GetAllRivers();
        RiverViewModel GetRiver(int id);
        RiverViewModel CreateRiver(RiverModel rModel);
        RiverViewModel UpdateRiver(RiverModel rModel);
        void RemoveRiver(int id);
    }
}