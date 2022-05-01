using LiteDB;
using System.Text.Json.Serialization;

namespace OposedApi.Models
{
    public class Template
    {
        public int Id { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        public bool IsPublic { get; set; } = false;
        public Event Data { get; set; }
    }
}
