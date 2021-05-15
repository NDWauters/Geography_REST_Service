using System;
using System.Collections.Generic;
using BusinessLogicLayer.BusinessLogicServices;
using BusinessLogicLayer.ViewModels.Continent;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using Geography_REST_Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
namespace Geography_REST_Service_Tests
{
    public class ContinentControllerTests
    {
        private readonly Mock<ContinentRepo> _continentMockRepo;
        private readonly Mock<CountryRepo> _countryMockRepo;
        private readonly Mock<CityRepo> _cityMockRepo;
        private readonly Mock<RiverRepo> _riverMockRepo;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly ContinentService _continentService;
        private readonly CountryService _countryService;
        private readonly CityService _cityService;
        private readonly RiverService _riverService;
        private readonly ContinentController _continentController;
        private readonly ILogger<ContinentController> _logger;

        public ContinentControllerTests()
        {
            _continentMockRepo = new Mock<ContinentRepo>();
            _countryMockRepo = new Mock<CountryRepo>();
            _cityMockRepo = new Mock<CityRepo>();
            _riverMockRepo = new Mock<RiverRepo>();
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _logger = Mock.Of<ILogger<ContinentController>>();

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

            _continentController = new ContinentController(_logger, _continentService, _countryService, _cityService);
        }

        #region CONTINENT

        [Fact]
        public void Continent_GET_GetAllContinents_ReturnsIEnumerableContinentViewModel()
        {
            IEnumerable<Continent> continents = new List<Continent>();

            _continentMockRepo.Setup(repo => repo.GetAll(null))
                .Returns(continents);

            var result = _continentController.GetAllContinents();

            Assert.IsType<List<ContinentViewModel>>(result);
        }

        #endregion

        #region COUNTRY



        #endregion

        #region CITY



        #endregion


    }
}
