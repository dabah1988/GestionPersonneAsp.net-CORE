using Entities;
using MesEntites;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using ServicesContrat;
using ServicesContrat.DTO;
using RepositoryServicesContrat;
namespace Services
{ 
    public class CountryService : ICountryService
    {
        private readonly IRepositoryCountry _ICountryRepository;

        private   readonly List<Country>? _countries;

        public CountryService(IRepositoryCountry ICountryRepository, bool isMockDataNeeded = true)
        {
            _ICountryRepository = ICountryRepository;
        }

        public async  Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (_ICountryRepository == null) throw new ArgumentNullException(nameof(countryAddRequest), " Country repository null ");
            if (countryAddRequest ==  null) throw new ArgumentNullException( nameof(countryAddRequest)," Country address null ");
            if (countryAddRequest.countryName == null) throw new ArgumentNullException(nameof(countryAddRequest.countryName), " Country name is  null ");
            Country country = countryAddRequest.ToCountry();
            country.countryId = Guid.NewGuid();
            var rechercherNom =   _ICountryRepository.GetCountryByCountryName(countryAddRequest.countryName);
            if (  rechercherNom != null) throw new ArgumentNullException(" Liste Country null ");
                await _ICountryRepository.AddCountry(country);
            return country.ToCountryResponse();
        }
        public async Task<List<CountryResponse>> ListeCountriesResponse()
    {
        return (await _ICountryRepository.ListeCountries()).Select(cn => cn.ToCountryResponse()).ToList() ;
        }

 
        public async Task<List<Country>> ListeCountries()
    {
          List<Country> pays = await _ICountryRepository.ListeCountries();
        return pays.ToList() ;
        }
        public async Task<CountryResponse?> GetCountryById(Guid? countryId)
        {
            try
            {
                if (countryId == null)
                    throw new ArgumentNullException(nameof(countryId));
                if (_ICountryRepository == null)
                    throw new ArgumentNullException(nameof(countryId));

                Country? result = await _ICountryRepository.GetCountryById(countryId);
                return result.ToCountryResponse();
            }
            catch (Exception ex)
            {
                // Handle exceptions here, logging or rethrowing as necessary.
                Console.WriteLine($"An error occurred: {ex}");
                return null;
            }
        }

        public async Task<int> UploadCountryFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            int countryInserted = 0;
            await formFile.CopyToAsync(memoryStream);
            using( ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["countries"];
                int rows = worksheet.Dimension.Rows;
                for(int row =2; row <= rows; row++) {
                    string? countryName = Convert.ToString(worksheet.Cells[row,1].Value);
                    if(!countryName.IsNullOrEmpty())
                    {
                        Country country = new Country() { countryName = countryName };
                        if ( (await _ICountryRepository.ListeCountries()).Where(s => s.countryName == countryName).Count() == 0)
                        {
                            await _ICountryRepository.AddCountry(country);
                            countryInserted++;  
                        }
                    }
                }
            }
            return countryInserted;
        }

    }
}
