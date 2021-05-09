using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    [Table("River")]
    public class River
    {
        public River() { }

        public River(string name, double length, IList<Country> countries)
        {
            Name = name;
            Length = length;
            Countries = countries;
        }

        [Key]
        public int RiverID { get; set; }
        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }
        public double Length { get; set; }
        public ICollection<Country> Countries { get; set; }
    }
}
