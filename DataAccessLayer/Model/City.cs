using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    [Table("City")]
    public class City
    {
        public City() { }

        public City(string name, int population, bool isCapital, Country country)
        {
            Name = name;
            Population = population;
            IsCapital = isCapital;
            Country = country;
        }

        [Key]
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
        public Country Country { get; set; }
    }
}
