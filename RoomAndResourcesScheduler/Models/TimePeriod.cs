namespace RoomAndResourcesScheduler.Models
{
    public class TimePeriod
    {
        public DateTime From { get; set; } 
        public DateTime To { get; set; }
        public bool OpenEnd { get; set; } = false;
    }
}
