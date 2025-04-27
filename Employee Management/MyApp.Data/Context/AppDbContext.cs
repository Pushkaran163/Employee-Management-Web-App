using Microsoft.EntityFrameworkCore;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Context
{
    /// <summary>
    /// Application database context for EF Core.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>Employee table</summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>Department table</summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary>Model relationships and configurations</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne<Department>()
                .WithMany()
                .HasForeignKey(e => e.DepartmentId);
        }
    }
}
