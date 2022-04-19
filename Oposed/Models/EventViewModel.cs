namespace Oposed.Models
{
    public class EventViewModel
    {
        public Event Event { get; set; } = new Event();
        public List<Resource> Resources { get; set; } = new List<Resource>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<Template> Templates { get; set; } = new List<Template>();
    }
}
