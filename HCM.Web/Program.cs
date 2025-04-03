using HCM.Core.Middleware;
using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure;
using HCM.Infrastructure.Repositories;
using HCM.Infrastructure.Repositories.Implementations;
using HCM.Web.Extensions;
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
                options.UseSqlServer(builder.Configuration.GetConnectionString("HCMConnectionString"),
                    options => options.MigrationsAssembly("HCM.Infrastructure"));
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
            // Add test data on start up when in dev env
            else
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<HCMContext>();

                dbContext.Database.Migrate();
                TestData.InitializeDbWithTestData(dbContext, true);
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
