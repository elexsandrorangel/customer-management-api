using CustomerManagement.Models;
using CustomerManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Repository.Contexts
{
    public class CustomerManagementContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = default!;

        public CustomerManagementContext(DbContextOptions<CustomerManagementContext> options)
            : base(options) 
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow.ToLocalTime();
                        entry.Entity.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                        break;
                    case EntityState.Modified:
                        entry.Entity.CreatedAt = entry.Entity.CreatedAt.ToLocalTime();
                        entry.Entity.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
