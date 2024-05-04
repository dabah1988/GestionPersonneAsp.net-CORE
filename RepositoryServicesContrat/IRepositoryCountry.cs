using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using MesEntites;

namespace RepositoryServicesContrat
{
    public  interface IRepositoryCountry
    {
        public Task<Country> AddCountry(Country countryAddRequest);
        public Task<Country?> GetCountryById(Guid? contryId);
        public Task<List<Country>> ListeCountries();
        public   Country GetCountryByCountryName(string countryName);

    }
}
