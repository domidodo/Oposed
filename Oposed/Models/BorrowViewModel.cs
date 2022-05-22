namespace Oposed.Models
{
    public class BorrowViewModel
    {
        public int DeviceId { get; set; }
        public List<Resource> Resources { get; set; } = new List<Resource>();
    }
}
