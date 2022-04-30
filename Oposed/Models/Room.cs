using Oposed.Enum;

namespace Oposed.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; } = null;
        public string Description { get; set; }
    }
}
