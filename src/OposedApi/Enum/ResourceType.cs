using System.Text.Json.Serialization;

namespace OposedApi.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResourceType
    {
        Room,
        Device
    }
}
