using Microsoft.EntityFrameworkCore;
using TestApplication.Domain.Entities;

namespace TestApplication.Application
{
    public interface IDbContext
    {
        DbSet<DataItem> DataItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
