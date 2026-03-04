using MediatR;
using ZombieHordeDefenseSystem.Application.DTOs;

namespace ZombieHordeDefenseSystem.Application.Querys;

public record CalcularEstrategiaQuery(int BalasDisponibles, int TiempoDisponible) : IRequest<CalcularEstrategiaRespuestaDTO>;