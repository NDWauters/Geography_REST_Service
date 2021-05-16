using System.Collections.Generic;
using BusinessLogicLayer.BusinessLogicServices;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.River;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Geography_REST_Service.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Geography_REST_Service_Tests
{
    public class RiverControllerTests
    {
        #region FIELDS

        private readonly Mock<IContinentRepo> _continentMockRepo;
        private readonly Mock<ICountryRepo> _countryMockRepo;
        private readonly Mock<ICityRepo> _cityMockRepo;
        private readonly Mock<IRiverRepo> _riverMockRepo;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly Mock<ILogger<RiverController>> _logger;
        private readonly ICountryService _countryService;
        private readonly IRiverService _riverService;
        private readonly RiverController _riverController;

        #endregion

        #region CTOR

        public RiverControllerTests()
        {
            _continentMockRepo = new Mock<IContinentRepo>();
            _countryMockRepo = new Mock<ICountryRepo>();
            _cityMockRepo = new Mock<ICityRepo>();
            _riverMockRepo = new Mock<IRiverRepo>();
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _logger = new Mock<ILogger<RiverController>>();

            _countryService = new CountryService(
                _countryMockRepo.Object,
                _continentMockRepo.Object,
                _riverMockRepo.Object,
                _cityMockRepo.Object);
            _riverService = new RiverService(_riverMockRepo.Object, _countryMockRepo.Object);

            _riverController = new RiverController(_logger.Object, _riverService, _countryService)
            {
                Url = _mockUrlHelper.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        #endregion

        #region GET

        [Fact]
        public void River_GET_GetAllRivers_ReturnsIEnumerableRiverViewModel()
        {
            IEnumerable<River> rivers = new List<River>();
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _riverMockRepo.Setup(repo => repo.GetAll(null))
                .Returns(rivers);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _riverController.GetAllRivers();

            Assert.IsType<List<RiverViewModel>>(result);
        }

        [Fact]
        public void River_GET_Get_InvalidID_ReturnsNotFound()
        {
            _riverMockRepo.Setup(repo => repo.Get(2))
                .Throws(new RiverException("River doesn't exist."));

            var result = _riverController.GetRiver(2);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void River_GET_Get_CorrectID_ReturnsOkObject()
        {
            var countries = new List<Country>();

            _riverMockRepo.Setup(repo => repo.Get(2))
                .Returns(new River("Schelde", 100, countries));

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(new Country("België", 11000000, 30700, It.IsAny<Continent>()));

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _riverController.GetRiver(2);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void River_GET_Get_CorrectID_ReturnsRiverViewModel()
        {
            var country = new Country("België", 11000000, 30700, new Continent()) { CountryID = 2 };

            var countries = new List<Country>() { country };

            var river = new River("Schelde", 100, countries)
            {
                RiverID = 2
            };

            _riverMockRepo.Setup(repo => repo.Get(2))
                .Returns(river);

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/River/2")
                .Verifiable();

            var result = _riverController.GetRiver(2).Result as OkObjectResult;

            Assert.IsType<RiverViewModel>(result?.Value);
            Assert.Equal("https://localhost:5001/api/River/2", (result.Value as RiverViewModel)?.RiverID);
            Assert.Equal(river.Name, (result.Value as RiverViewModel)?.Name);
            Assert.Equal(river.Length, (result.Value as RiverViewModel)?.Length);
            Assert.Equal($"https://localhost:5001/api/River/{river.RiverID}", (result.Value as RiverViewModel)?.RiverID);
        }

        #endregion

        #region POST

        [Fact]
        public void River_POST_ValidObject_ReturnsCreatedAtAction()
        {
            var river = new RiverModel
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/River/2")
                .Verifiable();

            var response = _riverController.Post(river);

            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact]
        public void River_POST_ValidObject_ReturnsCorrectItem()
        {
            var river = new RiverModel
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/River/2")
                .Verifiable();

            var response = _riverController.Post(river).Result as CreatedAtActionResult;
            var item = response?.Value as RiverViewModel;

            Assert.IsType<RiverViewModel>(item);
            Assert.Equal("https://localhost:5001/api/River/2", item.RiverID);
            Assert.Equal(river.Name, item.Name);
            Assert.Equal(river.Length, item.Length);
            Assert.Equal($"https://localhost:5001/api/River/{river.RiverID}", item.RiverID);
        }

        [Fact]
        public void River_POST_InvalidObject_ReturnsBadRequest()
        {
            var river = new RiverModel
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _riverController.ModelState.AddModelError("format error", "Int expected");
            var response = _riverController.Post(river).Result;

            Assert.IsType<BadRequestObjectResult>(response);
        }

        #endregion

        #region PUT

        [Fact]
        public void River_PUT_NotSameIDs_ReturnsBadRequest()
        {
            var riverModel = new RiverModel
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>());
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());
            var river = new River("Schelde", 100, It.IsAny<IList<Country>>());

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _riverMockRepo.Setup(x => x.Get(2))
                .Returns(river);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _riverController.Put(3, riverModel);

            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public void River_PUT_InvalidObject_ReturnsBadRequest()
        {
            var riverModel = new RiverModel
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>());
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());
            var river = new River("Schelde", 100, It.IsAny<IList<Country>>());

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _riverMockRepo.Setup(x => x.Get(2))
                .Returns(river);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _riverController.ModelState.AddModelError("format error", "int expected");
            var response = _riverController.Put(2, riverModel);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void River_PUT_SameIDs_ValidObject_ReturnsCreatedAtAction()
        {
            var riverModel = new RiverModel
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>());
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());
            var river = new River("Schelde", 100, It.IsAny<IList<Country>>());

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _riverMockRepo.Setup(x => x.Get(2))
                .Returns(river);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _riverController.Put(2, riverModel);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        #endregion

        #region DELETE

        [Fact]
        public void River_DELETE_UnknownID_ReturnsBadRequest()
        {
            var response = _riverController.DeleteRiver(3);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void River_DELETE_KnownID_ReturnsNoContent()
        {
            var river = new River
            {
                RiverID = 2,
                Name = "Schelde",
                Length = 100,
                Countries = new List<Country>()
            };

            _riverMockRepo.Setup(repo => repo.Get(2))
                .Returns(river);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _riverController.DeleteRiver(2);

            Assert.IsType<NoContentResult>(response);
        }

        #endregion
    }
}
