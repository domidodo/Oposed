using LiteDB;

namespace RoomAndResourcesSchedulerApi.Models
{
    public class Template
    {
        public int Id { get; set; }
        public Event Data { get; set; }
    }
}
