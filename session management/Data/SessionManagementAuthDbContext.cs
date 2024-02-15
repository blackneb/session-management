using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace session_management.Data
{
    public class SessionManagementAuthDbContext : IdentityDbContext
    {
        public SessionManagementAuthDbContext(DbContextOptions<SessionManagementAuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var AdminRoleId = "2";
            var UserRoleId = "3";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = AdminRoleId,
                    ConcurrencyStamp =AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole
                    {
                        Id=UserRoleId,
                        ConcurrencyStamp=UserRoleId,
                        Name = "User",
                        NormalizedName = "User".ToUpper()
                    }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}