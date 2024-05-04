using MesEntites;
using Microsoft.EntityFrameworkCore;
using RepositoryServicesContrat;
using System;

namespace RepositoryService
{
    public class RepositoryPerson : IRepositoryPerson
    {
        private readonly ApplicationDb  _dbContext;
        public RepositoryPerson(ApplicationDb dbContext)
        {
                _dbContext = dbContext; 
        }
        public async Task<Person> AddPerson(Person? person)
        {
            if(person == null) throw new ArgumentNullException(nameof(person));
              _dbContext.Persons.Add(person);
         await   _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonById(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException(nameof(personId));
            Person? personFound = await _dbContext.Persons.Where( p => p.PersonId== personId).FirstOrDefaultAsync();
            if (personFound != null)
            {
                _dbContext.Persons.Remove(personFound);
                int resultat = await _dbContext.SaveChangesAsync();
                if (resultat != -1) return true;
            }
            return false;   
        }

        public async  Task<List<Person>> GetAllPersons()
        {
            return await  _dbContext.Persons.Include("Country").ToListAsync();
        }

        public Task<List<Person>> GetFilterPerson(string searchBy, string searchText)
        {
            return null ;
        }

        public async  Task<bool> Remove(Person person)
        {
             _dbContext.Persons.Remove(person) ;
            int result =  await _dbContext.SaveChangesAsync() ;
            if(result != -1) return true ;  
            return false ;  
        }


        public async Task<Person?> GetPersonneById(Guid? personId)
        {
            return  await  _dbContext.Persons.Where(s => s.PersonId == personId).FirstOrDefaultAsync();
        }

        public Task<List<Person>> GetSortedPerson(List<Person> personnes, string sortedBy, bool Ascending)
        {
            return null;
        }

        public Task<Person> UpdatePerson(Person? personUpdateRequest)
        {
            return null;
        }
    }
}
