namespace ZombieHordeDefenseSystem.Domain.Interfaces;

public interface ISimulacionRepository
{
    Task<Guid> RegistrarSimulacionAsync(int BalasDisponibles, int TiempoDisponible);
}