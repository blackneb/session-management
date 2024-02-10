using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }
        }
    }
}
