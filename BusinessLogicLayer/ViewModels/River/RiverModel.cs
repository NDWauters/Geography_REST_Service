using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.River
{
    public class RiverModel
    {
        public int RiverID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public double Length { get; set; }

        [Required]
        public IList<int> Countries { get; set; }
    }
}
