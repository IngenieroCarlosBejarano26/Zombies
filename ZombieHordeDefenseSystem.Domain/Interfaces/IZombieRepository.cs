using ZombieHordeDefenseSystem.Domain.Entities;

namespace ZombieHordeDefenseSystem.Domain.Interfaces;

public interface IZombieRepository
{
    Task<IReadOnlyList<Zombie>> ObtenerZombiesAsync();
}