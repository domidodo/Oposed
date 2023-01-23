using System.ComponentModel.DataAnnotations;

namespace OposedApi.Models
{
    public class Authentication
    {
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
    }
}
