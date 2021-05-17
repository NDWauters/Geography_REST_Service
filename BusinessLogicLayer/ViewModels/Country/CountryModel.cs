using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Country
{
    public class CountryModel
    {
        public int CountryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Population { get; set; }

        [Required]
        public double Surface { get; set; }

        [Required]
        public int ContinentID { get; set; }

        public IList<int> Cities { get; set; }
        public IList<int> Rivers { get; set; }
    }
}
