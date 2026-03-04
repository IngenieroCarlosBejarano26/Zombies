using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using ZombieHordeDefenseSystem.Domain.Entities;
using ZombieHordeDefenseSystem.Domain.Interfaces;
using ZombieHordeDefenseSystem.Infrastructure.Persistence.Conexion;

namespace ZombieHordeDefenseSystem.Infrastructure.Persistence.Repositories;

public class ZombieEliminadoRepository(IConexionDB conexionDB) : IZombieEliminadoRepository
{
    private readonly IConexionDB _conexionDB = conexionDB;
    private const string SP_REGISTRAR_ZOMBIES_ELIMINADOS_POR_SIMULACION = "SP_REGISTRAR_ZOMBIES_ELIMINADOS_POR_SIMULACION";

    public async Task RegistrarZombieEliminadoAsync(Guid simulacionId, IReadOnlyList<Zombie> zombies)
    {
        DataTable dataTable = new();
        dataTable.Columns.Add("ZombieId", typeof(Guid));
        dataTable.Columns.Add("PuntosObtenidos", typeof(int));

        using SqlConnection connection = _conexionDB.CrearConexionDB();
        await connection.OpenAsync();

        DynamicParameters dynamicParameters = new();

        foreach (var zombie in zombies)
        {
            dataTable.Rows.Add(zombie.ZombieId, zombie.Puntaje);
        }

        dynamicParameters.Add("@Zombies", dataTable.AsTableValuedParameter("ZombieEliminadoType"));
        dynamicParameters.Add("@SimulacionId", simulacionId);

        await connection.ExecuteAsync(sql: SP_REGISTRAR_ZOMBIES_ELIMINADOS_POR_SIMULACION, param: dynamicParameters, commandType: CommandType.StoredProcedure);
    }
}