using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using TestApplication.Server;
using FluentAssertions;


namespace IntegrationTests
{
    public class DataItemApiTests: IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public DataItemApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task Post_DataItem_ShouldReturn_Success()
        {
            // Arrange
            var dataItem = new { name = "Alice", email = "alice@example.com" };
            var content = new StringContent(JsonSerializer.Serialize(dataItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/data", content);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_DataItems_ShouldReturn_List()
        {
            // Act
            var response = await _client.GetAsync("/api/data");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Alice"); // Assuming "Alice" was posted before
        }

    }
}
