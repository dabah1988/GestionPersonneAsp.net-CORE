using MesEntites;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD;
using CRUD.controllers;
namespace CRUD.TEST
{
    public    class IntegrationWebApplicationTest : WebApplicationFactory<Progam>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("TEST");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDb>));
                if (descriptor != null) services.Remove(descriptor);
                services.AddDbContext<ApplicationDb>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseTest");
                });
            });
        }
    }
}
