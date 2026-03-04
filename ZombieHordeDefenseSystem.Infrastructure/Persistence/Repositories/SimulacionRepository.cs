using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using ZombieHordeDefenseSystem.Domain.Interfaces;
using ZombieHordeDefenseSystem.Infrastructure.Persistence.Conexion;

namespace ZombieHordeDefenseSystem.Infrastructure.Persistence.Repositories;

public class SimulacionRepository(IConexionDB conexionDB) : ISimulacionRepository
{
    private readonly IConexionDB _conexionDB = conexionDB;
    private const string SP_REGISTRAR_SIMULACION = "SP_REGISTRAR_SIMULACION";

    public async Task<Guid> RegistrarSimulacionAsync(int BalasDisponibles, int TiempoDisponible)
    {
        using SqlConnection connection = _conexionDB.CrearConexionDB();
        await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add(name: "@TiempoDisponible", value: TiempoDisponible, dbType: DbType.Int32);
        parameters.Add(name: "@BalasDisponibles", value: BalasDisponibles, dbType: DbType.Int32);
        parameters.Add(name: "@SimulacionId", value: Guid.NewGuid(), dbType: DbType.Guid, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            sql: SP_REGISTRAR_SIMULACION,
            param: parameters,
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<Guid>("@SimulacionId");
    }
}