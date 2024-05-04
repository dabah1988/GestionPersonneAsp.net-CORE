using MesEntites;
using System.ComponentModel.DataAnnotations;

namespace CountryServicesContrat.DTO
{
    public  class PersonUpdateRequest
    {
        [Required(ErrorMessage ="Person Id must not be null")]  
        public Guid PersonId { get; set; }
            [Required(ErrorMessage = "Le nom de la personne est requis")]
            public string? PersonName { get; set; }
            [Required(ErrorMessage = "Date de naissance obligatoire ")]
            public DateTime? DateofBirth { get; set; }
            [Required(ErrorMessage = "Le gendre de la personne est requis")]
            public string? Gender { get; set; }
            public string? Email { get; set; }
            public Guid? CountryId { get; set; }
            [Required(ErrorMessage = "L'adresse est obligatoire'")]
            public string? Adress { get; set; }
            [Required(ErrorMessage = "Merci de renseigner votre adresse de news Letters")]
            public bool ReceivesNewsLetter { get; set; }

            public Person ToPerson()
    {
        return new Person
        {
            PersonId = PersonId,
                    PersonName = PersonName,
                    DateofBirth = DateofBirth.ToString(),
                    Gender = Gender.ToString(),
                    CountryId = CountryId,
                    Email = Email,
                    Adress = Adress,
                    ReceivesNewsLetter = ReceivesNewsLetter
                };
            }
        public override string ToString()
        {
            return $" Personne Id : {PersonId}\n Personne Name {PersonName}   \n Mail : {Email} \n Adresse : {Adress} \n Genre : {Gender}  ";
        }

    }
    }
