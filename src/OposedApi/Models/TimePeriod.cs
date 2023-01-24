using LiteDB;

namespace OposedApi.Models
{
    public class TimePeriod
    {
        public int Id { get; init; } = 0;
        public int? EventId { get; set; }
        public DateTime From { get; set; } 
        public DateTime To { get; set; }
        public DateTime LastPing { get; set; } = DateTime.MinValue;
        
        [BsonIgnore]
        public bool IsExecutedOnTime
        {
            get
            {
                var now = DateTime.Now;

                if(From > now)
                    return true;
                
                // 6 min is the max waiting time between the pings
                if (To < now && (To - LastPing).TotalMinutes <= 6)
                    return true;

                if ((now - LastPing).TotalMinutes <= 6)
                    return true;

                return false;
            }
        }

        [BsonIgnore]
        public bool IsCurrentlsRunning
        {
            get
            {
                var now = DateTime.Now;
                
                // Use Ping if enabled
                if ((now - LastPing).TotalMinutes <= 6)
                    return true;

                // Is Ping not enabled use the normal time
                if (string.IsNullOrEmpty(Settings.PingKey) && From <= now && now <= To)
                    return true;

                return false;
            }
        }
    }
}
