using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.River;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRiverService
    {
        IEnumerable<RiverViewModel> GetAllRivers();
        RiverViewModel GetRiver();
    }
}