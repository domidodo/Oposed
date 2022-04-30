
namespace OposedApi.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; } = null;
        public string Description { get; set; }
    }
}
