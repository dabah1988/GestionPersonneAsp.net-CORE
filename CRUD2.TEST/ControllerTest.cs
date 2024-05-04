using ServicesContrat;
using Moq;
using AutoFixture;
using CountryServicesContrat;
using CountryServicesContrat.DTO;
using CRUD.controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using ServicesContrat.DTO;
namespace CRUD.TEST
{
    public  class ControllerTest
    {
        private readonly ICountryService _countryService;
        private readonly IPersonService _personService;
        private readonly Mock<ICountryService> _countryServiceMock;
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly Fixture _IFixture;
        public ControllerTest()
        {
                _IFixture = new Fixture();
               _personServiceMock = new Mock<IPersonService>();
               _countryServiceMock = new Mock<ICountryService>();   
              _personService = _personServiceMock.Object;
             _countryService = _countryServiceMock.Object;                
        }
        [Fact]
        public async Task IndexShouldReturnViewWithPersonsLists()
        {
            List<CountryResponse> country_response_list = _IFixture.Create<List<CountryResponse>>();
            CountryResponse countryResponse = _IFixture.Create<CountryResponse>();
            List<PersonResponse> persons_response_list = _IFixture
                .Create<List<PersonResponse>>();


            _personServiceMock.Setup(temp => temp.GetAllPersons())
              .ReturnsAsync(persons_response_list);

            _personServiceMock.Setup(temp => temp.GetSortedPerson(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(persons_response_list);
                
            _personServiceMock.Setup(temp => temp.GetFilterPerson( It.IsAny<string>(), It.IsAny<string>()))
               .ReturnsAsync(persons_response_list) ;

            _countryServiceMock.Setup(temp => temp.GetCountryById(It.IsAny<Guid>()))
                .ReturnsAsync(countryResponse);

            HomeController homeController = new HomeController(_personService, _countryService, null);
          IActionResult result =   await homeController.Index(_IFixture.Create<string>(), _IFixture.Create<string>(), _IFixture.Create<bool>(), _IFixture.Create<bool>());
            Assert.IsType<ViewResult>(result);          
        }

        [Fact]
        public async Task CreateShouldReturnWiewIfThereNoErrorOfValidation()
        {
            PersonAddRequest personAddRequest = _IFixture.Create<PersonAddRequest>();
            PersonResponse personResponse = _IFixture.Create<PersonResponse>();
            CountryResponse countryResponse = _IFixture.Create<CountryResponse>();
            _personServiceMock.Setup(temp =>temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);
            _countryServiceMock.Setup(temp => temp.GetCountryById(It.IsAny<Guid>()))  .ReturnsAsync(countryResponse);
            HomeController homeController = new HomeController(_personService, _countryService, null);
         IActionResult result = await    homeController.Create(personAddRequest);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
            //Assert.IsType<ViewResult>(result);  
        }

    }
}
