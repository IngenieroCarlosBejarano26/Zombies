using MediatR;
using ZombieHordeDefenseSystem.Domain.Entities;

namespace ZombieHordeDefenseSystem.Application.EvenHandler
{
    public record FinalizarSimulacionEvent(Guid SimulacionId, IReadOnlyList<Zombie> Zombies) : INotification;
}