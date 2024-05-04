using CountryServicesContrat.DTO;
using MesEntites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class ValidationHelper

    {
        public static void ModelValidation(object objet)
        {

            ValidationContext validationContext = new ValidationContext(objet);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(objet, validationContext, validationResults, true);
            if (!isValid) { throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage); }
        }

    }
}
