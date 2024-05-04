using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using MesEntites;

namespace RepositoryServicesContrat
{
    public  interface IRepositoryPerson
    {
        public Task<Person> AddPerson(Person? personAddRequest);
        public Task<List<Person>> GetAllPersons();

        public Task<Person?> GetPersonneById(Guid? personId);

        public Task<List<Person>> GetFilterPerson(string searchBy, string searchText);

        public Task<Person> UpdatePerson(Person? personUpdateRequest);

        public Task<List<Person>> GetSortedPerson(List<Person> personnes, string sortedBy, bool Ascending);
        public Task<bool> DeletePersonById(Guid? personId);
        public  Task<bool> Remove(Person person);

    }
}
