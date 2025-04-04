using System.Text.Json.Nodes;

namespace TestApplication.Domain.Entities
{
    public class DataItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string Data { get; set; }
    }
}
