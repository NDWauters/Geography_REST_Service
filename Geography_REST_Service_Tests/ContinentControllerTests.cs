using System.Collections.Generic;
using BusinessLogicLayer.BusinessLogicServices;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.City;
using BusinessLogicLayer.ViewModels.Continent;
using BusinessLogicLayer.ViewModels.Country;
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
    public class ContinentControllerTests
    {
        #region FIELDS

        private readonly Mock<IContinentRepo> _continentMockRepo;
        private readonly Mock<ICountryRepo> _countryMockRepo;
        private readonly Mock<ICityRepo> _cityMockRepo;
        private readonly Mock<IRiverRepo> _riverMockRepo;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly Mock<ILogger<ContinentController>> _logger;
        private readonly IContinentService _continentService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IRiverService _riverService;
        private readonly ContinentController _continentController;

        #endregion

        #region CTOR

        public ContinentControllerTests()
        {
            _continentMockRepo = new Mock<IContinentRepo>();
            _countryMockRepo = new Mock<ICountryRepo>();
            _cityMockRepo = new Mock<ICityRepo>();
            _riverMockRepo = new Mock<IRiverRepo>();
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _logger = new Mock<ILogger<ContinentController>>();

            _continentService = new ContinentService(_continentMockRepo.Object, _countryMockRepo.Object);
            _countryService = new CountryService(
                _countryMockRepo.Object,
                _continentMockRepo.Object,
                _riverMockRepo.Object,
                _cityMockRepo.Object);
            _cityService = new CityService(
                _cityMockRepo.Object,
                _countryMockRepo.Object,
                _continentMockRepo.Object);
            _riverService = new RiverService(_riverMockRepo.Object, _countryMockRepo.Object);

            _continentController = new ContinentController(_logger.Object, _continentService, _countryService, _cityService)
            {
                Url = _mockUrlHelper.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        #endregion

        #region CONTINENT

        #region GET

        [Fact]
        public void Continent_GET_GetAllContinents_ReturnsIEnumerableContinentViewModel()
        {
            IEnumerable<Continent> continents = new List<Continent>();

            _continentMockRepo.Setup(repo => repo.GetAll(null))
                .Returns(continents);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _continentController.GetAllContinents();

            Assert.IsType<List<ContinentViewModel>>(result);
        }

        [Fact]
        public void Continent_GET_Get_InvalidID_ReturnsNotFound()
        {
            _continentMockRepo.Setup(repo => repo.Get(2))
                .Throws(new ContinentException("Continent doesn't exist."));

            var result = _continentController.GetContinent(2);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void Continent_GET_Get_CorrectID_ReturnsOkObject()
        {
            var countries = new List<Country>();

            _continentMockRepo.Setup(repo => repo.Get(2))
                .Returns(new Continent("Europa", countries));

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _continentController.GetContinent(2);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Continent_GET_Get_CorrectID_ReturnsContinentViewModel()
        {
            var countries = new List<Country>()
            {
                new Country("België", 11000000, 30700, new Continent())
            };

            var continent = new Continent("Europa", countries)
            {
                ContinentID = 2
            };

            _continentMockRepo.Setup(repo => repo.Get(2))
                .Returns(continent);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2")
                .Verifiable();

            var result = _continentController.GetContinent(2).Result as OkObjectResult;

            Assert.IsType<ContinentViewModel>(result?.Value);
            Assert.Equal("https://localhost:5001/api/Continent/2", (result.Value as ContinentViewModel)?.ContinentID);
            Assert.Equal(continent.Name, (result.Value as ContinentViewModel)?.Name);
            Assert.Equal(continent.Population, (result.Value as ContinentViewModel)?.Population);
            Assert.Equal($"https://localhost:5001/api/Continent/{continent.ContinentID}", (result.Value as ContinentViewModel)?.ContinentID);
        }

        #endregion

        #region POST

        [Fact]
        public void Continent_POST_ValidObject_ReturnsCreatedAtAction()
        {
            var continent = new ContinentModel
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _countryMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<Country>() { country });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2")
                .Verifiable();

            var response = _continentController.PostContinent(continent);

            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact]
        public void Continent_POST_ValidObject_ReturnsCorrectItem()
        {
            var continent = new ContinentModel
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _countryMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<Country>() { country });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2")
                .Verifiable();

            var response = _continentController.PostContinent(continent).Result as CreatedAtActionResult;
            var item = response?.Value as ContinentViewModel;

            Assert.IsType<ContinentViewModel>(item);
            Assert.Equal("https://localhost:5001/api/Continent/2", item.ContinentID);
            Assert.Equal(continent.Name, item.Name);
            Assert.Equal($"https://localhost:5001/api/Continent/{continent.ContinentID}", item.ContinentID);
        }

        [Fact]
        public void Continent_POST_InvalidObject_ReturnsBadRequest()
        {
            var continent = new ContinentModel
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _countryMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<Country>() { country });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _continentController.ModelState.AddModelError("format error", "Int expected");
            var response = _continentController.PostContinent(continent).Result;

            Assert.IsType<BadRequestObjectResult>(response);
        }

        #endregion

        #region PUT

        [Fact]
        public void Continent_PUT_NotSameIDs_ReturnsBadRequest()
        {
            var continentModel = new ContinentModel
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>());
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _countryMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<Country>() { country });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.PutContinent(3, continentModel);

            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public void Continent_PUT_InvalidObject_ReturnsBadRequest()
        {
            var continentModel = new ContinentModel
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>());
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _countryMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<Country>() { country });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _continentController.ModelState.AddModelError("format error", "int expected");
            var response = _continentController.PutContinent(2, continentModel);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Continent_PUT_SameIDs_ValidObject_ReturnsCreatedAtAction()
        {
            var continentModel = new ContinentModel
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>());
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _countryMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<Country>() { country });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.PutContinent(2, continentModel);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        #endregion

        #region DELETE

        [Fact]
        public void Continent_DELETE_UnknownID_ReturnsBadRequest()
        {
            var response = _continentController.DeleteContinent(3);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Continent_DELETE_KnownID_ReturnsNoContent()
        {
            var continent = new Continent
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<Country>()
            };

            _continentMockRepo.Setup(repo => repo.Get(2))
                .Returns(continent);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.DeleteContinent(2);

            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Continent_DELETE_CountriesNotEmpty_ReturnsBadRequest()
        {
            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());

            var continent = new Continent
            {
                ContinentID = 2,
                Name = "Europa",
                Countries = new List<Country>() { country }
            };

            _continentMockRepo.Setup(repo => repo.Get(2))
                .Returns(continent);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.DeleteContinent(2);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        #endregion

        #endregion

        #region COUNTRY

        #region GET

        [Fact]
        public void Country_GET_GetAllCountries_ReturnsIEnumerableCountryViewModel()
        {
            IEnumerable<Country> countries = new List<Country>();

            _countryMockRepo.Setup(repo => repo.GetAll(null))
                .Returns(countries);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _continentController.GetAllCountries();

            Assert.IsType<List<CountryViewModel>>(result);
        }

        [Fact]
        public void Country_GET_Get_InvalidID_ReturnsNotFound()
        {
            _countryMockRepo.Setup(repo => repo.Get(2))
                .Throws(new CountryException("Country doesn't exist."));

            var result = _continentController.GetCountry(2);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void Country_GET_Get_CorrectID_ReturnsOkObject()
        {
            var countries = new List<Country>();
            var continent = new Continent("Europa", countries);

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(new Country("België", 11000000, 30700, continent));

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _continentController.GetCountry(2);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Country_GET_Get_CorrectID_ReturnsCountryViewModel()
        {
            var countries = new List<Country>()
            {
                new Country("België", 11000000, 30700, new Continent())
            };

            var continent = new Continent("Europa", countries)
            {
                ContinentID = 2
            };

            var country = new Country("België", 11000000, 30700, continent) { CountryID = 2 };

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2/Country/2")
                .Verifiable();

            var result = _continentController.GetCountry(2).Result as OkObjectResult;

            Assert.IsType<CountryViewModel>(result?.Value);
            Assert.Equal("https://localhost:5001/api/Continent/2/Country/2", (result.Value as CountryViewModel)?.CountryID);
            Assert.Equal(country.Name, (result.Value as CountryViewModel)?.Name);
            Assert.Equal(country.Population, (result.Value as CountryViewModel)?.Population);
            Assert.Equal($"https://localhost:5001/api/Continent/{continent.ContinentID}/Country/{country.CountryID}",
                (result.Value as CountryViewModel)?.CountryID);
        }

        #endregion

        #region POST

        [Fact]
        public void Country_POST_ValidObject_ReturnsCreatedAtAction()
        {
            var country = new CountryModel
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Surface = 30700,
                Population = 11000000,
                Rivers = new List<int>() { 2 },
                Cities = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>()) { CityID = 2 };
            var river = new River("Schelde", 50, It.IsAny<IList<Country>>()) { RiverID = 2 };

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _cityMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<City>() { city });

            _riverMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<River>() { river });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2/Country/2")
                .Verifiable();

            var response = _continentController.PostCountry(2, country);

            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact]
        public void Country_POST_ValidObject_ReturnsCorrectItem()
        {
            var country = new CountryModel
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Surface = 30700,
                Population = 11000000,
                Rivers = new List<int>() { 2 },
                Cities = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>()) { CityID = 2 };
            var river = new River("Schelde", 50, It.IsAny<IList<Country>>()) { RiverID = 2 };

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _cityMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<City>() { city });

            _riverMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<River>() { river });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2/Country/2")
                .Verifiable();

            var response = _continentController.PostCountry(2, country).Result as CreatedAtActionResult;
            var item = response?.Value as CountryViewModel;

            Assert.IsType<CountryViewModel>(item);
            Assert.Equal("https://localhost:5001/api/Continent/2/Country/2", item.CountryID);
            Assert.Equal(country.Name, item.Name);
            Assert.Equal(country.Population, item.Population);
            Assert.Equal(country.Surface, item.Surface);
            Assert.Equal($"https://localhost:5001/api/Continent/{continent.ContinentID}/Country/{country.CountryID}", item.CountryID);
        }

        [Fact]
        public void Country_POST_InvalidObject_ReturnsBadRequest()
        {
            var country = new CountryModel
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Surface = 30700,
                Population = 11000000,
                Rivers = new List<int>() { 2 },
                Cities = new List<int>() { 2 }
            };

            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>()) { CityID = 2 };
            var river = new River("Schelde", 50, It.IsAny<IList<Country>>()) { RiverID = 2 };

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _cityMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<City>() { city });

            _riverMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<River>() { river });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _continentController.ModelState.AddModelError("format error", "Int expected");
            var response = _continentController.PostCountry(2, country).Result;

            Assert.IsType<BadRequestObjectResult>(response);
        }

        #endregion

        #region PUT

        [Fact]
        public void Country_PUT_NotSameIDs_ReturnsBadRequest()
        {
            var countryModel = new CountryModel
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Surface = 30700,
                Population = 11000000,
                Rivers = new List<int>() { 2 },
                Cities = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>()) { CityID = 2 };
            var river = new River("Schelde", 50, It.IsAny<IList<Country>>()) { RiverID = 2 };

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _cityMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<City>() { city });

            _riverMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<River>() { river });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.PutCountry(2, 3, countryModel);

            Assert.IsType<BadRequestResult>(response);

            var response2 = _continentController.PutCountry(3, 2, countryModel);

            Assert.IsType<BadRequestResult>(response2);
        }

        [Fact]
        public void Country_PUT_InvalidObject_ReturnsBadRequest()
        {
            var countryModel = new CountryModel
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Surface = 30700,
                Population = 11000000,
                Rivers = new List<int>() { 2 },
                Cities = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>()) { CityID = 2 };
            var river = new River("Schelde", 50, It.IsAny<IList<Country>>()) { RiverID = 2 };

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _cityMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<City>() { city });

            _riverMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<River>() { river });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _continentController.ModelState.AddModelError("format error", "int expected");
            var response = _continentController.PutCountry(2, 2, countryModel);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Country_PUT_SameIDs_ValidObject_ReturnsCreatedAtAction()
        {
            var countryModel = new CountryModel
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Surface = 30700,
                Population = 11000000,
                Rivers = new List<int>() { 2 },
                Cities = new List<int>() { 2 }
            };

            var country = new Country("België", 11000000, 30700, It.IsAny<Continent>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>()) { CityID = 2 };
            var river = new River("Schelde", 50, It.IsAny<IList<Country>>()) { RiverID = 2 };

            _continentMockRepo.Setup(x => x.Get(2))
                .Returns(continent);

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _cityMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<City>() { city });

            _riverMockRepo.Setup(x => x.GetAll(new List<int>() { 2 }))
                .Returns(new List<River>() { river });

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.PutCountry(2, 2, countryModel);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        #endregion

        #region DELETE

        [Fact]
        public void Country_DELETE_UnknownID_ReturnsBadRequest()
        {
            var response = _continentController.DeleteCountry(3);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Country_DELETE_KnownID_ReturnsNoContent()
        {
            var country = new Country
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Cities = new List<City>(),
                Rivers = new List<River>()
            };

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.DeleteCountry(2);

            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Country_DELETE_RiversNotEmpty_ReturnsBadRequest()
        {
            var river = new River("Schelde", 100, It.IsAny<IList<Country>>());

            var country = new Country
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Cities = new List<City>(),
                Rivers = new List<River>() { river }
            };

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.DeleteCountry(2);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Country_DELETE_CitiesNotEmpty_ReturnsBadRequest()
        {
            var city = new City("Brussel", 1000000, true, It.IsAny<Country>());

            var country = new Country
            {
                CountryID = 2,
                ContinentID = 2,
                Name = "België",
                Cities = new List<City>() { city },
                Rivers = new List<River>()
            };

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.DeleteCountry(2);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        #endregion

        #endregion

        #region CITY

        #region GET

        [Fact]
        public void City_GET_GetAllCities_ReturnsIEnumerableCityViewModel()
        {
            IEnumerable<City> cities = new List<City>();

            _cityMockRepo.Setup(repo => repo.GetAll(null))
                .Returns(cities);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _continentController.GetAllCities(4);

            Assert.IsType<List<CityViewModel>>(result);
        }

        [Fact]
        public void City_GET_Get_InvalidID_ReturnsNotFound()
        {
            _cityMockRepo.Setup(repo => repo.Get(2))
                .Throws(new CityException("City doesn't exist."));

            var result = _continentController.GetCity(1, 1, 2);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void City_GET_Get_CorrectID_ReturnsOkObject()
        {
            var countries = new List<Country>();
            var continent = new Continent("Europa", countries) { ContinentID = 2 };
            var country = new Country("België", 11000000, 30700, continent) { CountryID = 2, ContinentID = 2 };

            _cityMockRepo.Setup(repo => repo.Get(2))
                .Returns(new City("Brussel", 1000000, true, country) { CityID = 2, CountryID = 2 });

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var result = _continentController.GetCity(2, 2, 2);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void City_GET_Get_CorrectID_ReturnsCityViewModel()
        {
            var countries = new List<Country>()
            {
                new Country("België", 11000000, 30700, new Continent())
            };

            var continent = new Continent("Europa", countries)
            {
                ContinentID = 2
            };

            var country = new Country("België", 11000000, 30700, continent) { CountryID = 2, ContinentID = 2 };
            var city = new City("Brussel", 1000000, true, country) { CityID = 2, CountryID = 2 };

            _cityMockRepo.Setup(repo => repo.Get(2))
                .Returns(city);

            _countryMockRepo.Setup(repo => repo.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2/Country/2/City/2")
                .Verifiable();

            var result = _continentController.GetCity(2, 2, 2).Result as OkObjectResult;

            Assert.IsType<CityViewModel>(result?.Value);
            Assert.Equal("https://localhost:5001/api/Continent/2/Country/2/City/2", (result.Value as CityViewModel)?.CityID);
            Assert.Equal(city.Name, (result.Value as CityViewModel)?.Name);
            Assert.Equal(city.Population, (result.Value as CityViewModel)?.Population);
            Assert.Equal(city.IsCapital, (result.Value as CityViewModel)?.IsCapital);
            Assert.Equal(
                $"https://localhost:5001/api/Continent/{continent.ContinentID}/Country/{country.CountryID}/City/{city.CityID}",
                (result.Value as CityViewModel)?.CountryID);
        }

        #endregion

        #region POST

        [Fact]
        public void City_POST_ValidObject_ReturnsCreatedAtAction()
        {
            var cityModel = new CityModel
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                Population = 1000000,
                IsCapital = true
            };

            var city = new City("Lokeren", 50000, false, It.IsAny<Country>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var country = new Country(
                "België",
                11000000,
                30700,
                continent,
                new List<City>() { city },
                It.IsAny<IList<River>>())
            { ContinentID = 2, CountryID = 2 };

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2/Country/2/City/2")
                .Verifiable();

            var response = _continentController.PostCity(2, 2, cityModel);

            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact]
        public void City_POST_ValidObject_ReturnsCorrectItem()
        {
            var cityModel = new CityModel
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                Population = 1000000,
                IsCapital = true
            };

            var city = new City("Lokeren", 50000, false, It.IsAny<Country>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var country = new Country(
                    "België",
                    11000000,
                    30700,
                    continent,
                    new List<City>() { city },
                    It.IsAny<IList<River>>())
            { ContinentID = 2, CountryID = 2 };

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost:5001/api/Continent/2/Country/2/City/2")
                .Verifiable();

            var response = _continentController.PostCity(2, 2, cityModel).Result as CreatedAtActionResult;
            var item = response?.Value as CityViewModel;

            Assert.IsType<CityViewModel>(item);
            Assert.Equal("https://localhost:5001/api/Continent/2/Country/2/City/2", item.CityID);
            Assert.Equal(cityModel.Name, item.Name);
            Assert.Equal(cityModel.Population, item.Population);
            Assert.Equal(cityModel.IsCapital, item.IsCapital);
            Assert.Equal(
                $"https://localhost:5001/api/Continent/{continent.ContinentID}/Country/{country.CountryID}/City/{cityModel.CityID}",
                item.CityID);
        }

        [Fact]
        public void City_POST_InvalidObject_ReturnsBadRequest()
        {
            var cityModel = new CityModel
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                Population = 1000000,
                IsCapital = true
            };

            var city = new City("Lokeren", 50000, false, It.IsAny<Country>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var country = new Country(
                    "België",
                    11000000,
                    30700,
                    continent,
                    new List<City>() { city },
                    It.IsAny<IList<River>>())
            { ContinentID = 2, CountryID = 2 };

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _continentController.ModelState.AddModelError("format error", "Int expected");
            var response = _continentController.PostCity(2, 2, cityModel).Result;

            Assert.IsType<BadRequestObjectResult>(response);
        }

        #endregion

        #region PUT

        [Fact]
        public void City_PUT_NotSameIDs_ReturnsBadRequest()
        {
            var cityModel = new CityModel
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                Population = 1000000,
                IsCapital = true
            };

            var city = new City("Lokeren", 50000, false, It.IsAny<Country>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var country = new Country(
                    "België",
                    11000000,
                    30700,
                    continent,
                    new List<City>() { city },
                    It.IsAny<IList<River>>())
            { ContinentID = 2, CountryID = 2 };

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _cityMockRepo.Setup(x => x.Get(2))
                .Returns(city);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.PutCity(2, 2, 3, cityModel);

            Assert.IsType<BadRequestResult>(response);

            var response2 = _continentController.PutCity(2, 3, 2, cityModel);

            Assert.IsType<BadRequestResult>(response2);
        }

        [Fact]
        public void City_PUT_InvalidObject_ReturnsBadRequest()
        {
            var cityModel = new CityModel
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                Population = 1000000,
                IsCapital = true
            };

            var city = new City("Lokeren", 50000, false, It.IsAny<Country>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var country = new Country(
                    "België",
                    11000000,
                    30700,
                    continent,
                    new List<City>() { city },
                    It.IsAny<IList<River>>())
            { ContinentID = 2, CountryID = 2 };

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _cityMockRepo.Setup(x => x.Get(2))
                .Returns(city);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            _continentController.ModelState.AddModelError("format error", "int expected");
            var response = _continentController.PutCity(2, 2, 2, cityModel);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void City_PUT_SameIDs_ValidObject_ReturnsCreatedAtAction()
        {
            var cityModel = new CityModel
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                Population = 1000000,
                IsCapital = true
            };

            var city = new City("Lokeren", 50000, false, It.IsAny<Country>());
            var continent = new Continent("Europa", It.IsAny<IList<Country>>()) { ContinentID = 2 };
            var country = new Country(
                    "België",
                    11000000,
                    30700,
                    continent,
                    new List<City>() { city },
                    It.IsAny<IList<River>>())
            { ContinentID = 2, CountryID = 2 };

            _countryMockRepo.Setup(x => x.Get(2))
                .Returns(country);

            _cityMockRepo.Setup(x => x.Get(2))
                .Returns(city);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.PutCity(2, 2, 2, cityModel);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        #endregion

        #region DELETE

        [Fact]
        public void City_DELETE_UnknownID_ReturnsBadRequest()
        {
            var response = _continentController.DeleteCity(3);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void City_DELETE_KnownID_ReturnsNoContent()
        {
            var city = new City
            {
                CityID = 2,
                CountryID = 2,
                Name = "Brussel",
                IsCapital = true,
                Population = 1000000
            };

            _cityMockRepo.Setup(repo => repo.Get(2))
                .Returns(city);

            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            var response = _continentController.DeleteCity(2);

            Assert.IsType<NoContentResult>(response);
        }

        #endregion

        #endregion
    }
}
