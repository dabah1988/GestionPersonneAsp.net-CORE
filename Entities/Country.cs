using System.ComponentModel.DataAnnotations;
namespace Entities
{ 
    public class Country
{ 
    
        [Key]
        public Guid countryId   {get; set; }
        [StringLength(40)]
        public string? countryName  {get; set; }
    }
}
