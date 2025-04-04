using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using TestApplication.Application.Models;
using TestApplication.Domain.Entities;

namespace TestApplication.Application
{
    public class ApplicationQueryFacade : IApplicationQueryFacade
    {
        private readonly IDbContext dbContext;

        public ApplicationQueryFacade(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<JsonNode>> GetDataItemListAsync(GetDataItemListQuery? query = null, CancellationToken cancellationToken = default)
        {

            if (query == null || string.IsNullOrEmpty(query.searchQuery))
                return await dbContext.DataItems.Select(i => ParseJsonNode(i.Data)).ToListAsync(cancellationToken);


            return await dbContext.DataItems
                .Where(s => EF.Functions.Like(s.Data, $"%{query.searchQuery}%"))
                .Select(i => ParseJsonNode(i.Data)!)
                .ToListAsync(cancellationToken);

        }

        private static JsonNode ParseJsonNode(string? jsonString)
        {
            var emtpyObject = new JsonObject();


            if (string.IsNullOrEmpty(jsonString))
                return emtpyObject;

            try
            {
                return JsonNode.Parse(jsonString) ?? emtpyObject;
            }
            catch
            {
                // If parsing fails, return null to prevent exceptions
                return emtpyObject;
            }
        }

    }
}
