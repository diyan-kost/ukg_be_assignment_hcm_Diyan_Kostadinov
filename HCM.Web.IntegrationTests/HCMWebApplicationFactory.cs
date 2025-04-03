using HCM.Infrastructure;
using HCM.Web.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HCM.Web.IntegrationTests
{
    public class HCMWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configure In-Memory Database for testing
                var dbContextOptions = new DbContextOptionsBuilder<HCMContext>()
                    .UseInMemoryDatabase("TestDatabase")
                    .Options;

                var dbContext = new HCMContext(dbContextOptions);
                services.AddSingleton(dbContext);

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<HCMContext>();

                    context.Database.EnsureCreated();

                    TestDbHelper.InitializeDbForTests(context);
                }

                services.AddAuthorization();
                services.AddRazorPages().AddRazorPagesOptions(o => o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));
            });
        }
    }
}
