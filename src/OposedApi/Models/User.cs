using OposedApi.Enum;

namespace OposedApi.Models
{
    public record User
    {
        public int Id { get; init; }
        public string AuthKey { get; set; }
        public string LdapDn { get; set; }
        public bool Active { get; set; } = false;
        public DateTime PasswordExpirationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Language { get; set; } = "en";
        public UserRole Role { get; set; } = UserRole.User;
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }
        public List<int> DisabledNewsletterIds { get; set; } = new List<int>();
    }
}
