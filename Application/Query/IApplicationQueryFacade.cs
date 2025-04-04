using System.Text.Json.Nodes;
using TestApplication.Application.Models;

namespace TestApplication.Application
{
    public interface IApplicationQueryFacade
    {
        Task<IEnumerable<JsonNode>> GetDataItemListAsync(GetDataItemListQuery? query = null, CancellationToken cancellationToken = default);
    }
}
