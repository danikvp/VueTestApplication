using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using TestApplication.Application;
using TestApplication.Application.Command;
using TestApplication.Application.Models;

namespace TestApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IApplicationQueryFacade applicationQueryFacade;
        private readonly IApplicationCommandHandler applicationCommandHandler;

        public DataController(IApplicationQueryFacade applicationQueryFacade, IApplicationCommandHandler applicationCommandHandler)
        {
            this.applicationQueryFacade = applicationQueryFacade;
            this.applicationCommandHandler = applicationCommandHandler;
        }



        [HttpPost]
        public async Task<ActionResult<int>> PostData([FromBody] JsonNode formData, CancellationToken cancellationToken = default)
        {
            return Ok(await applicationCommandHandler.AddDataItemAsync(new CreateDataItemCommand { FormData = formData }, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] GetDataItemListQuery query, CancellationToken cancellationToken = default)
        {
            // Return all data
            return Ok(await applicationQueryFacade.GetDataItemListAsync(query, cancellationToken));
        }

    }
}
