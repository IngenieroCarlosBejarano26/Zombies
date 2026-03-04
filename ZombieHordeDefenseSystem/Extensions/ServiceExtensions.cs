using ZombieHordeDefenseSystem.Application;
using ZombieHordeDefenseSystem.Infrastructure;

namespace ZombieHordeDefenseSystem.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddProjectServices(
      this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddApplication();
            services.AddInfrastructure();

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();


                });
            });
            return services;
        }
    }
}
