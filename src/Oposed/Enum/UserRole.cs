using System.Text.Json.Serialization;

namespace Oposed.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        User,
        Admin
    }
}
