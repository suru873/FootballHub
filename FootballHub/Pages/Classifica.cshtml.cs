using HubCalcio.Services; // Controlla che questo namespace sia corretto
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace HubCalcio // <--- Deve corrispondere a quello della pagina .cshtml
{
    public class ClassificaModel : PageModel
    {
        private readonly SportmonksService _api;
        public JsonElement Classifica { get; set; }
        public string Errore { get; set; } = string.Empty;

        public ClassificaModel(SportmonksService api) => _api = api;

        public async Task OnGetAsync()
        {
            try
            {
                var data = await _api.GetClassificaDanimarcaAsync();

                if (data.ValueKind == JsonValueKind.Array && data.GetArrayLength() > 0)
                {
                    // Cerchiamo le righe della classifica nel primo gruppo
                    var primoGruppo = data[0];
                    if (primoGruppo.TryGetProperty("standings", out var standings))
                    {
                        // ORDINE: Forza l'ordinamento per posizione (1, 2, 3...)
                        var listaOrdinata = standings.EnumerateArray()
                            .OrderBy(x => x.GetProperty("position").GetInt32())
                            .ToList();

                        Classifica = JsonDocument.Parse(JsonSerializer.Serialize(listaOrdinata)).RootElement;
                    }
                }
            }
            catch (Exception ex)
            {
                Errore = "Impossibile caricare i dati: " + ex.Message;
                Classifica = JsonDocument.Parse("[]").RootElement;
            }
        }
    }
}