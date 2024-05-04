using Microsoft.AspNetCore.Mvc.Filters;
using MesEntites;

namespace CRUD.Filters.ActionFilter
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonListActionFilter> _logger;
        public PersonListActionFilter(ILogger<PersonListActionFilter> logger )
        {
                _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("In OnActionExecuted");
            _logger.LogInformation("{filterName} {methodName}", nameof(PersonListActionFilter), nameof(OnActionExecuted));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("In OnActionExecuting");
            _logger.LogInformation("{filterName} {methodName}" , nameof(PersonListActionFilter) , nameof(OnActionExecuting));
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if( !string.IsNullOrEmpty(searchBy) ) 
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(Person.PersonName),
                        nameof(Person.Email),
                        nameof(Person.Country),
                        nameof(Person.Gender),
                        nameof(Person.ReceivesNewsLetter),
                         nameof(Person.Adress)
                    };
                    if(searchByOptions.Any( temp => temp == searchBy) == false )                     
                    {
                        _logger.LogInformation($" search by value {searchBy}");
                        context.ActionArguments["searchBy"] = nameof(Person.PersonName);
                        _logger.LogInformation($" search by value updated : it is equal to : {searchBy}");
                    }
                }
            }
        }
    }
}
