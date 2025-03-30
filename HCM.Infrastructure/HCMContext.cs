using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure
{
    public class HCMContext : DbContext
    {
        public HCMContext(DbContextOptions<HCMContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);


        //    modelBuilder.Entity<User>()
        //        .HasOne(u => u.Role)
        //        .WithMany()
        //        .HasForeignKey(u => u.RoleId);
        //        //.OnDelete(DeleteBehavior.Restrict);
        //}

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
