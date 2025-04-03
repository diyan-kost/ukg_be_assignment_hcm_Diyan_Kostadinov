using HCM.Core.Services.Implementations;
using HCM.Core.Services;
using HCM.Infrastructure.Repositories.Implementations;
using HCM.Infrastructure.Repositories;

namespace HCM.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();

            services.AddScoped<ISalariesService, SalariesService>();
            services.AddScoped<ISalariesRepository, SalariesRepository>();

            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRolesService, RolesService>();

            return services;
        }
    }
}
