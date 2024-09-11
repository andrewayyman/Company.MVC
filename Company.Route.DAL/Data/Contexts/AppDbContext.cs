using Company.Route.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        // Automatically Chaining on parameterless constructor , we can chain on the parameterized constructor which take DbContectOptions as parameter
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        //{
        //    optionsBuilder.UseSqlServer(" Server=WILDRABBIT; Database=CompanyMVC; Trusted_Connection=True; TrustServerCertificate = True ");
        //}

        // to use config class
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Department> Departments { get; set; }

    }
}
