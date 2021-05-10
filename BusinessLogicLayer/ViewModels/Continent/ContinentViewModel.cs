using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Continent
{
    public class ContinentViewModel
    {
        public string ContinentID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        private IList<string> Countries { get; set; }
    }
}
