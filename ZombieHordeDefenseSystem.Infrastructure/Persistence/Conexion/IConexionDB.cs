using Microsoft.Data.SqlClient;

namespace ZombieHordeDefenseSystem.Infrastructure.Persistence.Conexion;

public interface IConexionDB
{
    SqlConnection CrearConexionDB();
}