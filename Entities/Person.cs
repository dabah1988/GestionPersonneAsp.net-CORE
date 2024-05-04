using Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MesEntites
{ 
    public  class Person
    { 
        [Key]
       public Guid  PersonId  {get; set; }
        [StringLength(100)]
        public string? PersonName  {get; set; }
        [StringLength(20)]
        public string? DateofBirth { get; set;}
        [StringLength(10)]
        public string? Gender { get; set;}
        [StringLength(30)]
        public string? Email { get; set;}
        public Guid? CountryId { get; set;}
        [ForeignKey("CountryId")]
        public Country? Country { get; set;}
        [StringLength(100)]
        public string? Adress { get; set;}
        public bool ReceivesNewsLetter { get; set; } = false;

        public bool PossedeLePermis {  get; set; } = false; 
    }
}
