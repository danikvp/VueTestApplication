using TestApplication.Infrastructure;

namespace TestApplication.Server.Extensions
{
    public static class DataInitializerExtensions
    {
        public static void InitDataBase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataItemDbContext>();
            DbInitializer.Initialize(context);
        }
    }
}
