using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Infrastructure;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext configuration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataItemDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // Add an in-memory database for testing
                services.AddDbContext<DataItemDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Ensure the database is created
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataItemDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
