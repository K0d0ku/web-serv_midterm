using Microsoft.AspNetCore.Identity;

namespace KuroApi.Models
{
    public class AppUser : IdentityUser
    {
        public string Nickname { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
    }
}
