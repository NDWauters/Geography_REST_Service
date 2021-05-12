using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    [Table("Country")]
    public class Country
    {
        public Country() { }

        public Country(string name, int population, double surface)
        {
            Name = name;
            Population = population;
            Surface = surface;
            Cities = new HashSet<City>();
            Rivers = new HashSet<River>();
        }

        public Country(string name, int population, double surface, Continent continent, IList<City> cities, IList<River> rivers)
        {
            Name = name;
            Population = population;
            Surface = surface;
            Cities = cities;
            Rivers = rivers;
            Continent = continent;
        }

        [Key] public int CountryID { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }

        [Required] 
        public int Population { get; set; }
        [Required] 
        public double Surface { get; set; }
        [Required] 
        public int ContinentID { get; set; }
        public Continent Continent { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<River> Rivers { get; set; }
    }
}
