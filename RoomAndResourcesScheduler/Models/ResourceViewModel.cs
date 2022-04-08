namespace RoomAndResourcesScheduler.Models
{
    public class ResourceViewModel
    {
        public Resource Resource { get; set; }
        public List<EventWithSchedule> EventWithFrom { get; set; }
    }
}
