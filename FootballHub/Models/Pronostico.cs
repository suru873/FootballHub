namespace FootballHub.Models;

public class Pronostico
{
    public int Id { get; set; }
    public int UtenteId { get; set; }
    public string SquadraCasa { get; set; } = string.Empty;
    public string SquadraOspite { get; set; } = string.Empty;
    public string RisultatoPronosticato { get; set; } = string.Empty;
    public DateTime DataPartita { get; set; }
    public DateTime DataInserimento { get; set; } = DateTime.Now;

    public Utente? Utente { get; set; }
}
