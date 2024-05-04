using System;
using System.Runtime.CompilerServices;
using Entities;
namespace ServicesContrat.DTO
{
    public  class CountryResponse
{
    public Guid countryId { get; set; }
     public string? countryName { get; set; }      
        
    }

    public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse ( this Country  theContry  )
{
    return   new CountryResponse() { countryId = theContry.countryId , countryName = theContry.countryName };   
        }
    }
}
