using Microsoft.AspNetCore.Identity;
using ZahimarProject.Models;

namespace ZahimarProject.Authentication
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
