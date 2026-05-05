using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using HubCalcio.Services;

namespace FootballHub.Pages
{
    public class PronosticiModel : PageModel
    {
        private readonly SportmonksService _api;
        public JsonElement PartiteConQuote { get; set; }
        public string Errore { get; set; }

        public PronosticiModel(SportmonksService api) => _api = api;

        public async Task OnGetAsync()
        {
            try
            {
                // Recuperiamo le partite di oggi (il service deve includere 'odds')
                // Se non hai un metodo specifico, usa quello delle partite filtrando per ID 271
                PartiteConQuote = await _api.GetPartitePerDataAsync(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                Errore = ex.Message;
                // Inizializzazione di sicurezza per evitare il crash del ValueKind
                PartiteConQuote = JsonDocument.Parse("[]").RootElement;
            }
        }
    }
}