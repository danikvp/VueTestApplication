using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestApplication.Application;
using TestApplication.Application.Command;

namespace TestApplication.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder) {
            string connectionString = builder.Configuration.GetConnectionString("DataItemInMemoryDb")?? throw new InvalidOperationException("Connection string 'DataItemInMemoryDb' not found.");

            builder.Services.AddDbContext<DataItemDbContext>(options => options.UseInMemoryDatabase(connectionString));
            builder.Services.AddScoped<IDbContext, DataItemDbContext>();
            builder.Services.AddScoped<IApplicationQueryFacade, ApplicationQueryFacade>();
            builder.Services.AddScoped<IApplicationCommandHandler, ApplicationCommandHandler>();
        }
    }
}
