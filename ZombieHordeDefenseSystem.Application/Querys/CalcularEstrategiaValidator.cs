using FluentValidation;

namespace ZombieHordeDefenseSystem.Application.Querys;

public class CalcularEstrategiaValidator : AbstractValidator<CalcularEstrategiaQuery>
{
    public CalcularEstrategiaValidator()
    {
        RuleFor(x => x.BalasDisponibles)
            .GreaterThanOrEqualTo(0).WithMessage("Las balas disponibles deben ser positivo.");
        RuleFor(x => x.TiempoDisponible)
            .GreaterThanOrEqualTo(0).WithMessage("El tiempo disponible debe ser positivo.");
    }
}