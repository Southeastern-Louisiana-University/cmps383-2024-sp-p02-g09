using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Features.Users;

namespace Selu383.SP24.Api.Data;

public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DataContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var userRole = modelBuilder.Entity<UserRole>();
        userRole.HasKey(x => new {x.UserId, x.RoleId });
        userRole.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId);

        userRole.HasOne(x => x.User)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UserId);


        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}