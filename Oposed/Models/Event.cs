namespace Oposed.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int RoomId { get; set; } = 0;
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

        public Room? Room { get; set; } = null;

        public User? Organizer { get; set; } = null;

        public List<User>? Visitors { get; set; } = null;
        public List<TimePeriod>? Schedule { get; set; } = null;
    }
}
