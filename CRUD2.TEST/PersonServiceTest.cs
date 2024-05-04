using ServicesContrat;
using Services;
using CountryServicesContrat;
using MesEntites;
using CountryServicesContrat.DTO;
using CountryServicesContrat.enums;
using ServicesContrat.DTO;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Entities;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using RepositoryService;
using RepositoryServicesContrat;

namespace CRUD.TEST
{
    public class PersonServiceTest
    {
        private readonly IPersonService _IPersonService;
        private readonly ICountryService _ICountryService;
        private readonly ITestOutputHelper _ITestOutputHelper;
        private readonly ApplicationDb _dbContext;
        private readonly IFixture _IFixture;
        List<PersonResponse>? listeResponsesFromAddRequest ;
        List<PersonResponse>?  listeResponsesFromSearch;
        List<PersonResponse>? listeResponsesFromGetAllPersonnes;


        public PersonServiceTest(ITestOutputHelper ITestOutputHelper )
        {
            _IFixture = new Fixture();  
            var persons = new List<Person>() { };
            var countries = new List<Country>() { };
            DbContextMock<ApplicationDb> dbContextMoq = new DbContextMock<ApplicationDb>
                   (new DbContextOptionsBuilder<ApplicationDb>().Options);
            ApplicationDb dbContext = dbContextMoq.Object;
            dbContextMoq.CreateDbSetMock(temp => temp.Persons, persons);
            dbContextMoq.CreateDbSetMock(temp => temp.Countries, countries);
            _ICountryService = new CountryService(null);
            _IPersonService = new PersonneService(null, null,null );
            _ITestOutputHelper = ITestOutputHelper;
        }
        [Fact]
        public async Task AddPerson_nullPerson()
        {
            PersonAddRequest personAddRequest = null;
            Func<Task> resultat =  () => _IPersonService.AddPerson(personAddRequest);
            //ArgumentNullException
           await  resultat.Should().ThrowAsync<ArgumentNullException>() ;
        }
        [Fact]
        public async void check_If_personId_From_AddRequest_isEmpty()
        {
            CountryAddRequest? countryAddRequest = _IFixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _ICountryService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = _IFixture.Create<PersonAddRequest>();
            personAddRequest.CountryId = countryResponse.countryId;
            PersonResponse personneResponse = await _IPersonService.AddPerson(personAddRequest);
            List<PersonResponse> ListePersonnesResponse = await _IPersonService.GetAllPersons();
            //Assert.True(personneResponse.PersonId != Guid.Empty);
            //Assert.Contains(personneResponse, ListePersonnesResponse);

            personneResponse.PersonId.Should().NotBe(Guid.Empty);
            ListePersonnesResponse.Should().Contain(personneResponse);
        }

