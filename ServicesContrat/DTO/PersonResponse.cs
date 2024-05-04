using System;
using System.Reflection;
using Entities;
using MesEntites;
using ServicesContrat;

namespace CountryServicesContrat.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public DateTime? DateofBirth { set; get; }
        public double? Age { get; set; }
        public string? Adress { get; set; }
        public bool ReceivesNewsLetter { get; set; } = false;
            public override string  ToString()
        {
            return $" Personne Id :{PersonId}\n Personne Name {PersonName} \n Age {Age}  \n Mail : {Email} \n Adresse : {Adress} \n Genre : {Gender}  ";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null ) return false;
             if(  !(typeof(PersonResponse ).Equals( obj.GetType() )) ) return false;    
             PersonResponse laPersonne = (PersonResponse)obj;
            if (Adress == laPersonne.Adress && Email == laPersonne.Email && Gender == laPersonne.Gender &&
                CountryId == laPersonne.CountryId  &&
                ReceivesNewsLetter == laPersonne.ReceivesNewsLetter && Age == laPersonne.Age    ) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return  PersonId.GetHashCode();
        }

      public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Adress = Adress,
                DateofBirth = DateofBirth,
                Gender = Gender,
                CountryId = CountryId,
                Email = Email,
                ReceivesNewsLetter = ReceivesNewsLetter,
            };
        }
       
    }

    public static class Extensions
    {
        public static PersonResponse ToPersonneResponse( this Person person )
        {
            //ICountryService _ICountryService = new CountryService();
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Adress = person.Adress,
                Email = person.Email,
                DateofBirth = DateTime.Parse ("1988-11-01"),
                ReceivesNewsLetter = person.ReceivesNewsLetter,
                Age = (person.DateofBirth != null) ? Math.Floor((DateTime.Now - DateTime.Parse("1988-11-01")).TotalDays / 365.25) : null 
                

            };
        }

    }
}
