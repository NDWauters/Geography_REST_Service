using System.Collections.Generic;
using BusinessLogicLayer.ViewModels.River;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Geography_REST_Service.Controllers
{
    [ApiController]
    [Route("[api]/[controller]")]
    public class RiverController : Controller
    {
        private readonly ILogger<RiverController> _logger;

        public RiverController(ILogger<RiverController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IEnumerable<RiverViewModel> GetAllContinents()
        {
            return null;
        }

        [HttpGet("{id}")]
        public ActionResult<RiverViewModel> GetContinent(int id)
        {
            return null;
        }

        [HttpPost]
        public ActionResult<RiverModel> Post([FromBody] RiverModel continent)
        {
            return null;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RiverModel continent)
        {
            return null;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContinent(int id)
        {
            return null;
        }
    }
}
