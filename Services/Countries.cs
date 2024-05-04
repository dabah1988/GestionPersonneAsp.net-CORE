using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Services;
using ServicesContrat;
using ServicesContrat.DTO;
namespace Services
{
    public class Countries

{
    private readonly List<Country>? _countries = new List<Country>();

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        if (countryAddRequest is null)
            { 
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
  
            Country country = countryAddRequest.ToCountry();
            if(_countries == null )throw new ArgumentOutOfRangeException(nameof(country));
            _countries.Add(country);
            return country.ToCountryResponse();
        }
    }
}
