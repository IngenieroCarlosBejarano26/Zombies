namespace ZombieHordeDefenseSystem.Domain.Entities;

public class Zombie
{
    public Guid ZombieId { get; set; }
    public string NombreTipoZombie { get; set; } = string.Empty;
    public int TiempoDisparo { get; set; }
    public int BalasNecesarias { get; set; }
    public int NivelAmenaza { get; set; }
    public int Puntaje { get; set; }
}