using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Avatar { get; set; }
        public ApplicationUser()
        {
            this.Avatar = "/avatars/default-avatar.jpg";
        }
    }
}
