using System.Collections.Generic;

namespace BusinessLogicLayer.ViewModels.River
{
    public class RiverViewModel
    {
        public string RiverID { get; set; }
        
        public string Name { get; set; }

        public double Length { get; set; }

        public IList<string> Countries { get; set; } = new List<string>();
    }
}
