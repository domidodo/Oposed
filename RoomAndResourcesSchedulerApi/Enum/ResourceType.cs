using System.Text.Json.Serialization;

namespace RoomAndResourcesSchedulerApi.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResourceType
    {
        Room,
        Device
    }
}
