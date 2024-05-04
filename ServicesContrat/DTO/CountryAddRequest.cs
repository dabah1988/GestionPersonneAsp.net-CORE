using System;
using Entities;

namespace ServicesContrat.DTO
{
    public  class CountryAddRequest
{
    public string? countryName { get; set; }
        public CountryAddRequest()
{
    countryName = " ";
        } 

        public Country ToCountry()
    {
        return new Country() { countryName = countryName };
        }
    }
}