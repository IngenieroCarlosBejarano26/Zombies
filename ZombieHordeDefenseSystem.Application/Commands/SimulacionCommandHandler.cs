using MediatR;
using ZombieHordeDefenseSystem.Application.EvenHandler;
using ZombieHordeDefenseSystem.Domain.Interfaces;

namespace ZombieHordeDefenseSystem.Application.Commands
{
    public class SimulacionCommandHandler(IZombieEliminadoRepository zombieEliminadoRepository) : INotificationHandler<FinalizarSimulacionEvent>
    {
        private readonly IZombieEliminadoRepository _zombieEliminadoRepository = zombieEliminadoRepository;

        public async Task Handle(FinalizarSimulacionEvent request, CancellationToken cancellationToken)
        {
            await _zombieEliminadoRepository.RegistrarZombieEliminadoAsync(request.SimulacionId, request.Zombies);
        }
    }
}