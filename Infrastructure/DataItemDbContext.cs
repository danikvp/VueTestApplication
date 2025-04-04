using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json.Nodes;
using TestApplication.Application;
using TestApplication.Domain.Entities;

namespace TestApplication.Infrastructure
{

    public class DataItemDbContext : DbContext, IDbContext
    {
        public DataItemDbContext(DbContextOptions<DataItemDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DataItem> DataItems => Set<DataItem>();

    }
}
