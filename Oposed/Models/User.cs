using Oposed.Enum;

namespace Oposed.Models
{
    public record User
    {
        private string _avatar;

        public int Id { get; init; }
        public string AuthKey { get; set; }
        public string LdapDn { get; set; }
        public bool Active { get; set; } = false;
        public DateTime LastLogin { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { 
            get {
                if (!string.IsNullOrWhiteSpace(_avatar))
                {
                    return _avatar;
                }
                return "/img/DefaultUserAvatar.png";
            } 
            set { 
                _avatar = value; 
            }
        }

        public List<int> DisabledNewsletterIds { get; set; } = new List<int>();
    }
}
