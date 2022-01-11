namespace RoomAndResourcesSchedulerApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int OrganizerId { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool EnableJoinNotification { get; set; } = true;
        public string Name { get; set; } = "";
        public string? Image { get; set; } = null;
        public string Description { get; set; } = "";
        public List<int> VisitorIds { get; set; } = new List<int>();
        public int MaxVisitorCount { get; set; }
        public List<TimePeriod> Schedule { get; set; } = new List<TimePeriod>();
        public List<string> Tags { get; set; } = new List<string>();

        public long Ticks { 
            get {
                DateTime now = DateTime.Now;
                List<long> tickList = new List<long>();
                foreach (TimePeriod schedule in Schedule)
                {
                    tickList.Add(schedule.To.Ticks - now.Ticks);
                }
                tickList.Sort();
                foreach (long tick in tickList)
                {
                    if (tick > 0)
                        return tick;
                }
                return -100;
            }
        }
    }
}
