using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Data.Contexts
{
    // we inherted from IdentityDbContext to use Identity Package
    // we have 7 classes in IdentityDbContext added to db 
    public class AppDbContext : IdentityDbContext
    {
        // Automatically Chaining on parameterless constructor , we can chain on the parameterized constructor which take DbContectOptions as parameter
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        //{
        //    optionsBuilder.UseSqlServer(" Server=WILDRABBIT; Database=CompanyMVC; Trusted_Connection=True; TrustServerCertificate = True ");
        //}

        // to run fluent api
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>()
                        .ToTable("Roles");
            modelBuilder.Entity<IdentityUser>()
                        .ToTable("Users");

        }


        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
