using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccessLayer.Model
{
    [Table("Continent")]
    public class Continent
    {
        public Continent() { }

        public Continent(string name, ICollection<Country> countries)
        {
            Name = name;
            Countries = countries;
        }

        [Key] public int ContinentID { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }

        public int Population
        {
            get => Countries.Sum(x => x.Population);
        }

        public ICollection<Country> Countries { get; set; }
    }
}
