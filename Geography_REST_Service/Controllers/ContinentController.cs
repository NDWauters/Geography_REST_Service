using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.City;
using BusinessLogicLayer.ViewModels.Continent;
using BusinessLogicLayer.ViewModels.Country;

namespace Geography_REST_Service.Controllers
{
    //NOG BEKIJKEN: CONTROLE OP UNIEK ZIJN IN CREATE EN UPDATE
    [ApiController]
    [Route("api/[controller]")]
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
            _logger.LogInformation(1,
                $"{DateTime.Now.ToShortDateString()} - GET - " +
                $"{Url.Action("GetAllContinents", "Continent", new {}, Request.Scheme)}");

            var continents = _continentService.GetAllContinents().ToList();

            foreach (var continent in continents)
            {
                continent.Countries = continent.Countries
                    .Select(x => Url.Action("GetCountry", "Continent", 
                        new { continent.ContinentID, countryID = x }, 
                        Request.Scheme))
                    .ToList();

                continent.ContinentID = Url.Action("GetContinent", "Continent", 
                    new { id = continent.ContinentID }, 
                    Request.Scheme);
            }

            return continents;
        }

        [HttpGet("{id}")]
        public ActionResult<ContinentViewModel> GetContinent(int id)
        {
            try
            {
                _logger.LogInformation(2,
                    $"{DateTime.Now.ToShortDateString()} - GET - " +
                    $"{Url.Action("GetContinent", "Continent", new { id }, Request.Scheme)}");

                var continent = _continentService.GetContinent(id);

                continent.ContinentID = Url.Action("GetContinent", "Continent", 
                    new { id }, 
                    Request.Scheme);

                continent.Countries = continent.Countries
                    .Select(x => Url.Action("GetCountry", "Continent", 
                        new { continentID = id, countryID = x },
                        Request.Scheme))
                    .ToList();

                return Ok(continent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return NotFound(e.Message);
            }
            
        }

        [HttpPost]
        public ActionResult<ContinentModel> PostContinent([FromBody] ContinentModel continent)
        {
            try
            {
                _logger.LogInformation(3,
                    $"{DateTime.Now.ToShortDateString()} - POST - " +
                    $"{Url.Action("PostContinent", "Continent", new {}, Request.Scheme)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newContinent = _continentService.CreateContinent(continent);

                var newID = Convert.ToInt32(newContinent.ContinentID);

                newContinent.ContinentID = Url.Action("GetContinent", "Continent", 
                    new { id = newID },
                    Request.Scheme);

                newContinent.Countries = continent.Countries
                    .Select(x => Url.Action("GetCountry", "Continent",
                        new {continentID = newID, countryID = x},
                        Request.Scheme))
                    .ToList();

                return CreatedAtAction(nameof(GetContinent), new { id = newID }, newContinent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutContinent(int id, [FromBody] ContinentModel continent)
        {
            try
            {
                _logger.LogInformation(4,
                    $"{DateTime.Now.ToShortDateString()} - PUT - " +
                    $"{Url.Action("PutContinent", "Continent", new { id }, Request.Scheme)}");

                if (continent.ContinentID != id)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedContinent = _continentService.UpdateContinent(continent);

                updatedContinent.ContinentID = Url.Action("GetContinent", "Continent",
                    new { id }, 
                    Request.Scheme);

                updatedContinent.Countries = updatedContinent.Countries
                    .Select(x => Url.Action("GetCountry", "Continent",
                        new { continentID = id, countryID = x },
                        Request.Scheme))
                    .ToList();

                return CreatedAtAction(nameof(GetContinent), new { id }, updatedContinent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContinent(int id)
        {
            try
            {
                _logger.LogInformation(5,
                    $"{DateTime.Now.ToShortDateString()} - DELETE - " +
                    $"{Url.Action("DeleteContinent", "Continent", new { id }, Request.Scheme)}");

                _continentService.RemoveContinent(id);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        #endregion

        #region COUNTRY
        
        [HttpGet("{continentID}/Country")]
        public IEnumerable<CountryViewModel> GetAllCountries()
        {
            _logger.LogInformation(1,
                $"{DateTime.Now.ToShortDateString()} - GET - " +
                $"{Url.Action("GetAllCountries", "Continent", new {}, Request.Scheme)}");

            var countries = _countryService.GetAllCountries().ToList();

            foreach (var country in countries)
            {
                country.Cities = country.Cities
                    .Select(x
                        => Url.Action("GetCity", "Continent",
                            new { country.ContinentID, country.CountryID, cityID = x },
                            Request.Scheme))
                    .ToList();

                country.CountryID = Url.Action("GetCountry", "Continent", 
                    new { country.ContinentID, country.CountryID},
                    Request.Scheme);

                country.Rivers = country.Rivers
                    .Select(x => Url.Action("GetRiver", "River",
                        new { id = x },
                        Request.Scheme))
                    .ToList();
            }

            return countries;
        }
        
        [HttpGet("{continentID}/Country/{countryID}")]
        public ActionResult<CountryViewModel> GetCountry(int countryID)
        {
            try
            {
                _logger.LogInformation(2,
                    $"{DateTime.Now.ToShortDateString()} - GET - " +
                    $"{Url.Action("GetCountry", "Continent", new { countryID }, Request.Scheme)}");

                var country = _countryService.GetCountry(countryID);

                country.CountryID = Url.Action("GetCountry", "Continent", 
                    new { country.ContinentID, country.CountryID }, 
                    Request.Scheme);

                country.Cities = country.Cities
                    .Select(x => Url.Action("GetCity", "Continent", 
                        new { country.ContinentID, countryID, cityID = x }, 
                        Request.Scheme))
                    .ToList();

                country.Rivers = country.Rivers
                    .Select(x => Url.Action("GetRiver", "River",
                        new { id = x },
                        Request.Scheme))
                    .ToList();

                return Ok(country);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return NotFound(e.Message);
            }
        }
        
        [HttpPost("{continentID}/Country")]
        public ActionResult<CountryViewModel> PostCountry(int continentID, [FromBody] CountryModel country)
        {
            try
            {
                _logger.LogInformation(3,
                    $"{DateTime.Now.ToShortDateString()} - POST - " +
                    $"{Url.Action("PostCountry", "Continent", new { continentID }, Request.Scheme)}");

                if (continentID != country.ContinentID)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newCountry = _countryService.CreateCountry(country);

                var newID = Convert.ToInt32(newCountry.CountryID);

                newCountry.CountryID = Url.Action("GetCountry", "Continent", 
                    new { continentID, countryID = newID },
                    Request.Scheme);

                newCountry.Cities = country.Cities.Select(x => Url.Action("GetCity", "Continent",
                    new {continentID, countryID = newID, cityID = x},
                    Request.Scheme))
                    .ToList();

                newCountry.Rivers = country.Rivers.Select(x => Url.Action("GetRiver", "River",
                        new { id = x },
                        Request.Scheme))
                    .ToList();

                return CreatedAtAction(nameof(GetCountry), new { continentID, countryID = newID }, newCountry);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{continentID}/Country/{countryID}")]
        public IActionResult PutCountry(int continentID, int countryID, [FromBody] CountryModel country)
        {
            try
            {
                _logger.LogInformation(4,
                    $"{DateTime.Now.ToShortDateString()} - PUT - " +
                    $"{Url.Action("PutCountry", "Continent", new { continentID, countryID }, Request.Scheme)}");

                if (country.CountryID != countryID)
                {
                    return BadRequest();
                }

                if (country.ContinentID != continentID)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedCountry = _countryService.UpdateCountry(country);

                updatedCountry.CountryID = Url.Action("GetCountry", "Continent", new { continentID, countryID });

                updatedCountry.Cities = country.Cities.Select(x => Url.Action("GetCity", "Continent",
                        new { continentID, countryID, cityID = x },
                        Request.Scheme))
                    .ToList();

                updatedCountry.Rivers = country.Rivers.Select(x => Url.Action("GetRiver", "River",
                        new { id = x },
                        Request.Scheme))
                    .ToList();

                return CreatedAtAction(nameof(GetCountry), new { continentID, countryID }, updatedCountry);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("{continentID}/Country/{countryID}")]
        public IActionResult DeleteCountry(int countryID)
        {
            try
            {
                _logger.LogInformation(5,
                    $"{DateTime.Now.ToShortDateString()} - DELETE - " +
                    $"{Url.Action("DeleteCountry", "Continent", new { countryID }, Request.Scheme)}");

                _countryService.RemoveCountry(countryID);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        #endregion

        #region CITY

        [HttpGet("{continentID}/Country/{countryID}/City")]
        public IEnumerable<CityViewModel> GetAllCities(int continentID)
        {
            _logger.LogInformation(1,
                $"{DateTime.Now.ToShortDateString()} - GET - " +
                $"{Url.Action("GetAllCities", "Continent", new { }, Request.Scheme)}");

            var cities = _cityService.GetAllCities().ToList();

            foreach (var city in cities)
            {
                city.CityID = Url.Action("GetCity", "Continent",
                    new { continentID, city.CountryID, city.CityID },
                    Request.Scheme);

                city.CountryID = Url.Action("GetCountry", "Continent",
                    new { continentID, city.CountryID },
                    Request.Scheme);
            }

            return cities;
        }

        [HttpGet("{continentID}/Country/{countryID}/City/{cityID}")]
        public ActionResult<CityViewModel> GetCity(int continentID, int countryID, int cityID)
        {
            try
            {
                _logger.LogInformation(2,
                    $"{DateTime.Now.ToShortDateString()} - GET - " +
                    $"{Url.Action("GetCity", "Continent", new { continentID, countryID, cityID }, Request.Scheme)}");

                var city = _cityService.GetCity(countryID, cityID);

                city.CityID = Url.Action("GetCity", "Continent",
                    new { countryID, city.CityID },
                    Request.Scheme);

                city.CountryID = Url.Action("GetCountry", "Continent",
                    new { continentID, city.CountryID },
                    Request.Scheme);

                return city;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return NotFound(e.Message);
            }
            
        }
        
        [HttpPost("{continentID}/Country/{countryID}/City")]
        public ActionResult<CityModel> PostCity(int continentID, int countryID, [FromBody] CityModel city)
        {
            try
            {
                _logger.LogInformation(3,
                    $"{DateTime.Now.ToShortDateString()} - POST - " +
                    $"{Url.Action("PostCity", "Continent", new { continentID, countryID }, Request.Scheme)}");

                if (countryID != city.CountryID)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newCity = _cityService.CreateCity(city);

                var newID = Convert.ToInt32(newCity.CityID);

                newCity.CityID = Url.Action("GetCity", "Continent", 
                    new { continentID, countryID, cityID = newID }, 
                    Request.Scheme);

                newCity.CountryID = Url.Action("GetCountry", "Continent",
                    new { continentID, countryID },
                    Request.Scheme);

                return CreatedAtAction(nameof(GetCity), new { continentID, countryID, cityID = newID }, newCity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{continentID}/Country/{countryID}/City/{cityID}")]
        public IActionResult PutCity(int continentID, int countryID, int cityID, [FromBody] CityModel city)
        {
            try
            {
                _logger.LogInformation(4,
                    $"{DateTime.Now.ToShortDateString()} - PUT - " +
                    $"{Url.Action("PutCity", "Continent", new { continentID, countryID, cityID }, Request.Scheme)}");

                if (city.CityID != cityID)
                {
                    return BadRequest();
                }

                if (city.CountryID != countryID)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedCity = _cityService.UpdateCity(city);

                updatedCity.CityID = Url.Action("GetCity", "Continent", 
                    new { continentID, city.CountryID, city.CityID },
                    Request.Scheme);

                updatedCity.CountryID = Url.Action("GetCountry", "Continent",
                    new { continentID, city.CountryID },
                    Request.Scheme);

                return CreatedAtAction(nameof(GetCity), new { continentID, city.CountryID, city.CityID }, updatedCity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("{continentID}/Country/{countryID}/City/{cityID}")]
        public IActionResult DeleteCity(int cityID)
        {
            try
            {
                _logger.LogInformation(5,
                    $"{DateTime.Now.ToShortDateString()} - DELETE - " +
                    $"{Url.Action("DeleteCity", "Continent", new { cityID }, Request.Scheme)}");

                _cityService.RemoveCity(cityID);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        #endregion
    }
}