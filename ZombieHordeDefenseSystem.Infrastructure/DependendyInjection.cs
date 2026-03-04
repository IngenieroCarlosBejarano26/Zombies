using Microsoft.Extensions.DependencyInjection;
using ZombieHordeDefenseSystem.Domain.Interfaces;
using ZombieHordeDefenseSystem.Infrastructure.Persistence.Conexion;
using ZombieHordeDefenseSystem.Infrastructure.Persistence.Repositories;

namespace ZombieHordeDefenseSystem.Infrastructure;

public static class DependendyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IConexionDB, ConexionDB>();
        services.AddScoped<IZombieRepository, ZombieRepository>();
        services.AddScoped<ISimulacionRepository, SimulacionRepository>();
        services.AddScoped<IZombieEliminadoRepository, ZombieEliminadoRepository>();

        return services;
    }
}