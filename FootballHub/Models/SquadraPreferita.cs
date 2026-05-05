namespace FootballHub.Models;

public class SquadraPreferita
{
    public int Id { get; set; }
    public int UtenteId { get; set; }
    public int ApiSquadraId { get; set; }
    public string NomeSquadra { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string Campionato { get; set; } = string.Empty;

    public Utente? Utente { get; set; }
}
