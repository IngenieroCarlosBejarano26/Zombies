using Dapper;
using Microsoft.Data.SqlClient;
using ZombieHordeDefenseSystem.Domain.Entities;
using ZombieHordeDefenseSystem.Domain.Interfaces;
using ZombieHordeDefenseSystem.Infrastructure.Persistence.Conexion;

namespace ZombieHordeDefenseSystem.Infrastructure.Persistence.Repositories;

public class ZombieRepository(IConexionDB conexionDB) : IZombieRepository
{
    private readonly IConexionDB _conexionDB = conexionDB;
    private const string SP_OBTENER_LISTA_DE_ZOMBIES = "SP_OBTENER_LISTA_DE_ZOMBIES";

    public async Task<IReadOnlyList<Zombie>> ObtenerZombiesAsync()
    {
        using SqlConnection connection = _conexionDB.CrearConexionDB();
        await connection.OpenAsync();

        IEnumerable<Zombie> zombies = await connection.QueryAsync<Zombie>(
                sql: SP_OBTENER_LISTA_DE_ZOMBIES,
                commandType: System.Data.CommandType.StoredProcedure
            );

        return zombies.ToList().AsReadOnly();
    }
}