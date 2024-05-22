using CustomerManagement.Models;
using CustomerManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Repository.Contexts
{
    public class CustomerManagementContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = default!;

        public CustomerManagementContext() { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow.ToLocalTime();
                        entry.Entity.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                        entry.Entity.IsDeleted = false;
                        entry.Entity.Active = true;
                        break;
                    case EntityState.Modified:
                        entry.Entity.CreatedAt = entry.Entity.CreatedAt.ToLocalTime();
                        entry.Entity.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                        break;
                    case EntityState.Deleted:
                        entry.Entity.IsDeleted = true;
                        entry.Entity.Active = false;
                        entry.Entity.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
