namespace Oposed.Models
{
    public class Template
    {
        public int Id { get; set; }
        public bool IsPublic { get; set; } = false;
        public Event Data { get; set; }
    }
}
