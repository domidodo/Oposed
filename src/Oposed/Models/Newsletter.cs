namespace Oposed.Models
{
    public class Newsletter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
