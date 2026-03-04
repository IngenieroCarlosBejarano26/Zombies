namespace ZombieHordeDefenseSystem.Domain.Entities;

public class Simulacion
{
    public Guid SimulacionId { get; set; }
    public int TiempoDisponible { get; set; }
    public int BalasDisponibles { get; set; }
}