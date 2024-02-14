﻿using Microsoft.AspNetCore.Identity;

namespace Selu383.SP24.Api.Features.Users
{
    public class User : IdentityUser<int>
    {
        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

    }
}
