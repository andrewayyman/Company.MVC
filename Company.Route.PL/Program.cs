using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Company.Route.PL
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            #region Dependency Injection Services
            // LIFE TIME OF SERVICES
            //builder.Services.AddScoped<AppDbContext>();    // Per Request, Runs while request is running
            //builder.Services.AddSingleton<AppDbContext>(); // Per Application, Runs while application is running
            //builder.Services.AddTransient<AppDbContext>(); // Per operation, Runs every time when we call the operation 

            #endregion
            // Extension method to apply DI include all services [ scoped , singelton , transient ] ,default is scoped
            builder.Services.AddDbContext<AppDbContext>(Options =>
            {
                // Read it from AppSettings
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // means when we ask for IDepartmentRepository , create object of DepartmentRepository
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); 
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); 


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if ( !app.Environment.IsDevelopment() )
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
