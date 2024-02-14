using Microsoft.AspNetCore.Identity;

namespace Selu383.SP24.Api.Features.Users
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> Users { get; set; } = new List<UserRole>();

    }

    public class RoleNames
    {
        public const string Admin = nameof(Admin);

        public const string User = nameof(User);
    }
}
