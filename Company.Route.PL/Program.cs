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

            #region Configure Services & Allow DI

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(Options =>
            {
                // Read it from AppSettings
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // NOTE :: AFTER USING UNITOFWORK NO NEED TO INJECT REPOSITORIES WE INJECT UNITOFWORK AND USE IT TO ACCESS THE REPOSITORIES
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MappingProfiles)); // transient lifetime
            // MailKitSettings
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailService, EmailSettings>();
            // SmsSettings
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddTransient<ISmsService, SmsService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
               {
               })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });

            #endregion

            var app = builder.Build();

            #region Configure HTTP Request & Middleware
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


            #endregion

            app.Run();
        }
    }
}
