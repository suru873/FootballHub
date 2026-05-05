namespace FootballHub.Models;

public class Utente
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime DataRegistrazione { get; set; } = DateTime.Now;

    public List<SquadraPreferita> SquadrePreferite { get; set; } = new();
    public List<Pronostico> Pronostici { get; set; } = new();
}
