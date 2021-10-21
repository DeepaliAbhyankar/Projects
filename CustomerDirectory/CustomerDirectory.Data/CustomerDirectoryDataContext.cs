using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerDirectory.Data
{
    public class CustomerDirectoryDataContext : DbContext
    {
        public CustomerDirectoryDataContext(DbContextOptions<CustomerDirectoryDataContext> options)
            : base(options)
        { }
        public DbSet<Customer> Customers { get; set; }
       
        public virtual Task<int> SaveChangesAsync(string userName)
        {
            AuditCustomer(userName);
            return base.SaveChangesAsync();
        }

        private void AuditCustomer(string userName)
        {
            var dateTime = DateTime.Now;
            var entries = ChangeTracker.Entries().Where(e => e.Entity is Customer && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach(var entry in entries)
            {
               switch(entry.State)
                {
                    case EntityState.Added:
                        ((Customer)entry.Entity).CreatedBy = userName;
                        ((Customer)entry.Entity).CreatedDate = dateTime;
                        ((Customer)entry.Entity).ModifiedBy = userName;
                        ((Customer)entry.Entity).ModifiedDate = dateTime;
                        break;
                    case EntityState.Modified:
                        entry.Property("CreatedDate").IsModified = false;
                        entry.Property("CreatedBy").IsModified = false;
                        ((Customer)entry.Entity).ModifiedBy = userName;
                        ((Customer)entry.Entity).ModifiedDate = dateTime;
                        break;
                }
            }

        }
    }   
}
