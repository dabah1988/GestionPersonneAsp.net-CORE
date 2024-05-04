using Entities;
using MesEntites;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServicesContrat.DTO;

namespace ServicesContrat
{ 
    public interface ICountryService
    { 
        public Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest) ;
        public Task<List<CountryResponse>> ListeCountriesResponse();

        public Task<CountryResponse?> GetCountryById(Guid? contryId);
        public Task<List<Country>> ListeCountries();
        public   Task<int> UploadCountryFromExcelFile(IFormFile formFile);

    }
}
