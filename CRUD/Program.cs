using ServicesContrat;
using Services;
using CountryServicesContrat;
using Microsoft.EntityFrameworkCore;
using MesEntites;
using OfficeOpenXml;
using RepositoryService;
using RepositoryServicesContrat;
using CRUD.Filters.ActionFilter;
using Serilog;
using Serilog.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(
    (HostBuilderContext monContext, IServiceProvider services ,LoggerConfiguration loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(monContext.Configuration)
        .ReadFrom.Services(services);   
    }
    );


builder.Services.AddControllersWithViews(
    options =>
    {
        options.Filters.Add(typeof(PersonListActionFilter));
    }
    );
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IRepositoryCountry, RepositoryCountry>();
builder.Services.AddScoped<IRepositoryPerson, RepositoryPerson>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IPersonService, PersonneService>();
builder.Services.AddDbContext<ApplicationDb>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var app = builder.Build();
app.UseRouting();
app.MapControllers();
if (!builder.Environment.IsEnvironment("TEST"))
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "rotativa");
}

//builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
//{
//    loggerConfiguration
//        //.WriteTo.Logger()
//        .WriteTo.File("log.txt"); // Ajoutez d'autres sinks au besoin
//});

//app.UseHttpLogging();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}
app.Logger.LogDebug("Debug-message");
app.Logger.LogCritical("Log critical");
app.Logger.LogWarning("Log warning");
app.Logger.LogTrace("Log Trace");
app.Run();

public partial class Progam
{

}