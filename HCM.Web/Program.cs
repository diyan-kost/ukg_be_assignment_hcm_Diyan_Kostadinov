using HCM.Core.Middleware;
using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure;
using HCM.Infrastructure.Repositories;
using HCM.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace HCM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Index";
                });

            builder.Services.AddHttpContextAccessor(); 

            builder.Services.AddDbContext<HCMContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HCMConnectionString"));
            });

            builder.Services.AddApplicationServices();

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

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.Run();
        }
    }
}
