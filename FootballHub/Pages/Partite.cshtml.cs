using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using HubCalcio.Services; // Assicurati che questo namespace sia corretto

namespace HubCalcio.Pages
{
    public class PartiteModel : PageModel
    {
        private readonly SportmonksService _api;

        public PartiteModel(SportmonksService api)
        {
            _api = api;
        }

        public JsonElement Partite { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DataSelezionata { get; set; }

        public async Task OnGetAsync()
        {
            // Se non è selezionata una data, usa quella odierna
            if (string.IsNullOrEmpty(DataSelezionata))
            {
                DataSelezionata = DateTime.Now.ToString("yyyy-MM-dd");
            }

            try
            {
                // Chiamata al servizio con il filtro ID 271 (Danimarca)
                Partite = await _api.GetPartitePerDataAsync(DataSelezionata);
            }
            catch (Exception ex)
            {
                // In caso di errore (es. 404), inizializziamo Partite come array vuoto
                // per evitare crash nella View
                var emptyDoc = JsonDocument.Parse("[]");
                Partite = emptyDoc.RootElement;
                ViewData["Errore"] = "Impossibile caricare le partite: " + ex.Message;
            }
        }
    }
}