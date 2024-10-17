using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AutoMapper;
using Company.Route.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Company.Route.DAL.Models;
using System.Configuration;
using Company.Route.PL.Settings;

#region Identity Package Services  

/*
  
 *  3 Main Services in Identity Package  * 
 
 ---------- 1. UserManager   -> Manage User      -------------- 

    [5 Functions]
    - Create User     (Sign Up)
    - Update User 
    - Delete User 
    - Read User Data 
    - Confirm Account (Email Confirmation)


 ---------- 2. SignInManager -> Authentaction    -------------- 

    [3 Functions]
    - Sign In 
    - Sign Out 
    - IsSIgned (Check if user is signed in)
    ++++ More Functions ++++
    - Reset Password
    - Two Factor Authentication
    - OTP Authentication
    - External Login (Google, Facebook, Twitter, Microsoft)


 ---------- 3. RoleManager   -> Manage Roles     -------------- 
    
    - Create Role
    - Update Role
    - Delete Role




 
 
 
 
 
 
 */


#endregion


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
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();

            // NOTE :: AFTER USING UNITOFWORK NO NEED TO INJECT REPOSITORIES WE INJECT UNITOFWORK AND USE IT TO ACCESS THE REPOSITORIES
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            // Allow AutoMapper , it needs object from mappingprofile 
            builder.Services.AddAutoMapper(typeof(MappingProfiles)); // transient lifetime

            // Services for User nad SignIn Managers after using identity
            #region Identity Servies
            //builder.Services.AddScoped<UserManager<ApplicationUser>>();
            //builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            //builder.Services.AddScoped<RoleManager<ApplicationUser>>(); 
            #endregion // instead of them we can use only one do the same
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
               {
                   //config.Password.RequiredUniqueChars = 2;
                   //config.Password.RequireDigit = true;
                   //config.Password.RequireLowercase = true;
                   //config.Password.RequireUppercase = true;
                   //config.Password.RequireNonAlphanumeric = true;
                   //config.User.RequireUniqueEmail = true;
                   //config.Lockout.MaxFailedAccessAttempts = 5; // attempts lock account after it
                   //config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); // time locked

                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()  ;

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });

            // MailKitSettings
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailService, EmailSettings>();

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
