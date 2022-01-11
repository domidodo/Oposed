using System.Text.Json.Serialization;

namespace RoomAndResourcesScheduler.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResourceType
    {
        Room,
        Device
    }
}
