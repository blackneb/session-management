using Microsoft.EntityFrameworkCore;
using session_management.Models;

namespace session_management.Data
{
    public class SessionManagementDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserKeyModel> UserKeys { get; set; }
        public DbSet<KeyModel> Keys { get; set; }
        public DbSet<KeyExtensionModel> KeyExtensions { get; set; }
        public DbSet<AdminModel> Admins { get; set; }

        public SessionManagementDbContext(DbContextOptions<SessionManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add any additional configuration for your models if needed.
            // For example, configuring relationships, indexes, etc.
        }
    }
}
