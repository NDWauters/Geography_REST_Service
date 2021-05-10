using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.City
{
    public class CityModel
    {
        public int CityID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Population { get; set; }

        [Required]
        public bool IsCapital { get; set; }

        [Required]
        public int CountryID { get; set; }
    }
}
