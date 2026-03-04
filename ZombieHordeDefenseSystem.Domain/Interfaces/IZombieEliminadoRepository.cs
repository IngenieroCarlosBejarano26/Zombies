using ZombieHordeDefenseSystem.Domain.Entities;

namespace ZombieHordeDefenseSystem.Domain.Interfaces;

public interface IZombieEliminadoRepository
{
    Task RegistrarZombieEliminadoAsync(Guid simulacionId, IReadOnlyList<Zombie> zombies);
}