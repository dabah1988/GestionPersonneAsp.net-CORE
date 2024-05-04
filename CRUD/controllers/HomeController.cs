using CountryServicesContrat;
using CountryServicesContrat.DTO;
using Entities;
using MesEntites;
using Microsoft.AspNetCore.Mvc;
using ServicesContrat;
using ServicesContrat.DTO;
using Services;
using Rotativa.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CRUD.Filters.ActionFilter;
using System.Reflection;
using CRUD.Filters.AuthorizationFilter;

namespace CRUD.controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IPersonService _IPersonService;
       private readonly ICountryService _ICountryService;
       private readonly ApplicationDb _dbcontext;
        private readonly ILogger<PersonneService> _logger;
        public HomeController(IPersonService personService, ICountryService countryService, ApplicationDb dbContext)
        
        {
            _IPersonService = personService?? throw new ArgumentException(nameof(personService));
            _ICountryService = countryService?? throw new AggregateException(nameof(countryService));
            if(dbContext != null)
            _dbcontext = dbContext ?? throw new AggregateException(nameof(dbContext));
        }
        private  List<CountryPersonne> JointureLinq(List<PersonResponse> ListePersonSorted, List<CountryPersonne> listePersonnesAveclesPays)
        {
            List<CountryPersonne> listePersonnesAveclesPaysResultats = (from a in ListePersonSorted
                                                                        join b in listePersonnesAveclesPays
                                        on a.PersonId equals b.Person.PersonId
                                                                        select b).ToList();
            return listePersonnesAveclesPaysResultats;
        }
        private async Task<List<CountryPersonne>>  GetListCountryPersonne()
        {
            List<PersonResponse> listePersonnesResponse = await _IPersonService.GetAllPersons();
            List<CountryPersonne> listePersonnesAveclesPays = new List<CountryPersonne>();
            if (ViewBag.searchields == null) ViewBag.searchields = GetColumnsOfSearch();
            foreach (var laPersonne in listePersonnesResponse)
            {
                CountryResponse? countryResponse = await _ICountryService.GetCountryById(laPersonne.CountryId);
                if (countryResponse == null) throw new ArgumentNullException(nameof(countryResponse));
                var lePays = new Country() { countryId = countryResponse.countryId, countryName = countryResponse.countryName };
                var personne = new Person()
                {
                    PersonId = laPersonne.PersonId,
                    PersonName = laPersonne.PersonName,
                    Email = laPersonne.Email,
                    Adress = laPersonne.Adress,
                    Country = lePays,
                    Gender = laPersonne.Gender,
                    DateofBirth = laPersonne.DateofBirth.ToString(),
                    CountryId = laPersonne.CountryId
                };
                listePersonnesAveclesPays.Add(new CountryPersonne() { Person = personne, Country = personne.Country, CritereDeRecherche = ViewBag.searchields });
            }
            return listePersonnesAveclesPays;
        }
        private Dictionary<string,string> GetColumnsOfSearch()
        {
            return  new Dictionary<string, string>()
            {
                {nameof(Person.PersonName) ,"Nom de la personne" } ,
              { nameof(Person.Email) ,"Email de la personne" } ,
              { nameof(Person.Adress) ,"Adresse  de la personne" } ,
              { nameof(Person.DateofBirth) ,"date de naissance de la personne" } ,
              { nameof(Person.Gender) ,"genre  de la personne" } ,
              { nameof(Person.Country) ,"Pays  d'origine " } 
            };
        }
        [Route("/")]
        [TypeFilter(typeof(PersonListActionFilter))]
        public async Task<IActionResult> Index(string searchBy, string searchValue, bool sortColumnByAscending, bool isSorted = false)
        {
            if (_IPersonService == null || _ICountryService == null) throw new ArgumentNullException("Impossible d'accéder aux services ");
            List<PersonResponse> listePersonnesResponse = await _IPersonService.GetAllPersons();
            List<CountryPersonne> listePersonnesAveclesPays = new List<CountryPersonne>();
            listePersonnesAveclesPays = await  GetListCountryPersonne();
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchValue))
            {
                if (isSorted)
                {
                    List<PersonResponse> ListePersonSorted = await _IPersonService.GetSortedPerson(listePersonnesResponse, searchBy, sortColumnByAscending);
                    listePersonnesAveclesPays = JointureLinq(ListePersonSorted, listePersonnesAveclesPays);
                }
                else
                {
                    var ListePersonSearched = await _IPersonService.GetFilterPerson(searchBy, searchValue);
                    listePersonnesAveclesPays = JointureLinq(ListePersonSearched, listePersonnesAveclesPays);
                }
            }
            return View(listePersonnesAveclesPays);
        }

        [Route("/Create")]
        [HttpGet]
        public async  Task<IActionResult> Create()
        {
            ViewBag.ListePays = await  _ICountryService.ListeCountries();
            return View(ViewBag.ListePays);  
        }

        [Route("/Create")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            ViewBag.ListePays = await _IPersonService.AddPerson(personAddRequest);
            return RedirectToAction("Index");
        }

        [Route("/Edit")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(string PersonId)
         {
            if (string.IsNullOrEmpty(PersonId)) return BadRequest("Identifiant null");
            if (_IPersonService == null) throw new InvalidOperationException("Service personne null");
            if (_ICountryService == null) throw new InvalidOperationException("Service country  null");           
            if (!Guid.TryParse(PersonId, out  Guid parsePersonId)) return BadRequest("Lidentifiant de la personne est invalide");
            PersonResponse? personResponse = await _IPersonService.GetPersonneById(Guid.Parse(PersonId));
            if(personResponse == null)  return NotFound("La personne n'a pas été trouvée");
            Person personne = personResponse.ToPersonUpdateRequest().ToPerson();

            if(personne.CountryId == null ) throw new InvalidOperationException("cette personne n'a aucun pays null");
            Country? country =      _ICountryService.ListeCountries().Result.Where(c => c.countryId == personne.CountryId).FirstOrDefault();
            CountryPersonne countryPersonne = new CountryPersonne()
            {
                  Country =country,
                  Person = personne
            };
            if(country != null)
            countryPersonne.Person.Country = country;
            return View(countryPersonne);
        }

        [Route("/Edit")]
        [HttpPost]
        public  async Task<IActionResult> Edit(PersonUpdateRequest personne)
        {
            if (personne == null) return NotFound("La personne n'a pas été trouvée");
            if (_IPersonService == null) throw new InvalidOperationException("le service est innaccessible");
            ValidationHelper.ModelValidation(personne);
            await _IPersonService.UpdatePerson(personne);
            return RedirectToAction("Index");
        }

        [Route("Delete")]
        [HttpGet]
        public async  Task<IActionResult> Delete(string PersonId)
        {
            if (string.IsNullOrEmpty(PersonId)) return BadRequest("Identifiant null");
            if (_IPersonService == null) throw new InvalidOperationException("Service personne null");
            if (_ICountryService == null) throw new InvalidOperationException("Service country  null");
            if (!Guid.TryParse(PersonId, out Guid parsePersonId)) return BadRequest("Lidentifiant de la personne est invalide");
            PersonResponse? personResponse = await _IPersonService.GetPersonneById(Guid.Parse(PersonId));
            if (personResponse == null) return NotFound("La personne n'a pas été trouvée");
            Person personne = personResponse.ToPersonUpdateRequest().ToPerson();
            if (personne.CountryId == null) throw new InvalidOperationException("cette personne n'a aucun pays null");
            Country? country = await _dbcontext.Countries.Where(c => c.countryId == personne.CountryId).FirstOrDefaultAsync();
            CountryPersonne countryPersonne = new CountryPersonne()
            {
                Country = country,
                Person = personne
            };
            await _IPersonService.DeletePersonById(parsePersonId);
          return RedirectToAction("Index");
        }

        [Route("PersonToPDF")]
        [HttpGet]
        public async Task<IActionResult> PersonToPDF()
        {
          List<CountryPersonne> personnesCountry =   await GetListCountryPersonne();
            return new ViewAsPdf("~/Views/Home/PersonToPDF.cshtml", personnesCountry, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top= 20,Bottom=20,Left=20,Right=20},
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }

    }
}
