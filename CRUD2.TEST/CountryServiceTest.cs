using ServicesContrat;
using Services;
using ServicesContrat.DTO;
using MesEntites;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Entities;
using AutoFixture;
using FluentAssertions;
using RepositoryServicesContrat;
using RepositoryService;
using Moq;
namespace CRUD2.TEST
{ 
    public class CountryServiceTest
    { 
        private readonly ICountryService _IcountryService;
        private readonly ApplicationDb _dbContext;
        private readonly IFixture _IFixture ;
        private readonly IRepositoryCountry _IRepositoryCountry;
        private readonly Mock<IRepositoryCountry> _IMokRepositoryCountry; 
        public CountryServiceTest()
        {
            var countries = new List<Country>() {};
            _IMokRepositoryCountry = new Mock<IRepositoryCountry>();
            _IRepositoryCountry = _IMokRepositoryCountry.Object;
            _IcountryService = new CountryService(_IRepositoryCountry);
            _IFixture = new Fixture();  
             }
        [Fact]
        public async Task Add_Null_Country_Should_Return_ArgumentNullException()
        {
            ////Arrange
            CountryAddRequest? countryAddRequest = null;
            Country?  country = null  ;
            _IMokRepositoryCountry.Setup( sc => sc.AddCountry(It.IsAny<Country>()) )
                .Throws<ArgumentNullException>();
            Func <Task> action = async() => await  _IcountryService.AddCountry(countryAddRequest); 
          await   action.Should().ThrowAsync<ArgumentNullException>();                             
        }

        [Fact]
        public async void Add_CountryContryNameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _IFixture.Build<CountryAddRequest>().With(temp => temp.countryName,null as string).Create() ;
            Country country = countryAddRequest.ToCountry();
            _IMokRepositoryCountry.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
              .ReturnsAsync(country);
            Func<Task> retour =  async () =>  await _IcountryService.AddCountry(countryAddRequest);
           await  retour.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Add_CountryDuplicateContryName_shoub_be_return_null()
        {
            //Arrange
            CountryAddRequest? AddRequest = _IFixture.Build<CountryAddRequest>().With(temp => temp.countryName, "IVOIRY COAST").Create();
            CountryAddRequest? AddRequest2 = _IFixture.Build<CountryAddRequest>().With(temp => temp.countryName, "IVOIRY COAST").Create();
            Country country1 = AddRequest2.ToCountry();
            _IMokRepositoryCountry.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country1);
            Func<Task> resultat = async () =>
            {
                await _IcountryService.AddCountry(AddRequest);
                await _IcountryService.AddCountry(AddRequest2);
            };
            await resultat.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact]
        public  async Task  GetCountryByCountryIdShouldReturnCoutry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _IFixture.Build<CountryAddRequest>()
                .With(temp => temp.countryName, "Nigeria")
                .Create();
            Country country = countryAddRequest.ToCountry();
            _IMokRepositoryCountry.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
            CountryResponse countryResponseCreated = await _IcountryService.AddCountry(countryAddRequest);
            _IMokRepositoryCountry.Setup(temp => temp.GetCountryById(It.IsAny<Guid>())).ReturnsAsync(country);
            CountryResponse? countryResponseTmp = await _IcountryService.GetCountryById(countryResponseCreated.countryId);
            countryResponseCreated.Should().Equals(countryResponseTmp);
            //countryExpected.Should().Be(countryResponseCreated);
        }



        [Fact]
        public  async void Add_CountryCountryDetails()
        { 
            //Arrange
            CountryAddRequest? AddRequest = _IFixture.Build<CountryAddRequest>().With(temp => temp.countryName, "Japon" ).Create();
           CountryResponse reponseCountry =  await   _IcountryService. AddCountry(AddRequest);   
                    //Assert.True(reponseCountry.countryId != Guid.Empty);
            reponseCountry.countryId.Should().NotBe(Guid.Empty);
        }

        #region ListeComplete
        [Fact]
        public void Check_if_ListeisEmpty()
        {
            List<CountryResponse>  ActualCountries = new List<CountryResponse>() ;
            ActualCountries.Should().BeEmpty();        }
          #endregion
        }
}