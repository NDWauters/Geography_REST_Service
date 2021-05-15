using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Country
{
    public class CountryViewModel
    {
        public string CountryID { get; set; }
        
        public string Name { get; set; }
        
        public int Population { get; set; }
        
        public double Surface { get; set; }
        
        public string ContinentID { get; set; }

        public IList<string> Cities { get; set; } = new List<string>();
        public IList<string> Rivers { get; set; } = new List<string>();
    }
}
