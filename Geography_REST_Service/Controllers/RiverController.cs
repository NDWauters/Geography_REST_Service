using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Country;
using BusinessLogicLayer.ViewModels.River;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Geography_REST_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiverController : Controller
    {
        private readonly ILogger<RiverController> _logger;
        private readonly IRiverService _riverService;
        private readonly ICountryService _countryService;

        public RiverController(ILogger<RiverController> logger,
            IRiverService riverService,
            ICountryService countryService)
        {
            _logger = logger;
            _riverService = riverService;
            _countryService = countryService;
        }

        [HttpGet]
        public IEnumerable<RiverViewModel> GetAllRivers()
        {
            var rivers = _riverService.GetAllRivers().ToList();

            foreach (var river in rivers)
            {
                river.RiverID = Url.Action("GetRiver", "River", 
                    new {id = river.RiverID}, 
                    Request.Scheme);

                var countryList = new List<CountryViewModel>();

                foreach (var countryID in river.Countries)
                {
                    var countryObject = _countryService.GetCountry(Convert.ToInt32(countryID));

                    countryList.Add(countryObject);
                }

                river.Countries = countryList
                    .Select(x => Url.Action("GetCountry", "Continent",
                        new { x.ContinentID, x.CountryID },
                        Request.Scheme))
                    .ToList();
            }

            return rivers;
        }

        [HttpGet("{id}")]
        public ActionResult<RiverViewModel> GetRiver(int id)
        {
            try
            {
                var river = _riverService.GetRiver(id);

                river.RiverID = Url.Action("GetRiver", "River",
                    new { id = river.RiverID },
                    Request.Scheme);

                var countryList = new List<CountryViewModel>();

                foreach (var countryID in river.Countries)
                {
                    var countryObject = _countryService.GetCountry(Convert.ToInt32(countryID));

                    countryList.Add(countryObject);
                }

                river.Countries = countryList
                    .Select(x => Url.Action("GetCountry", "Continent", 
                        new { x.ContinentID, x.CountryID },
                        Request.Scheme))
                    .ToList();

                return river;
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpPost]
        public ActionResult<RiverViewModel> Post([FromBody] RiverModel river)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newRiver = _riverService.CreateRiver(river);

                var newID = Convert.ToInt32(newRiver.RiverID);

                newRiver.RiverID = Url.Action("GetRiver", "River", new { id = newID }, Request.Scheme);

                var countryList = new List<CountryViewModel>();

                foreach (var countryID in river.Countries)
                {
                    var countryObject = _countryService.GetCountry(Convert.ToInt32(countryID));

                    countryList.Add(countryObject);
                }

                newRiver.Countries = countryList
                    .Select(x => Url.Action("GetCountry", "Continent",
                        new { x.ContinentID, x.CountryID },
                        Request.Scheme))
                    .ToList();

                return CreatedAtAction(nameof(GetRiver), new { id = newID }, newRiver);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RiverModel river)
        {
            try
            {
                if (river.RiverID != id)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedRiver = _riverService.UpdateRiver(river);

                updatedRiver.RiverID = Url.Action("GetRiver", "River", new { id }, Request.Scheme);

                var countryList = new List<CountryViewModel>();

                foreach (var countryID in river.Countries)
                {
                    var countryObject = _countryService.GetCountry(countryID);

                    countryList.Add(countryObject);
                }

                updatedRiver.Countries = countryList
                    .Select(x => Url.Action("GetCountry", "Continent",
                        new { x.ContinentID, x.CountryID },
                        Request.Scheme))
                    .ToList();

                return CreatedAtAction(nameof(GetRiver), new { id }, updatedRiver);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRiver(int id)
        {
            try
            {
                _riverService.RemoveRiver(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
