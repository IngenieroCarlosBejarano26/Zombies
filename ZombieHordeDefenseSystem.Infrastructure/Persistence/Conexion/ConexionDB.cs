using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ZombieHordeDefenseSystem.Infrastructure.Persistence.Conexion;

public class ConexionDB(IConfiguration configuration) : IConexionDB
{
    private const string connectionString = "ZombieHordeDefenseSystem";
    private readonly IConfiguration _configuration = configuration;

    public SqlConnection CrearConexionDB()
    {
        return new SqlConnection(_configuration.GetConnectionString(connectionString));
    }
}