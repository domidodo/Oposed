using System.Text.Json.Serialization;

namespace RoomAndResourcesScheduler.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        User,
        Admin
    }
}
