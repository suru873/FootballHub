using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using HubCalcio.Services; // Controlla che il namespace del tuo service sia corretto

namespace FootballHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SportmonksService _api;

        // DEFINISCI QUESTE DUE PROPRIETÀ
        public JsonElement Livescores { get; set; }
        public string Errore { get; set; } = string.Empty;

        public IndexModel(SportmonksService api) => _api = api;

        public async Task OnGetAsync()
        {
            try
            {
                // Carichiamo i dati
                Livescores = await _api.GetPartitePerDataAsync(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                // Se c'è un errore, lo salviamo qui
                Errore = "Si è verificato un problema: " + ex.Message;
                // Inizializziamo Livescores come array vuoto per non far crashare la pagina
                Livescores = JsonDocument.Parse("[]").RootElement;
            }
        }
    }
}