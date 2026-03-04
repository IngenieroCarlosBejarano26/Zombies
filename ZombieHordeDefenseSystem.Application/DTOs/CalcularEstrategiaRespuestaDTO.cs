using ZombieHordeDefenseSystem.Domain.Entities;

namespace ZombieHordeDefenseSystem.Application.DTOs;

public class CalcularEstrategiaRespuestaDTO
{
    public int PuntajeTotal { get; set; }
    public IReadOnlyList<Zombie> ZombiesEliminados { get; set; } = [];
}