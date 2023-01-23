using LiteDB;

namespace OposedApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int RoomId { get; set; } = 0;
        public List<int> DevicesIds { get; set; } = new List<int>();
        public int OrganizerId { get; set; } = 0;
        public bool IsPrivate { get; set; } = false;
        public bool EnableJoinNotification { get; set; } = true;
        public string Name { get; set; } = "";
        public string? Image { get; set; } = null;
        public string Description { get; set; } = "";
        public List<int> VisitorIds { get; set; } = new List<int>();
        public int MaxVisitorCount { get; set; }
        public List<int> TimePeriodIds { get; set; } = new List<int>();
        public List<string> Tags { get; set; } = new List<string>();


        [BsonIgnore]
        public Resource? Room { get; set; } = null;

        [BsonIgnore]
        public List<Resource> Devices { get; set; } = new List<Resource>();

        [BsonIgnore]
        public User? Organizer { get; set; } = null;

        [BsonIgnore]
        public List<User>? Visitors { get; set; } = null;

        [BsonIgnore]
        public List<TimePeriod>? Schedule { get; set; } = null;
    }
}
