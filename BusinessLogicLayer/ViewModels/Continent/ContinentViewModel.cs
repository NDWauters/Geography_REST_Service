using System.Collections.Generic;

namespace BusinessLogicLayer.ViewModels.Continent
{
    public class ContinentViewModel
    {
        public string ContinentID { get; set; }
        
        public string Name { get; set; }

        public int Population { get; set; }

        public IList<string> Countries { get; set; } = new List<string>();
    }
}
