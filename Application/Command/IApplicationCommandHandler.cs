using System.Text.Json.Nodes;
using TestApplication.Application.Models;

namespace TestApplication.Application.Command
{
    public interface IApplicationCommandHandler
    {
        Task<int> AddDataItemAsync(CreateDataItemCommand inputData, CancellationToken cancellationToken = default);
    }
}