        #region verificationIdPersonne
        [Fact]
        public async  void GetPersonneByID_null_Response()
        {
            if (_IPersonService == null) throw new ArgumentNullException("null");
            Guid? personId = null;        
         //await    Assert.ThrowsAsync<ArgumentNullException>(
         //      async  () =>
         //       {
         //           PersonResponse personneResponse = await _IPersonService.GetPersonneById(personId);
         //       });

            Func<Task> resultat = async () =>
            {
                PersonResponse? personneResponse = await _IPersonService.GetPersonneById(personId);
            };
            await resultat.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CheckIfPersonneIdFromGetPersonByid_Equals_To_PersonIdFromResponseRequest()
        {
            if(_IPersonService == null) throw new ArgumentNullException(nameof(IPersonService));    
            CountryAddRequest? countryAddRequest = _IFixture.Create<CountryAddRequest>() ;
            CountryResponse countryResponse = await   _ICountryService.AddCountry(countryAddRequest);
            PersonAddRequest personneRequest = _IFixture.Create<PersonAddRequest>();
            personneRequest.CountryId = countryResponse.countryId;
            PersonResponse personResponse = await _IPersonService.AddPerson(personneRequest);
            _ITestOutputHelper.WriteLine("Valeur attendue objt 1");
            _ITestOutputHelper.WriteLine(personResponse.ToString());
            PersonResponse? personneResponseFromGetPersonne = await _IPersonService.GetPersonneById(personResponse.PersonId);
            _ITestOutputHelper.WriteLine("Valeur attendue objet 2");
            _ITestOutputHelper.WriteLine(personneResponseFromGetPersonne?.ToString());
            //Assert.Equal(personResponse, personneResponseFromGetPersonne);
            personneResponseFromGetPersonne.Should().Equals(personResponse);

        }
        #endregion


        #region TestGetAllPersons
        [Fact]
        public  async void GetAllPerson_EmptyList()
        {
            List<PersonResponse> personResponses = await _IPersonService.GetAllPersons();
            //Assert.Empty(personResponses);
            personResponses.Should().BeEmpty();

        }
        [Fact]
        public async void check_if_collectionOfAddpersons_equals_to_GetAllPersons()
        {
            CreatePersonne();
            List<PersonResponse>? listeResponsesFromGetAllPersonnes = await _IPersonService.GetAllPersons();
            if(listeResponsesFromAddRequest == null) throw new ArgumentNullException(nameof(listeResponsesFromAddRequest));
            foreach (PersonResponse laPersonneResponse in listeResponsesFromAddRequest)
                //Assert.Contains(laPersonneResponse, listeResponsesFromGetAllPersonnes);
            listeResponsesFromGetAllPersonnes.Should().Contain(laPersonneResponse);
        }

        #endregion

        [Fact]
        public async void CreatePersonne()
        {
            CountryAddRequest countryAddRequest1 =  _IFixture.Build<CountryAddRequest>().With(ct => ct.countryName , "Cote d'Ivoire").Create();
            CountryAddRequest countryAddRequest2 =  _IFixture.Build<CountryAddRequest>().With(ct => ct.countryName, "Israel").Create();
            CountryAddRequest countryAddRequest3 =   _IFixture.Build<CountryAddRequest>().With(ct => ct.countryName, "Allemange").Create();
            CountryResponse countryResponse1 = await _ICountryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _ICountryService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = await _ICountryService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = _IFixture.Build<PersonAddRequest>()
                .With(ct => ct.PersonName, "Dabah Allobié Lynda Karen")
                .With(ct => ct.Email, "lynda@exemple.com")
                .With(ct=> ct.Gender,GenderOptions.Female.ToString())
                 .With(ct => ct.CountryId, countryResponse1.countryId)
                .Create();
              
            PersonAddRequest? personAddRequest2 =
                 _IFixture.Build<PersonAddRequest>()
                .With(ct => ct.PersonName, "Dabah Abbé Ahou Sara ")
                .With(ct => ct.Email, "amour@exemple.com")
                .With(ct => ct.Gender, GenderOptions.Female.ToString())
                .With(ct => ct.CountryId, countryResponse2.countryId)
                .Create();
            PersonAddRequest? personAddRequest3 =
                 _IFixture.Build<PersonAddRequest>()
                .With(ct => ct.PersonName, "Aké Claude ")
                .With(ct => ct.Email, "akeclause@exemple.com")
                .With(ct => ct.Gender, GenderOptions.Female.ToString())
                .With(ct => ct.CountryId, countryResponse2.countryId)
                .Create();
           PersonResponse personResponse1 = await _IPersonService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await _IPersonService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = await _IPersonService.AddPerson(personAddRequest3);
            listeResponsesFromAddRequest = new List<PersonResponse> {  personResponse1, personResponse2,personResponse3 };
        listeResponsesFromGetAllPersonnes =await   _IPersonService.GetAllPersons();
            
        }
    #region FiltrePersonne
    [Fact]
    public async void check_if_Person_SearchBYproperty_And_valueIsFound()
    {
        CreatePersonne();
        var textePourLaRecherche = "Israel";
        listeResponsesFromSearch = await _IPersonService.GetFilterPerson(nameof(Person.Country), textePourLaRecherche);
        //Assert.True(listeResponsesFromSearch.Count() > 0);
            listeResponsesFromSearch.Should().HaveCountGreaterThan(0);
    }

    #endregion

    #region MiseAjour
    [Fact]
    public async void UpdatePerson_NullPerson()
    {
        PersonUpdateRequest personUpdateRequest = null;
       //await  Assert.ThrowsAsync <ArgumentNullException>(
          //async   () =>
          //  {
          //      await  _IPersonService.UpdatePerson(personUpdateRequest);
          //  });

            Func<Task> resultat = 
          async () =>
          {
              await _IPersonService.UpdatePerson(personUpdateRequest);
          };

            await resultat.Should().ThrowAsync<ArgumentNullException>();
        }

    [Fact]
    public void VerificationFormatMail()
    {
        PersonUpdateRequest personne = new PersonUpdateRequest()
        {
            Email = "dabson2012@gmail.com",
            PersonName = "Ehouman Thomas"
        };
        Assert. Matches(@"^[a-zA-Z0-9.-_+]{1,}@[a-zA-Z]{2,}\.[a-zA-Z]{2,}$", personne.Email);
    }

    [Fact]
    public async void Check_if_Update_is_OK()
    {
            if(_IPersonService == null ) throw new ArgumentNullException(nameof(IPersonService));   
            CountryAddRequest countryAddRequest = new CountryAddRequest() { countryName = "Finlande" };
            CountryResponse countryResponse = await _ICountryService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
        {
            PersonName = "Aliman Sagou Emerance ",
            Email = "missAliman@live.fr",
            Gender = GenderOptions.Female.ToString(),
            DateofBirth = DateTime.Now,
            Adress = "Riviera Jardin",
            CountryId = countryResponse.countryId,
            ReceivesNewsLetter = true
        };
        PersonResponse personResponse = await  _IPersonService.AddPerson(personAddRequest);
            personResponse.CountryId = countryResponse.countryId;
        PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
        personResponse = await  _IPersonService.UpdatePerson(personUpdateRequest);
        //listeResponsesFromGetAllPersonnes = _IPersonService.GetAllPersons();
        PersonResponse personneApresUpdate = await  _IPersonService.GetPersonneById(personResponse.PersonId);
        _ITestOutputHelper.WriteLine($" {personResponse}");
        Assert.Equal(personResponse, personneApresUpdate);
        _ITestOutputHelper.WriteLine($" {personUpdateRequest}");
    }
    #endregion
    #region deletePerson

    [Fact]
    public async  void Delete_Valid_person_shoud_return_true()
    {
        bool isDeleted = false;
        CountryAddRequest countryAddRequest = new CountryAddRequest() { countryName = "Erythrée" };
        CountryResponse countryResponse = await _ICountryService.AddCountry(countryAddRequest);

        PersonAddRequest personAddRequest = new PersonAddRequest()
        {
            PersonName = "Lepry Pascal",
            Email = "pascalLepry@gmail.com",
            Adress = "Riviera Golf",
            DateofBirth = DateTime.Parse("1975-03-02"),
            CountryId = countryResponse.countryId,
            Gender = GenderOptions.Male.ToString(),
            ReceivesNewsLetter = true
        };
        PersonResponse personResponse = await _IPersonService.AddPerson(personAddRequest);
        isDeleted = await _IPersonService.DeletePersonById(personResponse.PersonId);
        Assert.True(isDeleted);

    }

        #endregion
        [Fact]
        public  async void  DeleteInvalidPerson_should_return_false()
     {
    bool isDeleted = await _IPersonService.DeletePersonById(Guid.NewGuid());
            Assert.False(isDeleted);
        }

    }
}
