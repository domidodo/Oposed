namespace RoomAndResourcesScheduler.Models
{
    public class EventWithSchedule
    {
        public Event Event { get; set; }
        public TimePeriod Schedule { get; set; }
    }
}
