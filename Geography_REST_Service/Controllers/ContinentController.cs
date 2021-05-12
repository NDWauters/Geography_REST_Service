using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.City;
using BusinessLogicLayer.ViewModels.Continent;
using BusinessLogicLayer.ViewModels.Country;

namespace Geography_REST_Service.Controllers
{
    [ApiController]
    [Route("[api]/[controller]")]
    public class ContinentController : ControllerBase
    {
        private readonly ILogger<ContinentController> _logger;
        private readonly IContinentService _continentService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public ContinentController(
            ILogger<ContinentController> logger, 
            IContinentService continentService,
            ICountryService countryService,
            ICityService cityService)
        {
            _logger = logger;
            _continentService = continentService;
            _countryService = countryService;
            _cityService = cityService;
        }

        #region CONTINENT

        [HttpGet]
        public IEnumerable<ContinentViewModel> GetAllContinents()
        {
            return null;
        }

        [HttpGet("{id}")]
        public ActionResult<ContinentViewModel> GetContinent(int id)
        {
            return null;
        }

        [HttpPost]
        public ActionResult<ContinentModel> Post([FromBody] ContinentModel continent)
        {
            return null;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ContinentModel continent)
        {
            return null;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContinent(int id)
        {
            return null;
        }

        #endregion

        #region COUNTRY
        
        [HttpGet("api/[controller]/{continentID}/Country")]
        public IEnumerable<CountryViewModel> GetAllCountries()
        {
            return null;
        }
        
        [HttpGet("api/[controller]/{continentID}/Country/{countryID}")]
        public ActionResult<CountryViewModel> GetCountry(int countryID)
        {
            return null;
        }
        
        [HttpPost("api/[controller]/{continentID}/Country")]
        public ActionResult<CountryModel> Post([FromBody] CountryModel country)
        {
            return null;
        }
        
        [HttpPut("api/[controller]/{continentID}/Country/{countryID}")]
        public IActionResult Put(int id, [FromBody] CountryModel country)
        {
            return null;
        }
        
        [HttpDelete("api/[controller]/{continentID}/Country/{countryID}")]
        public IActionResult DeleteCountry(int countryID)
        {
            return null;
        }

        #endregion

        #region CITY

        [HttpGet("api/[controller]/{continentID}/Country/{countryID}/City")]
        public IEnumerable<CityViewModel> GetAllCities()
        {
            return null;
        }

        [HttpGet("api/[controller]/{continentID}/Country/{countryID}/City/{cityID}")]
        public ActionResult<CityViewModel> GetCity(int cityID)
        {
            return null;
        }
        
        [HttpPost("api/[controller]/{ContinentID}/Country")]
        public ActionResult<CityModel> Post([FromBody] CityModel city)
        {
            return null;
        }
        
        [HttpPut("api/[controller]/{continentID}/Country/{countryID}/City/{cityID}")]
        public IActionResult Put(int id, [FromBody] CityModel city)
        {
            return null;
        }
        
        [HttpDelete("api/[controller]/{continentID}/Country/{countryID}/City/{cityID}")]
        public IActionResult DeleteCity(int cityID)
        {
            return null;
        }

        #endregion
    }
}
