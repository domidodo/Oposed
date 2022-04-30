namespace Oposed.Models
{
    public class EventViewModel
    {
        public Event Event { get; set; } = new Event();
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<Template> Templates { get; set; } = new List<Template>();
    }
}
