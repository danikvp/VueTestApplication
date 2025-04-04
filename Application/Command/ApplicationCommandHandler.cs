using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using TestApplication.Application.Models;
using TestApplication.Domain.Entities;

namespace TestApplication.Application.Command
{
    public class ApplicationCommandHandler : IApplicationCommandHandler
    {
        private readonly IDbContext dbContext;

        public ApplicationCommandHandler(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> AddDataItemAsync(CreateDataItemCommand createCommand, CancellationToken cancellationToken = default)
        {
            DataItem submission = new DataItem { Data = createCommand.FormData.ToString() };

            await dbContext.DataItems.AddAsync(submission, cancellationToken);

            return await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
