using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using TestApplication.Application;
using TestApplication.Application.Command;
using TestApplication.Application.Models;
using TestApplication.Infrastructure;

namespace UnitTests
{
    public class ApplicationFacadeTests
    {

        private DataItemDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DataItemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DataItemDbContext(options);
        }

        [Fact]
        public async Task ShouldRetrivedStoredDataItems()
        {
            using var dbContext = GetDbContext();

            var createItemCommand = new CreateDataItemCommand
            {
                FormData = JsonNode.Parse("""{"name":"Mark","email":"mark@example.com"}""")!
            };
            var applicationCommandHandler = new ApplicationCommandHandler(dbContext);
            var applicationQueryFacade = new ApplicationQueryFacade(dbContext);

            await applicationCommandHandler.AddDataItemAsync(createItemCommand);

            var retrievedItem = (await applicationQueryFacade.GetDataItemListAsync()).Single();

            Assert.Equal("Mark", retrievedItem["name"]!.ToString());
        }


        [Fact]
        public async Task ShouldStoreDataItem()
        {
            using var dbContext = GetDbContext();

            var handler = new ApplicationCommandHandler(dbContext);
            var jsonNode = new CreateDataItemCommand
            {
                FormData = JsonNode.Parse("""{"name":"John","email":"john@example.com"}""")!
            };
            await handler.AddDataItemAsync(jsonNode!);


            Assert.Single(dbContext.DataItems);
        }



    }
}