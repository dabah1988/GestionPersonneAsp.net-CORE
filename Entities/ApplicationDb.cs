using Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System;
using System.IO;
namespace MesEntites
{
    public  class ApplicationDb:DbContext
    {
        public virtual DbSet<Person> Persons { get; set; }  
        public virtual DbSet<Country>Countries { get; set; }

        public ApplicationDb(DbContextOptions options) : base(options)
        {

}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            string countriesJson = System.IO.File.ReadAllText("country.json");
            List<Country> listeCountries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach( Country country in listeCountries)  modelBuilder.Entity<Country>().HasData(country);

            string personJson = System.IO.File.ReadAllText("persons.json");
            List<Person> listePersonnes = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personJson);
            foreach (Person personne in listePersonnes) 
            {
                if (!string.IsNullOrEmpty(personne.DateofBirth))
                {
                    if (DateTime.TryParseExact(personne.DateofBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth))
                    {
                        personne.DateofBirth = dateOfBirth.ToString();
                    }
                    else
                    {
                        // Handle invalid date format
                    }
                }
                modelBuilder.Entity<Person>().HasData(personne);
            
            }
           }
    }
}
