using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure
{
    public class HCMContext : DbContext
    {
        public HCMContext(DbContextOptions<HCMContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salary>()
                .Property(s => s.Amount)
                .HasColumnType("decimal(10,2)"); 
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Salary> Salaries { get; set; }
    }
}
