using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryService;
using Entities;
using RepositoryServicesContrat;
using MesEntites;
using Microsoft.EntityFrameworkCore;
namespace RepositoryService
{
    public class RepositoryCountry : IRepositoryCountry
    {
        private readonly ApplicationDb _dbContext;
        public RepositoryCountry(ApplicationDb dbContext)
        {
                _dbContext = dbContext;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public  async Task<Country?> GetCountryById(Guid? countryId)
        {
            return  await _dbContext.Countries.Where( ct => ct.countryId == countryId).FirstOrDefaultAsync();
        }

        public   Country GetCountryByCountryName(string  countryName)
        {
            if(countryName == null ) throw new ArgumentNullException(nameof(countryName));     
            if(_dbContext.Countries == null) throw new ArgumentNullException(nameof(countryName));
            return _dbContext.Countries.Where(ct => ct.countryName == countryName).FirstOrDefault();
        }

        public async  Task<List<Country>> ListeCountries()
        {
            return await _dbContext.Countries.ToListAsync();
        }

    }
}

