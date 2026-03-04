using MediatR;
using ZombieHordeDefenseSystem.Application.DTOs;
using ZombieHordeDefenseSystem.Application.EvenHandler;
using ZombieHordeDefenseSystem.Domain.Entities;
using ZombieHordeDefenseSystem.Domain.Interfaces;

namespace ZombieHordeDefenseSystem.Application.Querys;

public class CalcularEstrategiaHandler(
    IZombieRepository zombieRepository,
    ISimulacionRepository simulacionRepository,
    IPublisher publisher
    ) : IRequestHandler<CalcularEstrategiaQuery, CalcularEstrategiaRespuestaDTO>
{
    private readonly IZombieRepository _zombieRepository = zombieRepository;
    private readonly ISimulacionRepository _simulacionRepository = simulacionRepository;
    private readonly IPublisher _publisher = publisher;

    public async Task<CalcularEstrategiaRespuestaDTO> Handle(CalcularEstrategiaQuery request, CancellationToken cancellationToken)
    {
        // Formula Utilizada: dp[b][t] = max(dp[b][t], points + dp[b − bulletsRequired][t − timeRequired])
        // Tener en cuenta que esta formula es la mas optima para resolver el problema de la mochila, ya que permite calcular el puntaje máximo que se puede obtener con una cantidad limitada de balas y tiempo, considerando cada zombie como un "objeto" con un "peso" (balas necesarias y tiempo requerido) y un "valor" (puntaje).
        // sin embargo, es importante mencionar que esta formula asume que cada zombie solo puede ser eliminado una vez, lo cual es una restricción importante a tener en cuenta al implementar la solución.
        // pero si el numero de balas o tiempo es muy grande, esta solución puede ser ineficiente en términos de tiempo de ejecución, ya que el algoritmo tiene una complejidad de O(n*m), donde n es el número de zombies y m es el número de balas o tiempo disponible. En casos donde m es muy grande, esto puede llevar a tiempos de ejecución prolongados.

        IReadOnlyList<Zombie> zombies = await _zombieRepository.ObtenerZombiesAsync();

        int balas = request.BalasDisponibles;
        int tiempo = request.TiempoDisponible;
        int hordas = zombies.Count;

        int[,] dp = new int[balas + 1, tiempo + 1];
        bool[,,] zombieEliminado = new bool[hordas, balas + 1, tiempo + 1];

        for (int i = 0; i < hordas; i++)
        {
            int balasNecezarias = zombies.ElementAt(i).BalasNecesarias;
            int tiempoRequerido = zombies.ElementAt(i).TiempoDisparo;
            int puntaje = zombies.ElementAt(i).Puntaje;

            for (int b = balas; b >= balasNecezarias; b--)
            {
                for (int t = tiempo; t >= tiempoRequerido; t--)
                {
                    int puntajeObtenido = Math.Max(dp[b, t], puntaje + dp[b - balasNecezarias, t - tiempoRequerido]);

                    if (puntajeObtenido > dp[b, t])
                    {
                        dp[b, t] = puntajeObtenido;
                        zombieEliminado[i, b, t] = true;
                    }
                }
            }
        }

        List<Zombie> zombiesEliminados = [];
        int balasRestantes = balas;
        int tiempoRestante = tiempo;

        for (int i = hordas - 1; i >= 0; i--)
        {
            if (zombieEliminado[i, balasRestantes, tiempoRestante])
            {
                Zombie zombie = zombies.ElementAt(i);
                zombiesEliminados.Add(zombie);
                balasRestantes -= zombie.BalasNecesarias;
                tiempoRestante -= zombie.TiempoDisparo;
            }
        }

        Guid simulacionId = await _simulacionRepository.RegistrarSimulacionAsync(request.BalasDisponibles, request.TiempoDisponible);
        await _publisher.Publish(new FinalizarSimulacionEvent(simulacionId, zombiesEliminados), cancellationToken);

        return new CalcularEstrategiaRespuestaDTO
        {
            PuntajeTotal = dp[balas, tiempo],
            ZombiesEliminados = zombiesEliminados
        };
    }
}