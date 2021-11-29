using Microsoft.AspNetCore.Identity;

namespace EShop.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set;}
    }
}
