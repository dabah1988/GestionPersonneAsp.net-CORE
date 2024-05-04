using System.Reflection;
using CountryServicesContrat;
using CountryServicesContrat.DTO;
using CountryServicesContrat.enums;
using MesEntites;
using ServicesContrat;
using ServicesContrat.DTO;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryServicesContrat;
using Microsoft.Extensions.Logging;
namespace Services
{ 
    public class PersonneService : IPersonService
    { 
        private readonly List<Person> _listePersonnes;
        private readonly ICountryService? _countryService;
        private readonly IRepositoryPerson _IRepositoryPerson;
        private readonly IRepositoryCountry _IRepositoryCountry;
        private readonly ILogger<PersonneService> _logger;

        public PersonneService ( IRepositoryPerson IRepositoryPerson, IRepositoryCountry IRepositoryCountry, ILogger<PersonneService> logger ,    bool isMockDataNeeded = true)
        {
            _IRepositoryPerson = IRepositoryPerson;
            _IRepositoryCountry = IRepositoryCountry;
            _listePersonnes = new List<Person>();
            _countryService = new CountryService(_IRepositoryCountry, true );
            _logger = logger;
        }

        private async Task<PersonResponse> PersonToPersonneResponseType(Person person)
        {
            _logger.LogInformation("PersonToPersonneResponseType executed");
            if (_countryService == null) throw new ArgumentOutOfRangeException("Service country indisponible");
            PersonResponse personResponse = person.ToPersonneResponse();
              var temp_country   =  await  _countryService.GetCountryById(person.CountryId);
            if(temp_country is not null )   personResponse.CountryName = temp_country.countryName;
            return personResponse;
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            _logger.LogInformation("in AddPerson AddPerson");
            if (_countryService == null) throw new ArgumentOutOfRangeException("Service country indisponible");
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));
            if (String.IsNullOrEmpty(personAddRequest.PersonName)) throw new ArgumentException("Personne name must not be null");
            ValidationHelper.ModelValidation(personAddRequest);
            Person person = personAddRequest.ToPerson();
            var lePays  = await _countryService.GetCountryById(person.CountryId);
            if (lePays == null) throw new ArgumentOutOfRangeException("Aucun  pays trouvé");
            person.PersonId = Guid.NewGuid();
            Country country = new Country() { countryId = lePays.countryId , countryName = lePays.countryName };
            person.Country = country;
            await   _IRepositoryPerson.AddPerson(person);
            _listePersonnes.Add(person);
            PersonResponse personResponse =await  PersonToPersonneResponseType(person);
            return personResponse;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("in GetAllPersons  ");
            return (await _IRepositoryPerson.GetAllPersons()).Select(temp => temp.ToPersonneResponse()).ToList(); 
        }


        public async Task <PersonResponse?> GetPersonneById(Guid? personId)
        {
            _logger.LogInformation("in GetPersonneById  ");
            if (personId == null) throw new ArgumentNullException("personne ID null");
            if (_IRepositoryPerson.GetAllPersons() == null) throw new ArgumentNullException("Impossible de récupérer les données depuis la base");
            Person? personne = (await _IRepositoryPerson.GetAllPersons()).Where(id => id.PersonId == personId).FirstOrDefault();
                if (personne == null) return null;
                else return personne.ToPersonneResponse();         
        }

        public async Task<List<PersonResponse>> GetFilterPerson(string searchBy, string searchText)
        {
            _logger.LogInformation("in GetFilterPerson  ");
            List<PersonResponse>? listeDefinitive = null , temp_listeDefinitive = null ;
            var people =( await _IRepositoryPerson.GetAllPersons());
            List<Person> temp_ListePersonnes  ;
            if (String.IsNullOrEmpty(searchText) || String.IsNullOrEmpty(searchBy)) return  people.Select(s => s.ToPersonneResponse()).ToList();
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    temp_ListePersonnes =   people.Where(s =>
                 !String.IsNullOrEmpty(s.PersonName) ? s.PersonName.Contains(searchText): false).ToList();
                       temp_listeDefinitive =   temp_ListePersonnes.Select(s => s.ToPersonneResponse()).ToList();
                    listeDefinitive = temp_listeDefinitive;
                    break;

                case nameof(Person.Email):
                    temp_ListePersonnes =  people
                    .Where(p => !string.IsNullOrEmpty(p.Email) && p.Email.Contains(searchText)).ToList();
                        temp_listeDefinitive = temp_ListePersonnes.Select(p => p.ToPersonneResponse()) .ToList();
                    listeDefinitive = temp_listeDefinitive;
                    break;

                case nameof(Person.Adress):
                    temp_ListePersonnes =   people.Where(s =>
            !String.IsNullOrEmpty(s.Adress) ? s.Adress.Contains(searchText) : false).ToList();
                    listeDefinitive = temp_ListePersonnes.Select(s => s.ToPersonneResponse()).ToList();
                    break;

                case nameof(Person.Country):
                    temp_ListePersonnes =   people.Where(s => !(s.Country == null) ?
                    s.Country.countryName.Contains(searchText) : false).ToList();
                    listeDefinitive = temp_ListePersonnes.Select(s => s.ToPersonneResponse()).ToList();
                    break;

                case nameof(Person.Gender):
                    var sexe = (searchText == "Male") ? GenderOptions.Male : GenderOptions.Female;
                    temp_ListePersonnes =   people.Where(s =>
            !String.IsNullOrEmpty(s.Gender) ? s.Gender.Equals(sexe.ToString()) : false).ToList();
                    listeDefinitive = temp_ListePersonnes.Select(s => s.ToPersonneResponse()).ToList();
                    break;

                default: listeDefinitive =   people.Select(s => s.ToPersonneResponse()).ToList(); break;

            }
            return listeDefinitive;
        }

        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> personnes, string sortedBy, bool Ascending)
        {
            //  if (personnes == null) throw new  ArgumentNullException(nameof(personnes));
            //  PropertyInfo? prop = typeof(Person).GetProperty(sortedBy);
            //  if (prop == null) throw new ArgumentNullException(nameof(prop));
            //  IQueryable<PersonResponse> query = await _IRepositoryPerson.GetAllPersons().Select(p => p.ToPersonneResponse());
            //query =  Ascending? query.OrderBy(personne => prop.Name): query.OrderByDescending(personne => prop.Name);
            // var  sortedList = await query.ToListAsync();
            //  return sortedList;
            return null;
        }
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));
            if (_IRepositoryPerson == null || _IRepositoryPerson.GetAllPersons() == null ) throw new ArgumentNullException(nameof(_IRepositoryPerson));      
            CountryAddRequest countryAddRequest = new CountryAddRequest() { countryName = "United kingdom" };
            //CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = (await _IRepositoryPerson. GetAllPersons()).Where(x => x.PersonId == personUpdateRequest.PersonId).FirstOrDefault();
            if (matchingPerson == null) throw new ArgumentException("La personne fournie n'a pas été trouvée dans la base de données");
            {
                matchingPerson.Email = personUpdateRequest.Email;
                matchingPerson.PersonName = personUpdateRequest.PersonName;
                matchingPerson.Adress = personUpdateRequest.Adress;
                matchingPerson.Gender = personUpdateRequest.Gender;
                matchingPerson.DateofBirth = personUpdateRequest.DateofBirth.ToString();
            };
            ValidationHelper.ModelValidation(matchingPerson);
           await  _IRepositoryPerson.UpdatePerson(matchingPerson);
            return matchingPerson.ToPersonneResponse();
        }

        public async Task<bool> DeletePersonById(Guid? personId)
    {
            _logger.LogInformation("in DeletePersonById");
            try
            {
                if (personId == null) throw new ArgumentNullException(nameof(personId), "Identifiant null");
                if (_IRepositoryPerson == null || _IRepositoryPerson.GetAllPersons() == null) throw new ArgumentNullException( "Liste de personnes vide");
                Person? personneAsupprimer = null;
                personneAsupprimer = ( await _IRepositoryPerson.GetAllPersons()).Where(s => s.PersonId == personId).FirstOrDefault();
                if (personneAsupprimer != null)
                {
                   await  _IRepositoryPerson.Remove(personneAsupprimer);
                }
            }catch (Exception ex)
            {

            }
            return false;
        }
    }
}
