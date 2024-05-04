using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using MesEntites;
using ServicesContrat;


namespace CountryServicesContrat.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="Le nom de la personne est requis")]
        public string? PersonName  {  get; set; }
        [Required(ErrorMessage = "Date de naissance obligatoire ")]
        public DateTime? DateofBirth { get; set; }
        [Required(ErrorMessage = "Le gendre de la personne est requis")]
        public string? Gender { get; set; }
        //"erancho@gs2e.ci",  sarantamon@gmail.com
        [RegularExpression(@"^[a-zA-Z0-9.-_+]{1,}@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$", ErrorMessage = "Adresse email invalide")]
        public string? Email { get; set; }
        public Guid? CountryId { get; set; }
        [Required(ErrorMessage = "L'adresse est obligatoire'")]
        public string? Adress { get; set; }
        [Required(ErrorMessage ="Merci de renseigner votre adresse de news Letters")]
        public bool ReceivesNewsLetter { get; set; } 

        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                DateofBirth = DateofBirth.ToString(),
                CountryId = CountryId,
                Gender = Gender.ToString(),
                Email = Email,
                Adress = Adress,
                ReceivesNewsLetter = ReceivesNewsLetter
            };
        }
    }
}
