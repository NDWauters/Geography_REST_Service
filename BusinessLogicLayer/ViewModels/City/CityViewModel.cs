using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.City
{
    public class CityViewModel
    {
        public string CityID { get; set; }

       
        public string Name { get; set; }
        
        public int Population { get; set; }
        
        public bool IsCapital { get; set; }
        
        public string CountryID { get; set; }
    }
}
