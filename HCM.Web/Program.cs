using HCM.Core.Middleware;
using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure;
using HCM.Infrastructure.Repositories;
using HCM.Infrastructure.Repositories.Implementations;
using HCM.Web.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace HCM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    //options.AccessDeniedPath = "/error";
                    options.LoginPath = "/Users/Login";
                });

            builder.Services.AddHttpContextAccessor(); 

            builder.Services.AddAuthorization(c =>
            {
                c.AddPolicy("Employee", p => p.RequireRole("Employee"));
                c.AddPolicy("Manager", p => p.RequireRole("Manager"));
                c.AddPolicy("HRAdmin", p => p.RequireRole("HRAdmin"));
            });

            builder.Services.AddDbContext<HCMContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HCMConnectionString"));
            });

            // add services/controllers
            {
                builder.Services.AddScoped<IUsersRepository, UsersRepository>();
                builder.Services.AddScoped<IUsersService, UsersService>();


                builder.Services.AddScoped<IEmployeesService, EmployeesService>();
                builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();

                builder.Services.AddScoped<ISalariesRepository, SalariesRepository>();

                builder.Services.AddScoped<UsersController>();
            }

            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();

            app.UseMiddleware<ErrorHandlingMiddleware>();


            app.Run();
        }
    }
}
