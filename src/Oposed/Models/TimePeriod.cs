namespace Oposed.Models
{
    public class TimePeriod
    {
        public int Id { get; init; } = 0;
        public int? EventId { get; set; }
        public DateTime From { get; set; } 
        public DateTime To { get; set; }
        public DateTime LastPing { get; set; } = DateTime.MinValue;
        public bool IsExecutedOnTime { get; set; } = true;
        public bool IsCurrentlsRunning { get; set; } = false;
    }
}
