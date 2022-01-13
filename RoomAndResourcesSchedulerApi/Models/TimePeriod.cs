namespace RoomAndResourcesSchedulerApi.Models
{
    public class TimePeriod
    {
        public int Id { get; init; } = 0;
        public int? EventId { get; set; }
        public DateTime From { get; set; } 
        public DateTime To { get; set; }
        public bool OpenEnd { get; set; } = false;
    }
}
