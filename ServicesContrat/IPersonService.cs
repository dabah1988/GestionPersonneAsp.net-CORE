using System;
using System.Collections.Generic;
using System.Linq;
using ServicesContrat;
using Entities;
using MesEntites;
using CountryServicesContrat.DTO;

namespace CountryServicesContrat
{
    public  interface  IPersonService
    {
        public Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
        public Task<List<PersonResponse>> GetAllPersons();

        public Task<PersonResponse?> GetPersonneById(Guid?  personId);

        public Task<List<PersonResponse>> GetFilterPerson(string searchBy , string searchText);

        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        public Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> personnes, string sortedBy, bool Ascending);
        public Task<bool> DeletePersonById(Guid? personId);   
    }
}
