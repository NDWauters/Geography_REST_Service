using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Continent
{
    public class ContinentModel
    {
        public int ContinentID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IList<int> Countries { get; set; } = new List<int>();
    }
}
